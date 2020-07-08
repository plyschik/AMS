using System.Threading.Tasks;
using AMS.Data.Requests;
using AMS.Exceptions;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieService.GetAll();
            
            return Ok(movies);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var movie = await _movieService.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }
            
            return Ok(movie);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieCreateRequest request)
        {
            var movie = await _movieService.Create(request);

            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }

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
