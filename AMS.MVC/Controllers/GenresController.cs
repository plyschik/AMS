using System;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using AMS.MVC.ViewModels.GenreViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GenresController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DatabaseContext _databaseContext;
        private readonly IFlashMessage _flashMessage;

        public GenresController(
            IUnitOfWork unitOfWork,
            DatabaseContext databaseContext,
            IFlashMessage flashMessage
        )
        {
            _unitOfWork = unitOfWork;
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var genres = await _unitOfWork.GenreRepository.GetAll();
            
            return View(genres);
        }

        [HttpGet("[controller]/[action]/{genreId:guid}")]
        [Authorize]
        public async Task<IActionResult> Show(Guid genreId)
        {
            var genre = await _databaseContext.Genres
                .Include(g => g.MovieGenres)
                .ThenInclude(mg => mg.Movie)
                .Where(g => g.Id == genreId)
                .FirstOrDefaultAsync();

            if (genre == null)
            {
                return NotFound();
            }
            
            return View(genre);
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
                var genre = new Genre
                {
                    Name = viewModel.Name
                };
                
                _unitOfWork.GenreRepository.Create(genre);
                await _unitOfWork.SaveChangesAsync();
                
                _flashMessage.Confirmation("Genre has been created.");

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var genre = await _unitOfWork.GenreRepository.GetById(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(new GenreEditViewModel
            {
                Name = genre.Name
            });
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, GenreEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var genre = await _unitOfWork.GenreRepository.GetById(id);

                if (genre == null)
                {
                    return NotFound();
                }

                genre.Name = viewModel.Name;
            
                _unitOfWork.GenreRepository.Update(genre);
                await _unitOfWork.SaveChangesAsync();
                                
                _flashMessage.Confirmation("Genre has been updated.");
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            var genre = await _unitOfWork.GenreRepository.GetById(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var genre = await _unitOfWork.GenreRepository.GetById(id);

            if (genre == null)
            {
                return NotFound();
            }

            _unitOfWork.GenreRepository.Delete(genre);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
