using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using AMS.MVC.ViewModels.PersonViewModels;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
    }
}
