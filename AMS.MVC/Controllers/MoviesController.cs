using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Authorization;
using AMS.MVC.Data;
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
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IFlashMessage _flashMessage;

        public MoviesController(
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager,
            IMovieRepository movieRepository,
            IGenreRepository genreRepository,
            IFlashMessage flashMessage
        )
        {
            _authorizationService = authorizationService; 
            _userManager = userManager;
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepository.GetAllWithGenres();
            
            return View(movies);
        }
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Show(Guid id)
        {
            var movie = await _movieRepository.GetByIdWithGenres(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var genres = await _genreRepository.GetAll();

            var viewModel = new MovieCreateViewModel();
            foreach (var genre in genres)
            {
                viewModel.Genres.Add(new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString()
                });
            }

            return View(viewModel);
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

                await _movieRepository.Create(movie);
                
                _flashMessage.Confirmation("Movie has been created.");
                
                return RedirectToAction(nameof(Index));
            }

            return View(movieCreateViewModel);
        }
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var movie = await _movieRepository.GetById(id);

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

            return View(movie);
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("Id, Title, Description, ReleaseDate, UserId")] Movie movie
        )
        {
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User,
                movie,
                MovieOperations.Edit
            );

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _movieRepository.Update(movie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _movieRepository.Exists(movie.Id))
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }

                _flashMessage.Confirmation("Movie has been updated.");
                
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            var movie = await _movieRepository.GetById(id);

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
            var movie = await _movieRepository.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            await _movieRepository.Delete(movie);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
