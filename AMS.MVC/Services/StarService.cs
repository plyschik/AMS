using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using AMS.MVC.Authorization;
using AMS.MVC.Data.Models;
using AMS.MVC.Exceptions.Movie;
using AMS.MVC.Repositories;
using AMS.MVC.ViewModels.MovieStarViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Services
{
    public interface IStarService
    {
        public Task<MovieStarCreateViewModel> FillCreateViewModelWithAvailableStars(
            Guid movieId,
            MovieStarCreateViewModel viewModel = null
        );
        
        public Task CreateMovieStar(Guid movieId, MovieStarCreateViewModel viewModel);
        
        public Task<MovieStarEditViewModel> GetMovieStarEdit(Guid movieId, Guid personId);

        public Task MovieStarEdit(Guid movieId, Guid personId, MovieStarEditViewModel viewModel);
        
        public Task<MovieStar> GetMovieStarToConfirmDelete(Guid movieId, Guid personId);

        public Task DeleteMovieStar(Guid movieId, Guid personId);
    }
    
    public class StarService : IStarService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUnitOfWork _unitOfWork;

        public StarService(
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationService authorizationService,
            IUnitOfWork unitOfWork
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieStarCreateViewModel> FillCreateViewModelWithAvailableStars(
            Guid movieId,
            MovieStarCreateViewModel viewModel = null
        )
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFound();
            }
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Create
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AuthenticationException();
            }
            
            if (viewModel == null)
            {
                viewModel = new MovieStarCreateViewModel();
            }
            
            var persons = await _unitOfWork.Persons.GetAllOrderedByLastNameAscending().ToListAsync();
            var movieStars = await _unitOfWork.Movies.GetStars(movieId);

            viewModel.Persons = persons.Except(movieStars).Select(person => new SelectListItem
            {
                Text = person.FullName,
                Value = person.Id.ToString()
            }).ToList();

            return viewModel;
        }

        public async Task CreateMovieStar(Guid movieId, MovieStarCreateViewModel viewModel)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFound();
            }
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Create
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AuthenticationException();
            }
            
            movie.MovieStars.Add(new MovieStar
            {
                PersonId = Guid.Parse(viewModel.SelectedPerson),
                Character = viewModel.Character
            });

            await _unitOfWork.Save();
        }

        public async Task<MovieStarEditViewModel> GetMovieStarEdit(Guid movieId, Guid personId)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFound();
            }
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Edit
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AuthenticationException();
            }

            var movieStar = await _unitOfWork.MovieStar.GetBy(
                ms => ms.MovieId == movieId && ms.PersonId == personId
            );

            return new MovieStarEditViewModel
            {
                Character = movieStar.Character 
            };
        }

        public async Task MovieStarEdit(Guid movieId, Guid personId, MovieStarEditViewModel viewModel)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFound();
            }
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Edit
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AuthenticationException();
            }
            
            var movieStar = await _unitOfWork.MovieStar.GetBy(
                ms => ms.MovieId == movieId && ms.PersonId == personId    
            );
            
            movieStar.Character = viewModel.Character;
            
            await _unitOfWork.Save();
        }

        public async Task<MovieStar> GetMovieStarToConfirmDelete(Guid movieId, Guid personId)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Delete
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AuthenticationException();
            }

            var movieStar = await _unitOfWork.MovieStar.GetByWithPerson(
                ms => ms.MovieId == movieId && ms.PersonId == personId
            );
            
            return movieStar;
        }

        public async Task DeleteMovieStar(Guid movieId, Guid personId)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Delete
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AuthenticationException();
            }
            
            var movieStar = await _unitOfWork.MovieStar.GetBy(
                ms => ms.MovieId == movieId && ms.PersonId == personId
            );

            if (movieStar == null)
            {
                throw new MovieNotFound();
            }
            
            _unitOfWork.MovieStar.Delete(movieStar);
            await _unitOfWork.Save();
        }
    }
}
