using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using AMS.MVC.Authorization;
using AMS.MVC.Data.Models;
using AMS.MVC.Exceptions;
using AMS.MVC.Exceptions.Movie;
using AMS.MVC.Helpers;
using AMS.MVC.Repositories;
using AMS.MVC.ViewModels.MovieViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Services
{
    public interface IMovieService
    {
        public Task<MovieIndexViewModel> GetMoviesList(string search, string sort, string order, int page);

        public Task<MovieShowViewModel> GetMovie(Guid id);

        public Task<MovieCreateViewModel> LoadGenresAndPersonsToCreateViewModel(MovieCreateViewModel viewModel = null);

        public Task CreateMovie(MovieCreateViewModel viewModel);

        public Task<MovieEditViewModel> LoadGenresAndPersonsToEditViewModel(MovieEditViewModel viewModel);

        public Task<MovieEditViewModel> GetEditViewModel(Guid id);

        public Task UpdateMovie(Guid id, MovieEditViewModel viewModel);
        
        public Task<Movie> GetMovieToConfirmDelete(Guid id);

        public Task DeleteMovie(Guid id);
    }
    
    public class MovieService : IMovieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            IAuthorizationService authorizationService,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieIndexViewModel> GetMoviesList(string search, string sort, string order, int page)
        {
            return new MovieIndexViewModel
            {
                Paginator = await new Paginator<Movie>().Create(
                    _unitOfWork.Movies.GetMoviesWithGenresOrderedBy(search, sort, order),
                    page,
                    5
                )
            };
        }

        public async Task<MovieShowViewModel> GetMovie(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetMovieWithGenresDirectorsWritersAndStars(id);

            if (movie == null)
            {
                throw new MovieNotFoundException();
            }

            return new MovieShowViewModel
            {
                Movie = movie,
                Genres = movie.MovieGenres.Select(mg => mg.Genre).ToList(),
                Directors = movie.MovieDirectors.Select(md => md.Person).ToList(),
                Writers = movie.MovieWriters.Select(mw => mw.Person).ToList(),
                Stars = movie.MovieStars.ToList()
            };
        }

        public async Task<MovieCreateViewModel> LoadGenresAndPersonsToCreateViewModel(MovieCreateViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new MovieCreateViewModel();
            }
            
            var genres = await _unitOfWork.Genres.GetAllOrderedBy("name", "desc").ToListAsync();
            var persons = await _unitOfWork.Persons.GetAllOrderedBy(null, "last_name", "asc").ToListAsync();

            viewModel.Genres = genres.Select(genre => new SelectListItem
            {
                Text = genre.Name,
                Value = genre.Id.ToString()
            }).ToList();

            viewModel.Persons = persons.Select(person => new SelectListItem
            {
                Text = person.FullName,
                Value = person.Id.ToString()
            }).ToList();

            return viewModel;
        }

        public async Task CreateMovie(MovieCreateViewModel viewModel)
        {
            var movie = _mapper.Map<Movie>(viewModel, options =>
            {
                options.AfterMap(async (source, destination) =>
                {
                    destination.User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                });
            });
            
            await _unitOfWork.Movies.Create(movie);
                
            foreach (var genreId in viewModel.SelectedGenres)
            {
                movie.MovieGenres.Add(new MovieGenre
                {
                    GenreId = Guid.Parse(genreId)
                });
            }

            foreach (var personId in viewModel.SelectedDirectors)
            {
                movie.MovieDirectors.Add(new MovieDirector
                {
                    PersonId = Guid.Parse(personId)
                });
            }
                
            foreach (var personId in viewModel.SelectedWriters)
            {
                movie.MovieWriters.Add(new MovieWriter
                {
                    PersonId = Guid.Parse(personId)
                });
            }
                
            await _unitOfWork.Save();
        }

        public async Task<MovieEditViewModel> LoadGenresAndPersonsToEditViewModel(MovieEditViewModel viewModel)
        {
            var genres = await _unitOfWork.Genres.GetAllOrderedBy("name", "desc").ToListAsync();
            var persons = await _unitOfWork.Persons.GetAllOrderedBy(null, "last_name", "asc").ToListAsync();

            viewModel.Genres = genres.Select(genre => new SelectListItem
            {
                Text = genre.Name,
                Value = genre.Id.ToString()
            }).ToList();

            viewModel.Persons = persons.Select(person => new SelectListItem
            {
                Text = person.FullName,
                Value = person.Id.ToString()
            }).ToList();

            return viewModel;
        }

        public async Task<MovieEditViewModel> GetEditViewModel(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetMovieWithGenresDirectorsWritersAndStars(id);

            if (movie == null)
            {
                throw new MovieNotFoundException();
            }
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieOperations.Edit
            );
            
            if (!isAuthorized.Succeeded)
            {
                throw new AccessDeniedException();
            }

            var movieEditViewModel = _mapper.Map<MovieEditViewModel>(movie, options =>
            {
                options.AfterMap((source, destination) =>
                {
                    destination.SelectedGenres = movie.MovieGenres
                        .Select(mg => mg.GenreId.ToString()).ToArray();
                    
                    destination.SelectedDirectors = movie.MovieDirectors
                        .Select(md => md.PersonId.ToString()).ToArray();
                    
                    destination.SelectedWriters = movie.MovieWriters
                        .Select(mw => mw.PersonId.ToString()).ToArray();
                });
            });
            
            movieEditViewModel = await LoadGenresAndPersonsToEditViewModel(movieEditViewModel);
            
            return movieEditViewModel;
        }

        public async Task UpdateMovie(Guid id, MovieEditViewModel viewModel)
        {
            var movie = await _unitOfWork.Movies.GetMovieWithGenresDirectorsWritersAndStars(id);

            if (movie == null)
            {
                throw new MovieNotFoundException();
            }
            
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                movie,
                MovieOperations.Edit
            );

            if (!isAuthorized.Succeeded)
            {
                throw new AuthenticationException();
            }

            _mapper.Map(viewModel, movie);

            var attachedGenreIds = movie.MovieGenres.Select(mg => mg.GenreId.ToString()).ToArray();
            var selectedGenreIds = viewModel.SelectedGenres ??= new string[] {};
            var genresIdsToAttach = selectedGenreIds.Except(attachedGenreIds).ToArray();
            var genresIdsToDetach = attachedGenreIds.Except(selectedGenreIds).ToArray();

            movie.MovieGenres = movie.MovieGenres.Where(
                mg => !genresIdsToDetach.Contains(mg.GenreId.ToString())
            ).ToList();
            
            foreach (var genreId in genresIdsToAttach)
            {
                movie.MovieGenres.Add(new MovieGenre
                {
                    GenreId = Guid.Parse(genreId)
                });
            }
            
            var attachedDirectorsIds = movie.MovieDirectors.Select(md => md.PersonId.ToString()).ToArray();
            var selectedDirectorsIds = viewModel.SelectedDirectors ??= new string[] {};
            var directorsIdsToAttach = selectedDirectorsIds.Except(attachedDirectorsIds).ToArray();
            var directorsIdsToDetach = attachedDirectorsIds.Except(selectedDirectorsIds).ToArray();

            movie.MovieDirectors = movie.MovieDirectors.Where(
                md => !directorsIdsToDetach.Contains(md.PersonId.ToString())
            ).ToList();
            
            foreach (var personId in directorsIdsToAttach)
            {
                movie.MovieDirectors.Add(new MovieDirector
                {
                    PersonId = Guid.Parse(personId)
                });
            }
            
            var attachedWritersIds = movie.MovieWriters.Select(md => md.PersonId.ToString()).ToArray();
            var selectedWritersIds = viewModel.SelectedWriters ??= new string[] {};
            var writersIdsToAttach = selectedWritersIds.Except(attachedWritersIds).ToArray();
            var writersIdsToDetach = attachedWritersIds.Except(selectedWritersIds).ToArray();

            movie.MovieWriters = movie.MovieWriters.Where(
                md => !writersIdsToDetach.Contains(md.PersonId.ToString())
            ).ToList();
            
            foreach (var personId in writersIdsToAttach)
            {
                movie.MovieWriters.Add(new MovieWriter
                {
                    PersonId = Guid.Parse(personId)
                });
            }

            await _unitOfWork.Save();
        }

        public async Task<Movie> GetMovieToConfirmDelete(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == id);

            if (movie == null)
            {
                throw new MovieNotFoundException();
            }

            return movie;
        }

        public async Task DeleteMovie(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetBy(m => m.Id == id);

            if (movie == null)
            {
                throw new MovieNotFoundException();
            }

            _unitOfWork.Movies.Delete(movie);
            await _unitOfWork.Save();
        }
    }
}
