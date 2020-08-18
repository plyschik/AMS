using System;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using AMS.MVC.ViewModels.MovieStarViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    public class StarsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFlashMessage _flashMessage;

        public StarsController(IUnitOfWork unitOfWork, IFlashMessage flashMessage)
        {
            _unitOfWork = unitOfWork;
            _flashMessage = flashMessage;
        }

        [HttpGet("[controller]/[action]/{movieId:guid}")]
        public async Task<IActionResult> Create(Guid movieId)
        {
            var movieStars = await _unitOfWork.MovieRepository.GetStars(movieId);
            var persons = await _unitOfWork.PersonRepository.GetAll();
            
            return View(new MovieStarCreateViewModel
            {
                Persons = persons.Except(movieStars).Select(person => new SelectListItem
                {
                    Text = person.FullName,
                    Value = person.Id.ToString()
                }).ToList()
            });
        }

        [HttpPost("[controller]/[action]/{movieId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid movieId, MovieStarCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var movie = await _unitOfWork.MovieRepository.GetById(movieId);
                
                movie.MovieStars.Add(new MovieStar
                {
                    PersonId = Guid.Parse(viewModel.SelectedPerson),
                    Character = viewModel.Character
                });

                await _unitOfWork.SaveChangesAsync();
                
                _flashMessage.Confirmation("Star has been attached to movie as character.");

                return RedirectToAction("Show", "Movies", new { id = movieId });
            }
            
            var movieStars = await _unitOfWork.MovieRepository.GetStars(movieId);
            var persons = await _unitOfWork.PersonRepository.GetAll();

            viewModel.Persons = persons.Except(movieStars).Select(person => new SelectListItem
            {
                Text = person.FullName,
                Value = person.Id.ToString()
            }).ToList();

            return View(viewModel);
        }
    }
}
