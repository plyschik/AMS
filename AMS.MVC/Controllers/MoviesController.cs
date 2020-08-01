using System;
using System.Threading.Tasks;
using AMS.MVC.Data.Models;
using AMS.MVC.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMovieRepository _movieRepository;
        private readonly IFlashMessage _flashMessage;

        public MoviesController(
            UserManager<ApplicationUser> userManager,
            IMovieRepository movieRepository,
            IFlashMessage flashMessage
        )
        {
            _userManager = userManager;
            _movieRepository = movieRepository;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepository.GetAll();
            
            return View(movies);
        }
        
        [Route("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Show(Guid id)
        {
            var movie = await _movieRepository.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title, Description, ReleaseDate")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.User = await _userManager.GetUserAsync(HttpContext.User);

                await _movieRepository.Create(movie);

                _flashMessage.Confirmation("Movie has been created.");
                
                return RedirectToAction(nameof(Create));
            }

            return View(movie);
        }
    }
}
