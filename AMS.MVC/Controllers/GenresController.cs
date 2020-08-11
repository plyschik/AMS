using System;
using System.Data;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GenresController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFlashMessage _flashMessage;

        public GenresController(IUnitOfWork unitOfWork, IFlashMessage flashMessage)
        {
            _unitOfWork = unitOfWork;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _unitOfWork.GenreRepository.GetAll();
            
            return View(genres);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.GenreRepository.Create(genre);
                await _unitOfWork.SaveChangesAsync();
                
                _flashMessage.Confirmation("Genre has been created.");

                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var genre = await _unitOfWork.GenreRepository.GetById(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, Name")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.GenreRepository.Update(genre);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DBConcurrencyException)
                {
                    if (!await _unitOfWork.GenreRepository.Exists(id))
                    {
                        return NotFound();
                    }
                    
                    return BadRequest();
                }
                
                _flashMessage.Confirmation("Genre has been updated.");
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(genre);
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
