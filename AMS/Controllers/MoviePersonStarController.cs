using System.Threading.Tasks;
using AMS.Exceptions;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPut("{personId:int}")]
        public async Task<IActionResult> AttachWriter(int movieId, int personId)
        {
            try
            {
                await _moviePersonStarService.AttachStarToMovie(movieId, personId);

                return Ok();
            }
            catch (MovieNotFound exception)
            {
                return NotFound(new
                {
                    exception.Message
                });
            }
            catch (PersonNotFound exception)
            {
                return NotFound(new
                {
                    exception.Message
                });
            }
            catch (StarAlreadyAttachedToMovie exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
        }

        [Authorize]
        [HttpDelete("{personId:int}")]
        public async Task<IActionResult> DetachWriter(int movieId, int personId)
        {
            try
            {
                await _moviePersonStarService.DetachStarFromMovie(movieId, personId);

                return NoContent();
            }
            catch (MoviePersonStarNotFound exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
        }
    }
}
