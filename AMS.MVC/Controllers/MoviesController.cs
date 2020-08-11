using System;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Authorization;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using AMS.MVC.ViewModels.MovieViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFlashMessage _flashMessage;

        public MoviesController(
            IUnitOfWork unitOfWork,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager,
            IFlashMessage flashMessage
        )
        {
            _unitOfWork = unitOfWork;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _unitOfWork.MovieRepository.GetAllWithGenres();
            
            return View(movies);
        }
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Show(Guid id)
        {
            var movie = await _unitOfWork.MovieRepository.GetByIdWithGenres(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var genres = await _unitOfWork.GenreRepository.GetAll();

            return View(new MovieCreateViewModel
            {
                Genres = genres.Select(genre => new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString()
                }).ToList()
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieCreateViewModel movieCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movie
                {
                    Title = movieCreateViewModel.Title,
                    Description = movieCreateViewModel.Description,
                    ReleaseDate = movieCreateViewModel.ReleaseDate,
                    User = await _userManager.GetUserAsync(HttpContext.User)
                };

                foreach (var genreId in movieCreateViewModel.SelectedGenres)
                {
                    movie.MovieGenres.Add(new MovieGenre
                    {
                        GenreId = Guid.Parse(genreId)
                    });
                }

                _unitOfWork.MovieRepository.Create(movie);
                await _unitOfWork.SaveChangesAsync();
                
                _flashMessage.Confirmation("Movie has been created.");
                
                return RedirectToAction(nameof(Index));
            }

            var genres = await _unitOfWork.GenreRepository.GetAll();
            
            movieCreateViewModel.Genres = genres.Select(genre => new SelectListItem
            {
                Text = genre.Name,
                Value = genre.Id.ToString(),
                Selected = movieCreateViewModel.SelectedGenres.Contains(genre.Id.ToString())
            }).ToList();
            
            return View(movieCreateViewModel);
        }
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var movie = await _unitOfWork.MovieRepository.GetByIdWithGenres(id);

            if (movie == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User,
                movie,
                MovieOperations.Edit
            );

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            var genres = await _unitOfWork.GenreRepository.GetAll();

            return View(new MovieEditViewModel
            {
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Genres = genres.Select(genre => new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString(),
                    Selected = movie.MovieGenres.Select(mg => mg.Genre).Contains(genre)
                }).ToList()
            });
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MovieEditViewModel movieEditViewModel)
        {
            var movie = await _unitOfWork.MovieRepository.GetByIdWithGenres(id);
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User,
                movie,
                MovieOperations.Edit
            );

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    movie.Title = movieEditViewModel.Title;
                    movie.Description = movieEditViewModel.Description;
                    movie.ReleaseDate = movieEditViewModel.ReleaseDate;
                    
                    var attachedGenreIds = movie.MovieGenres.Select(mg => mg.GenreId.ToString()).ToArray();
                    var selectedGenreIds = movieEditViewModel.SelectedGenres ??= new string[] {};
                    var genresIdsToAttach = selectedGenreIds.Except(attachedGenreIds).ToArray();
                    var genresIdsToDetach = attachedGenreIds.Except(selectedGenreIds).ToArray();

                    movie.MovieGenres = movie.MovieGenres.Where(
                        mg => !genresIdsToDetach.Contains(mg.GenreId.ToString())
                    ).ToList();
                    
                    foreach (var genreId in genresIdsToAttach)
                    {
                        movie.MovieGenres.Add(new MovieGenre
                        {
                            GenreId = Guid.Parse(genreId)
                        });
                    }
                    
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.MovieRepository.Exists(movie.Id))
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }

                _flashMessage.Confirmation("Movie has been updated.");
                
                return RedirectToAction(nameof(Show), new { id = movie.Id });
            }

            var genres = await _unitOfWork.GenreRepository.GetAll();

            movieEditViewModel.Genres = genres.Select(genre => new SelectListItem
            {
                Text = genre.Name,
                Value = genre.Id.ToString(),
                Selected = movie.MovieGenres.Select(mg => mg.Genre).Contains(genre)
            }).ToList();
            
            return View(movieEditViewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            _unitOfWork.MovieRepository.Delete(movie);
            await _unitOfWork.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
