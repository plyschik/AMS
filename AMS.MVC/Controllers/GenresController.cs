using System;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using AMS.MVC.Repositories;
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
        public IActionResult Index()
        {
            var genres = _unitOfWork.Genres.GetAll();
            
            return View(genres);
        }

        [HttpGet("[controller]/[action]/{genreId:guid}")]
        [Authorize]
        public async Task<IActionResult> Show(Guid genreId)
        {
            var genre = await _databaseContext.Genres
                .Where(g => g.Id == genreId)
                .FirstOrDefaultAsync();

            var movies = await _databaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId))
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();
            
            if (genre == null)
            {
                return NotFound();
            }
            
            return View(new GenreShowViewModel
            {
                Genre = genre,
                Movies = movies
            });
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
                
                await _unitOfWork.Genres.Create(genre);
                await _unitOfWork.Save();
                
                _flashMessage.Confirmation("Genre has been created.");

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

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
                var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

                if (genre == null)
                {
                    return NotFound();
                }

                genre.Name = viewModel.Name;
            
                _unitOfWork.Genres.Update(genre);
                await _unitOfWork.Save();
                                
                _flashMessage.Confirmation("Genre has been updated.");
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

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
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            _unitOfWork.Genres.Delete(genre);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
