using System;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using AMS.MVC.ViewModels.PersonViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFlashMessage _flashMessage;

        public PersonsController(IUnitOfWork unitOfWork, IFlashMessage flashMessage)
        {
            _unitOfWork = unitOfWork;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var persons = await _unitOfWork.PersonRepository.GetAll();
            
            return View(persons);
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
                var person = new Person
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    DateOfBirth = viewModel.DateOfBirth
                };
            
                _unitOfWork.PersonRepository.Create(person);
                await _unitOfWork.SaveChangesAsync();

                _flashMessage.Confirmation("Person has been created.");
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Manager, Administrator")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var person = await _unitOfWork.PersonRepository.GetById(id);

            if (person == null)
            {
                return NotFound();
            }

            return View(new PersonEditViewModel
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth
            });
        }
        
        [HttpPost]
        [Authorize(Roles = "Manager, Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PersonEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var person = await _unitOfWork.PersonRepository.GetById(id);

                if (person == null)
                {
                    return NotFound();
                }

                person.FirstName = viewModel.FirstName;
                person.LastName = viewModel.LastName;
                person.DateOfBirth = viewModel.DateOfBirth;
                
                _unitOfWork.PersonRepository.Update(person);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            var person = await _unitOfWork.PersonRepository.GetById(id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        [HttpPost("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var person = await _unitOfWork.PersonRepository.GetById(id);

            if (person == null)
            {
                return NotFound();
            }

            _unitOfWork.PersonRepository.Delete(person);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
