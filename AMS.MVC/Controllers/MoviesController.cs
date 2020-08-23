using System;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Authorization;
using AMS.MVC.Data.Models;
using AMS.MVC.Repositories;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFlashMessage _flashMessage;

        public MoviesController(
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IFlashMessage flashMessage
        )
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _unitOfWork.Movies.GetAllWithRelations();
            
            return View(movies);
        }
        
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Show(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdWithRelations(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        
        [Authorize(Roles = "Manager, Administrator")]
        public IActionResult Create()
        {
            var genres = _unitOfWork.Genres.GetAll();
            var persons = _unitOfWork.Persons.GetAll();

            return View(new MovieCreateViewModel
            {
                Genres = genres.Select(genre => new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString()
                }).ToList(),
                Persons = persons.Select(person => new SelectListItem
                {
                    Text = person.FullName,
                    Value = person.Id.ToString()
                }).ToList()
            });
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Administrator")]
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

                await _unitOfWork.Movies.Create(movie);
                
                foreach (var genreId in movieCreateViewModel.SelectedGenres)
                {
                    movie.MovieGenres.Add(new MovieGenre
                    {
                        GenreId = Guid.Parse(genreId)
                    });
                }

                foreach (var personId in movieCreateViewModel.SelectedDirectors)
                {
                    movie.MovieDirectors.Add(new MovieDirector
                    {
                        PersonId = Guid.Parse(personId)
                    });
                }
                
                foreach (var personId in movieCreateViewModel.SelectedWriters)
                {
                    movie.MovieWriters.Add(new MovieWriter
                    {
                        PersonId = Guid.Parse(personId)
                    });
                }
                
                await _unitOfWork.Save();
                
                _flashMessage.Confirmation("Movie has been created.");
                
                return RedirectToAction(nameof(Index));
            }

            var genres = _unitOfWork.Genres.GetAll();
            var persons = _unitOfWork.Persons.GetAll();
            
            movieCreateViewModel.Genres = genres.Select(genre => new SelectListItem
            {
                Text = genre.Name,
                Value = genre.Id.ToString()
            }).ToList();
            
            movieCreateViewModel.Persons = persons.Select(person => new SelectListItem
            {
                Text = person.FullName,
                Value = person.Id.ToString()
            }).ToList();
            
            return View(movieCreateViewModel);
        }
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdWithRelations(id);

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

            var genres = _unitOfWork.Genres.GetAll();
            var persons = _unitOfWork.Persons.GetAll();

            return View(new MovieEditViewModel
            {
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Genres = genres.Select(genre => new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString(),
                }).ToList(),
                Persons = persons.Select(person => new SelectListItem
                {
                    Text = person.FullName,
                    Value = person.Id.ToString(),
                }).ToList(),
                SelectedGenres = movie.MovieGenres.Select(mg => mg.GenreId.ToString()).ToArray(),
                SelectedDirectors = movie.MovieDirectors.Select(md => md.PersonId.ToString()).ToArray(),
                SelectedWriters = movie.MovieWriters.Select(mw => mw.PersonId.ToString()).ToArray()
            });
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MovieEditViewModel movieEditViewModel)
        {
            var movie = await _unitOfWork.Movies.GetByIdWithRelations(id);
            
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
                    
                    var attachedDirectorsIds = movie.MovieDirectors.Select(md => md.PersonId.ToString()).ToArray();
                    var selectedDirectorsIds = movieEditViewModel.SelectedDirectors ??= new string[] {};
                    var directorsIdsToAttach = selectedDirectorsIds.Except(attachedDirectorsIds).ToArray();
                    var directorsIdsToDetach = attachedDirectorsIds.Except(selectedDirectorsIds).ToArray();

                    movie.MovieDirectors = movie.MovieDirectors.Where(
                        md => !directorsIdsToDetach.Contains(md.PersonId.ToString())
                    ).ToList();
                    
                    foreach (var personId in directorsIdsToAttach)
                    {
                        movie.MovieDirectors.Add(new MovieDirector
                        {
                            PersonId = Guid.Parse(personId)
                        });
                    }
                    
                    var attachedWritersIds = movie.MovieWriters.Select(md => md.PersonId.ToString()).ToArray();
                    var selectedWritersIds = movieEditViewModel.SelectedWriters ??= new string[] {};
                    var writersIdsToAttach = selectedWritersIds.Except(attachedWritersIds).ToArray();
                    var writersIdsToDetach = attachedWritersIds.Except(selectedWritersIds).ToArray();

                    movie.MovieWriters = movie.MovieWriters.Where(
                        md => !writersIdsToDetach.Contains(md.PersonId.ToString())
                    ).ToList();
                    
                    foreach (var personId in writersIdsToAttach)
                    {
                        movie.MovieWriters.Add(new MovieWriter
                        {
                            PersonId = Guid.Parse(personId)
                        });
                    }
                    
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.Movies.Exists(m => m.Id == movie.Id))
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }

                _flashMessage.Confirmation("Movie has been updated.");
                
                return RedirectToAction(nameof(Show), new { id = movie.Id });
            }

            var genres = _unitOfWork.Genres.GetAll();
            var persons = _unitOfWork.Persons.GetAll();

            movieEditViewModel.Genres = genres.Select(genre => new SelectListItem
            {
                Text = genre.Name,
                Value = genre.Id.ToString(),
            }).ToList();
            
            movieEditViewModel.Persons = persons.Select(genre => new SelectListItem
            {
                Text = genre.FullName,
                Value = genre.Id.ToString(),
            }).ToList();
            
            return View(movieEditViewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == id);

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
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            _unitOfWork.Movies.Delete(movie);
            await _unitOfWork.Save();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
