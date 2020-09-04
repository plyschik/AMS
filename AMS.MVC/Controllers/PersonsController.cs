using System;
using System.Threading.Tasks;
using AMS.MVC.Exceptions;
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

        public async Task<IActionResult> Index(int page = 1, string sort = "last_name", string order = "asc")
        {
            try
            {
                ViewData["page"] = page;
                ViewData["sort"] = sort;
                ViewData["order"] = order;
                
                var viewModel = await _personService.GetPersonsList(page, sort, order);

                return View(viewModel);
            }
            catch (PageNumberOutOfRangeException)
            {
                return NotFound();
            }
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Show(Guid id)
        {
            try
            {
                var viewModel = await _personService.GetPerson(id);
                
                return View(viewModel);
            }
            catch (PersonNotFoundException)
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
        public async Task<IActionResult> Create(PersonCreateViewModel personCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                await _personService.CreatePerson(personCreateViewModel);

                _flashMessage.Confirmation("Person has been created.");
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(personCreateViewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Manager, Administrator")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var viewModel = await _personService.GetEditViewModel(id);
                
                return View(viewModel);
            }
            catch (PersonNotFoundException)
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
                    await _personService.UpdatePerson(id, viewModel);
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (PersonNotFoundException)
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
            catch (PersonNotFoundException)
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
            catch (PersonNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
