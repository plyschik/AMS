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
    }
}
