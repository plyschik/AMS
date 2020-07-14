using System.Threading.Tasks;
using AMS.Exceptions;
using AMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var persons = await _personService.GetAll();

            return Ok(persons);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var person = await _personService.GetById(id);

                return Ok(person);
            }
            catch (PersonNotFound exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
            
        }
    }
}
