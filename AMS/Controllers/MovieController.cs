using System.Threading.Tasks;
using AMS.Data.Requests;
using AMS.Exceptions;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieService.GetAll();
            
            return Ok(movies);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var movie = await _movieService.GetById(id);

                return Ok(movie);
            }
            catch (MovieNotFound)
            {
                return NotFound();
            }
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieCreateRequest request)
        {
            var movie = await _movieService.Create(request);

            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieUpdateRequest request)
        {
            try
            {
                var movie = await _movieService.Update(id, request);
                
                return Ok(movie);
            }
            catch (MovieNotFound)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartialUpdate(int id, JsonPatchDocument<MovieUpdateRequest> document)
        {
            if (document == null)
            {
                return BadRequest();
            }
            
            try
            {
                var movie = await _movieService.GetMovie(id);
                
                var mergedMovieUpdateRequest = _movieService.MergeMovieModelWithPatchDocument(movie, document);

                if (!TryValidateModel(mergedMovieUpdateRequest))
                {
                    return ValidationProblem(ModelState);
                }

                var movieResponse = await _movieService.UpdatePartial(mergedMovieUpdateRequest);
            
                return Ok(movieResponse);
            }
            catch (MovieNotFound)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _movieService.Delete(id);
                
                return NoContent();
            }
            catch (MovieNotFound)
            {
                return NotFound();
            }
        }
    }
}
