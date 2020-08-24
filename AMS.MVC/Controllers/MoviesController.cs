using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using AMS.MVC.Exceptions.Movie;
using AMS.MVC.Services;
using AMS.MVC.ViewModels.MovieViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IFlashMessage _flashMessage;

        public MoviesController(IMovieService movieService, IFlashMessage flashMessage)
        {
            _movieService = movieService;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _movieService.GetMoviesList();
            
            return View(viewModel);
        }
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Show(Guid id)
        {
            try
            {
                var viewModel = await _movieService.GetMovie(id);

                return View(viewModel);
            }
            catch (MovieNotFound)
            {
                return NotFound();
            }
        }
        
        [Authorize(Roles = "Manager, Administrator")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await _movieService.FillCreateViewModelWithGenresAndPersons();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieCreateViewModel movieCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                await _movieService.CreateMovie(movieCreateViewModel);
                
                _flashMessage.Confirmation("Movie has been created.");
                
                return RedirectToAction(nameof(Index));
            }

            var viewModel = await _movieService.FillCreateViewModelWithGenresAndPersons(movieCreateViewModel);

            return View(viewModel);
        }
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var viewModel = await _movieService.GetMovieEdit(id);

                return View(viewModel);
            }
            catch (MovieNotFound)
            {
                return NotFound();
            }
            catch (AuthenticationException)
            {
                return Forbid();
            }
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MovieEditViewModel movieEditViewModel)
        {
            if (ModelState.IsValid)
            {
                await _movieService.MovieEdit(id, movieEditViewModel);

                _flashMessage.Confirmation("Movie has been updated.");
                
                return RedirectToAction(nameof(Show), new { id });
            }

            movieEditViewModel = await _movieService.FillEditViewModelWithGenresAndPersons(movieEditViewModel);

            return View(movieEditViewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            try
            {
                var movie = await _movieService.GetMovieToConfirmDelete(id);

                return View(movie);
            }
            catch (MovieNotFound)
            {
                return NotFound();
            }
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _movieService.DeleteMovie(id);
                
                return RedirectToAction(nameof(Index));
            }
            catch (MovieNotFound)
            {
                return NotFound();
            }
        }
    }
}
