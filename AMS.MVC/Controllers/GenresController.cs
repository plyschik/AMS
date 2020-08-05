using System;
using System.Data;
using System.Threading.Tasks;
using AMS.MVC.Data.Models;
using AMS.MVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GenresController : Controller
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IFlashMessage _flashMessage;

        public GenresController(IGenreRepository genreRepository, IFlashMessage flashMessage)
        {
            _genreRepository = genreRepository;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepository.GetAll();
            
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
                await _genreRepository.Create(genre);
                
                _flashMessage.Confirmation("Genre has been created.");

                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var genre = await _genreRepository.GetById(id);

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
                    await _genreRepository.Update(genre);
                }
                catch (DBConcurrencyException)
                {
                    if (!await _genreRepository.Exists(id))
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
    }
}
