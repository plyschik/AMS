using System.Threading.Tasks;
using AMS.Exceptions;
using AMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId:int}/stars")]
    public class MoviePersonStarController : Controller
    {
        private readonly IMoviePersonStarService _moviePersonStarService;

        public MoviePersonStarController(IMoviePersonStarService moviePersonStarService)
        {
            _moviePersonStarService = moviePersonStarService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStars(int movieId)
        {
            try
            {
                var response = await _moviePersonStarService.GetStarsForMovie(movieId);

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
