using System.Threading.Tasks;
using AMS.Exceptions;
using AMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId:int}/genres")]
    public class MovieGenreController : Controller
    {
        private readonly IMovieGenreService _movieGenreService;

        public MovieGenreController(IMovieGenreService movieGenreService)
        {
            _movieGenreService = movieGenreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres(int movieId)
        {
            try
            {
                var genreResponses = await _movieGenreService.GetGenresForMovie(movieId);

                return Ok(genreResponses);
            }
            catch (MovieNotFound)
            {
                return NotFound();
            }
        }

        [HttpPut("{genreId:int}")]
        public async Task<IActionResult> AttachGenre(int movieId, int genreId)
        {
            try
            {
                await _movieGenreService.AttachGenreToMovie(movieId, genreId);

                return Ok();
            }
            catch (MovieNotFound exception)
            {
                return NotFound(new
                {
                    exception.Message
                });
            }
            catch (GenreNotFound exception)
            {
                return NotFound(new
                {
                    exception.Message
                });
            }
            catch (GenreAlreadyAttachedToMovie exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
        }

        [HttpDelete("{genreId:int}")]
        public async Task<IActionResult> DetachGenre(int movieId, int genreId)
        {
            try
            {
                await _movieGenreService.DetachGenreFromMovie(movieId, genreId);
                
                return NoContent();
            }
            catch (MovieGenreNotFound exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
        }
    }
}
