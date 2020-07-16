using System;
using System.Threading.Tasks;
using AMS.Exceptions;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPut("{personId:int}")]
        public async Task<IActionResult> AttachWriter(int movieId, int personId)
        {
            try
            {
                await _moviePersonWriterService.AttachWriterToMovie(movieId, personId);

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
            catch (WriterAlreadyAttachedToMovie exception)
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
                await _moviePersonWriterService.DetachWriterFromMovie(movieId, personId);

                return NoContent();
            }
            catch (MoviePersonWriterNotFound exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
        }
    }
}
