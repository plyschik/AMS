using System.Threading.Tasks;
using AMS.MVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.MVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GenresController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepository.GetAll();
            
            return View(genres);
        }
    }
}
