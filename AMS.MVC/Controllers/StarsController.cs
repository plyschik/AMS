using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using AMS.MVC.Exceptions.Movie;
using AMS.MVC.Services;
using AMS.MVC.ViewModels.MovieStarViewModel;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    public class StarsController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly IStarService _starService;

        public StarsController(IFlashMessage flashMessage, IStarService starService)
        {
            _flashMessage = flashMessage;
            _starService = starService;
        }

        [HttpGet("[controller]/[action]/{movieId:guid}")]
        public async Task<IActionResult> Create(Guid movieId)
        {
            try
            {
                var viewModel = await _starService.FillCreateViewModelWithAvailableStars(movieId);

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

        [HttpPost("[controller]/[action]/{movieId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid movieId, MovieStarCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _starService.CreateMovieStar(movieId, viewModel);

                    _flashMessage.Confirmation("Star has been attached to movie as character.");

                    return RedirectToAction("Show", "Movies", new {id = movieId});
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

            viewModel = await _starService.FillCreateViewModelWithAvailableStars(movieId, viewModel);

            return View(viewModel);
        }
        
        [HttpGet("[controller]/[action]/{movieId:guid}/{personId:guid}")]
        public async Task<IActionResult> Edit(Guid movieId, Guid personId)
        {
            try
            {
                var viewModel = await _starService.GetMovieStarEdit(movieId, personId);

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
        
        [HttpPost("[controller]/[action]/{movieId:guid}/{personId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid movieId, Guid personId, MovieStarEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _starService.MovieStarEdit(movieId, personId, viewModel);
                    
                    _flashMessage.Confirmation("Movie character has been updated.");
                
                    return RedirectToAction("Show", "Movies", new { id = movieId });
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
            
            return View(viewModel);
        }
        
        [HttpGet("[controller]/[action]/{movieId:guid}/{personId:guid}")]
        public async Task<IActionResult> ConfirmDelete(Guid movieId, Guid personId)
        {
            try
            {
                var movieStar = await _starService.GetMovieStarToConfirmDelete(movieId, personId);

                return View(movieStar);
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
        
        [HttpPost("[controller]/[action]/{movieId:guid}/{personId:guid}")]
        public async Task<IActionResult> Delete(Guid movieId, Guid personId)
        {
            try
            {
                await _starService.DeleteMovieStar(movieId, personId);
                
                return RedirectToAction("Show", "Movies", new { id = movieId });
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
    }
}
