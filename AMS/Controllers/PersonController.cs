using System;
using System.Threading.Tasks;
using AMS.Data.Requests;
using AMS.Exceptions;
using AMS.Services;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PersonCreateRequest request)
        {
            try
            {
                var person = await _personService.Create(request);

                return Ok(person);
            }
            catch (PersonAlreadyExists exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PersonUpdateRequest request)
        {
            try
            {
                var person = await _personService.Update(id, request);

                return Ok(person);
            }
            catch (PersonNotFound exception)
            {
                return NotFound(new
                {
                    exception.Message
                });
            }
            catch (PersonAlreadyExists exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
        }
        
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartialUpdate(int id, JsonPatchDocument<PersonUpdateRequest> document)
        {
            try
            {
                var personFromDatabase = await _personService.GetPerson(id);

                var personToPatch = _personService.MergePersonModelWithPatchDocument(
                    personFromDatabase,
                    document
                );

                if (!TryValidateModel(personToPatch))
                {
                    return ValidationProblem(ModelState);
                }

                var personResponse = await _personService.UpdatePartial(personToPatch, personFromDatabase);

                return Ok(personResponse);
            }
            catch (PersonNotFound exception)
            {
                return NotFound(new
                {
                    exception.Message
                });
            }
            catch (PersonAlreadyExists exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
        }
    }
}
