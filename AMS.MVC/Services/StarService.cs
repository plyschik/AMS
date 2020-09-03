using System;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Authorization;
using AMS.MVC.Data.Models;
using AMS.MVC.Exceptions;
using AMS.MVC.Exceptions.Movie;
using AMS.MVC.Repositories;
using AMS.MVC.ViewModels.MovieStarViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Services
{
    public interface IStarService
    {
        public Task<MovieStarCreateViewModel> LoadAvailableStarsToCreateViewModel(
            Guid movieId,
            MovieStarCreateViewModel viewModel = null
        );
        
        public Task CreateMovieStar(Guid movieId, MovieStarCreateViewModel viewModel);
        
        public Task<MovieStarEditViewModel> GetEditViewModel(Guid movieId, Guid personId);

        public Task UpdateMovieStar(Guid movieId, Guid personId, MovieStarEditViewModel viewModel);
        
        public Task<MovieStar> GetMovieStarToConfirmDelete(Guid movieId, Guid personId);

        public Task DeleteMovieStar(Guid movieId, Guid personId);
    }
    
    public class StarService : IStarService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public StarService(
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationService authorizationService,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieStarCreateViewModel> LoadAvailableStarsToCreateViewModel(
            Guid movieId,
            MovieStarCreateViewModel viewModel = null
        )
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFoundException();
            }
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Create
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AccessDeniedException();
            }
            
            if (viewModel == null)
            {
                viewModel = new MovieStarCreateViewModel();
            }
            
            var persons = await _unitOfWork.Persons.GetAllOrderedBy("last_name", "asc").ToListAsync();
            var stars = await _unitOfWork.MovieStar.GetStarsAsPersons(movieId).ToListAsync();

            viewModel.Persons = persons.Except(stars).Select(person => new SelectListItem
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
                throw new MovieNotFoundException();
            }
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Create
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AccessDeniedException();
            }

            var movieStar = _mapper.Map<MovieStar>(viewModel);
            
            movie.MovieStars.Add(movieStar);

            await _unitOfWork.Save();
        }

        public async Task<MovieStarEditViewModel> GetEditViewModel(Guid movieId, Guid personId)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFoundException();
            }
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Edit
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AccessDeniedException();
            }

            var movieStar = await _unitOfWork.MovieStar.GetBy(
                ms => ms.MovieId == movieId && ms.PersonId == personId
            );

            return _mapper.Map<MovieStarEditViewModel>(movieStar);
        }

        public async Task UpdateMovieStar(Guid movieId, Guid personId, MovieStarEditViewModel viewModel)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFoundException();
            }
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Edit
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AccessDeniedException();
            }
            
            var movieStar = await _unitOfWork.MovieStar.GetBy(
                ms => ms.MovieId == movieId && ms.PersonId == personId    
            );

            _mapper.Map(viewModel, movieStar);
            
            await _unitOfWork.Save();
        }

        public async Task<MovieStar> GetMovieStarToConfirmDelete(Guid movieId, Guid personId)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFoundException();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Delete
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AccessDeniedException();
            }

            var movieStar = await _unitOfWork.MovieStar.GetMovieStarWithPersonByMovieIdAndPersonId(movieId, personId);

            return movieStar;
        }

        public async Task DeleteMovieStar(Guid movieId, Guid personId)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFoundException();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieStarOperations.Delete
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AccessDeniedException();
            }
            
            var movieStar = await _unitOfWork.MovieStar.GetBy(
                ms => ms.MovieId == movieId && ms.PersonId == personId
            );

            if (movieStar == null)
            {
                throw new MovieNotFoundException();
            }
            
            _unitOfWork.MovieStar.Delete(movieStar);
            await _unitOfWork.Save();
        }
    }
}
