using System;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Authorization;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using AMS.MVC.Repositories;
using AMS.MVC.ViewModels.MovieStarViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace AMS.MVC.Controllers
{
    public class StarsController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DatabaseContext _databaseContext;
        private readonly IFlashMessage _flashMessage;

        public StarsController(
            IAuthorizationService authorizationService,
            IUnitOfWork unitOfWork,
            DatabaseContext databaseContext,
            IFlashMessage flashMessage
        )
        {
            _authorizationService = authorizationService;
            _unitOfWork = unitOfWork;
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }

        [HttpGet("[controller]/[action]/{movieId:guid}")]
        public async Task<IActionResult> Create(Guid movieId)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User,
                movie,
                MovieStarOperations.Create
            );

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            var movieStars = await _unitOfWork.Movies.GetStars(movieId);
            var persons = _unitOfWork.Persons.GetAll().ToList();
            
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
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User,
                movie,
                MovieStarOperations.Create
            );

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            if (ModelState.IsValid)
            {
                movie.MovieStars.Add(new MovieStar
                {
                    PersonId = Guid.Parse(viewModel.SelectedPerson),
                    Character = viewModel.Character
                });

                await _unitOfWork.Save();
                
                _flashMessage.Confirmation("Star has been attached to movie as character.");

                return RedirectToAction("Show", "Movies", new { id = movieId });
            }
            
            var movieStars = await _unitOfWork.Movies.GetStars(movieId);
            var persons = _unitOfWork.Persons.GetAll();

            viewModel.Persons = persons.Except(movieStars).Select(person => new SelectListItem
            {
                Text = person.FullName,
                Value = person.Id.ToString()
            }).ToList();

            return View(viewModel);
        }
        
        [HttpGet("[controller]/[action]/{movieId:guid}/{personId:guid}")]
        public async Task<IActionResult> Edit(Guid movieId, Guid personId)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User,
                movie,
                MovieStarOperations.Edit
            );

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            var movieStar = await _databaseContext.MovieStars.FirstOrDefaultAsync(
                ms => ms.MovieId == movieId && ms.PersonId == personId
            );
            
            return View(new MovieStatEditViewModel
            {
                Character = movieStar.Character 
            });
        }
        
        [HttpPost("[controller]/[action]/{movieId:guid}/{personId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid movieId, Guid personId, MovieStatEditViewModel viewModel)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User,
                movie,
                MovieStarOperations.Edit
            );

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            if (ModelState.IsValid)
            {
                var movieStar = await _databaseContext.MovieStars.FirstOrDefaultAsync(
                    ms => ms.MovieId == movieId && ms.PersonId == personId
                );

                movieStar.Character = viewModel.Character;

                await _databaseContext.SaveChangesAsync();

                _flashMessage.Confirmation("Movie character has been updated.");
                
                return RedirectToAction("Show", "Movies", new { id = movieId });
            }
            
            return View(viewModel);
        }
        
        [HttpGet("[controller]/[action]/{movieId:guid}/{personId:guid}")]
        public async Task<IActionResult> ConfirmDelete(Guid movieId, Guid personId)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User,
                movie,
                MovieStarOperations.Delete
            );

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            var movieStar = await _databaseContext.MovieStars
                .Include(ms => ms.Person)
                .FirstOrDefaultAsync(
                    ms => ms.MovieId == movieId && ms.PersonId == personId
                );
            
            return View(movieStar);
        }
        
        [HttpPost("[controller]/[action]/{movieId:guid}/{personId:guid}")]
        public async Task<IActionResult> Delete(Guid movieId, Guid personId)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User,
                movie,
                MovieStarOperations.Delete
            );

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            var movieStar = await _databaseContext.MovieStars.FirstOrDefaultAsync(
                ms => ms.MovieId == movieId && ms.PersonId == personId
            );

            if (movieStar == null)
            {
                return NotFound();
            }
            
            _databaseContext.MovieStars.Remove(movieStar);
            await _databaseContext.SaveChangesAsync();

            return RedirectToAction("Show", "Movies", new { id = movieId });
        }
    }
}
