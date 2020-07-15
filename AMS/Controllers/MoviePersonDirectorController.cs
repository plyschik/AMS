using System.Threading.Tasks;
using AMS.Exceptions;
using AMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId:int}/directors")]
    public class MoviePersonDirectorController : Controller
    {
        private readonly IMoviePersonDirectorService _moviePersonDirectorService;

        public MoviePersonDirectorController(IMoviePersonDirectorService moviePersonDirectorService)
        {
            _moviePersonDirectorService = moviePersonDirectorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDirectors(int movieId)
        {
            try
            {
                var response = await _moviePersonDirectorService.GetDirectorsForMovie(movieId);

                return Ok(response);
            }
            catch (MovieNotFound exception)
            {
                return NotFound(new
                {
                    exception.Message
                });
            }
        }
    }
}
