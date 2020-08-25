using System;
using System.Threading.Tasks;
using AMS.MVC.Exceptions.Person;
using AMS.MVC.Services;
using AMS.MVC.ViewModels.PersonViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IFlashMessage _flashMessage;

        public PersonsController(IPersonService personService, IFlashMessage flashMessage)
        {
            _personService = personService;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _personService.GetPersonsList();
            
            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Show(Guid id)
        {
            try
            {
                var viewModel = await _personService.GetPerson(id);
                
                return View(viewModel);
            }
            catch (PersonNotFound)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Manager, Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _personService.CreatePerson(viewModel);

                _flashMessage.Confirmation("Person has been created.");
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Manager, Administrator")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var viewModel = await _personService.GetPersonEdit(id);
                
                return View(viewModel);
            }
            catch (PersonNotFound)
            {
                return NotFound();
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "Manager, Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PersonEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _personService.PersonEdit(id, viewModel);
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (PersonNotFound)
                {
                    return NotFound();
                }
            }

            return View(viewModel);
        }
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            try
            {
                var person = await _personService.GetPersonToConfirmDelete(id);

                return View(person);
            }
            catch (PersonNotFound)
            {
                return NotFound();
            }
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _personService.DeletePerson(id);
                
                return RedirectToAction(nameof(Index));
            }
            catch (PersonNotFound)
            {
                return NotFound();
            }
        }
    }
}
