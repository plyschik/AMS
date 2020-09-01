using System;
using System.Threading.Tasks;
using AMS.MVC.Exceptions.Genre;
using AMS.MVC.Services;
using AMS.MVC.ViewModels.GenreViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IFlashMessage _flashMessage;

        public GenresController(IGenreService genreService, IFlashMessage flashMessage)
        {
            _genreService = genreService;
            _flashMessage = flashMessage;
        }

        [AllowAnonymous]
        public IActionResult Index(int page = 1)
        {
            var viewModel = _genreService.GetGenresList(page);
            
            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Show(Guid id)
        {
            try
            {
                var viewModel = await _genreService.GetGenreWithMovies(id);
                
                return View(viewModel);
            }
            catch (GenreNotFoundException)
            {
                return NotFound();
            }
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreCreateViewModel genreCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                await _genreService.CreateGenre(genreCreateViewModel);
                
                _flashMessage.Confirmation("Genre has been created.");

                return RedirectToAction(nameof(Index));
            }

            return View(genreCreateViewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var viewModel = await _genreService.GetEditViewModel(id);
                
                return View(viewModel);
            }
            catch (GenreNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, GenreEditViewModel genreEditViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _genreService.UpdateGenre(id, genreEditViewModel);
                                
                    _flashMessage.Confirmation("Genre has been updated.");
                
                    return RedirectToAction(nameof(Index));
                }
                catch (GenreNotFoundException)
                {
                    return NotFound();
                }
            }
            
            return View(genreEditViewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            try
            {
                var genre = await _genreService.GetGenreToConfirmDelete(id);

                return View(genre);
            }
            catch (GenreNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _genreService.DeleteGenre(id);

                return RedirectToAction(nameof(Index));
            }
            catch (GenreNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
