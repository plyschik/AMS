using System;
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

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GenreUpdateRequest request)
        {
            try
            {
                var genre = await _genreService.Update(id, request);

                return Ok(genre);
            }
            catch (GenreNotFound)
            {
                return NotFound();
            }
        }
        
        [Authorize]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartialUpdate(int id, JsonPatchDocument<GenreUpdateRequest> document)
        {
            if (document == null)
            {
                return BadRequest();
            }
            
            try
            {
                var genre = await _genreService.GetGenre(id);
                
                var mergedGenreUpdateRequest = _genreService.MergeGenreModelWithPatchDocument(genre, document);

                if (!TryValidateModel(mergedGenreUpdateRequest))
                {
                    return ValidationProblem(ModelState);
                }

                var genreResponse = await _genreService.UpdatePartial(mergedGenreUpdateRequest);
            
                return Ok(genreResponse);
            }
            catch (GenreNotFound)
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
                await _genreService.Delete(id);
                
                return NoContent();
            }
            catch (GenreNotFound)
            {
                return NotFound();
            }
        }
    }
}
