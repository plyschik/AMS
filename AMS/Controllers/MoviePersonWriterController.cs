using System.Threading.Tasks;
using AMS.Exceptions;
using AMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId:int}/writers")]
    public class MoviePersonWriterController : Controller
    {
        private readonly IMoviePersonWriterService _moviePersonWriterService;

        public MoviePersonWriterController(IMoviePersonWriterService moviePersonWriterService)
        {
            _moviePersonWriterService = moviePersonWriterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWriters(int movieId)
        {
            try
            {
                var response = await _moviePersonWriterService.GetWritersForMovie(movieId);

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
