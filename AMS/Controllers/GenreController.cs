using System;
using System.Threading.Tasks;
using AMS.Data.Requests;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GenreCreateRequest request)
        {
            try
            {
                var genre = await _genreService.Create(request);
                
                return Created("", genre);
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
