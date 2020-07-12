using System;
using System.Threading.Tasks;
using AMS.Data.Requests;
using AMS.Exceptions;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genreService.GetAll();

            return Ok(genres);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var genre = await _genreService.GetById(id);

                return Ok(genre);
            }
            catch (GenreNotFound)
            {
                return NotFound();
            }
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GenreCreateRequest request)
        {
            try
            {
                var genre = await _genreService.Create(request);

                return CreatedAtAction(nameof(Get), new { id = genre.Id }, genre);
            }
            catch (Exception exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
        }
    }
}
