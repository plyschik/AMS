using System;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using AMS.MVC.Repositories;
using AMS.MVC.ViewModels.PersonViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DatabaseContext _databaseContext;
        private readonly IFlashMessage _flashMessage;

        public PersonsController(
            IUnitOfWork unitOfWork,
            DatabaseContext databaseContext,
            IFlashMessage flashMessage
        )
        {
            _unitOfWork = unitOfWork;
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }

        public IActionResult Index()
        {
            var persons = _unitOfWork.Persons.GetAll().ToList();
            
            return View(persons);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Show(Guid id)
        {
            var person = await _databaseContext.Persons.FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            var director = await _databaseContext.Movies
                .Include(m => m.MovieDirectors)
                .Where(m => m.MovieDirectors.Any(md => md.PersonId == id))
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();
            
            var writer = await _databaseContext.Movies
                .Include(m => m.MovieWriters)
                .Where(m => m.MovieWriters.Any(mw => mw.PersonId == id))
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();
            
            var star = await _databaseContext.Movies
                .Include(m => m.MovieStars)
                .Where(m => m.MovieStars.Any(ms => ms.PersonId == id))
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();

            return View(new PersonShowViewModel
            {
                Person = person,
                MovieDirector = director,
                MovieWriter = writer,
                MovieStar = star
            });
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
            
                await _unitOfWork.Persons.Create(person);
                await _unitOfWork.Save();

                _flashMessage.Confirmation("Person has been created.");
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Manager, Administrator")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

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
                var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

                if (person == null)
                {
                    return NotFound();
                }

                person.FirstName = viewModel.FirstName;
                person.LastName = viewModel.LastName;
                person.DateOfBirth = viewModel.DateOfBirth;
                
                _unitOfWork.Persons.Update(person);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
        
        [HttpGet("[controller]/[action]/{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

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
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            _unitOfWork.Persons.Delete(person);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
