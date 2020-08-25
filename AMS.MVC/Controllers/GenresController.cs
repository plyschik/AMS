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
        public async Task<IActionResult> Index()
        {
            var viewModel = await _genreService.GetGenresList();
            
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
            catch (GenreNotFound)
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
        public async Task<IActionResult> Create(GenreCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _genreService.CreateGenre(viewModel);
                
                _flashMessage.Confirmation("Genre has been created.");

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var viewModel = await _genreService.GetGenreEdit(id);
                
                return View(viewModel);
            }
            catch (GenreNotFound)
            {
                return NotFound();
            }
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, GenreEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _genreService.GenreEdit(id, viewModel);
                                
                _flashMessage.Confirmation("Genre has been updated.");
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            try
            {
                var genre = await _genreService.GetGenreToConfirmDelete(id);

                return View(genre);
            }
            catch (GenreNotFound)
            {
                return NotFound();
            }
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _genreService.DeleteGenre(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
