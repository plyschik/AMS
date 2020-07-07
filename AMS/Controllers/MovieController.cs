using System.Threading.Tasks;
using AMS.Data.Requests;
using AMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieCreateRequest request)
        {
            await _movieService.Create(request);

            return Created(string.Empty, null);
        }
    }
}
