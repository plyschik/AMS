using System;
using System.Threading.Tasks;
using AMS.MVC.Data.Models;
using AMS.MVC.Exceptions.Genre;
using AMS.MVC.Repositories;
using AMS.MVC.ViewModels.GenreViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Services
{
    public interface IGenreService
    {
        public Task<GenreIndexViewModel> GetGenresList();

        public Task<GenreShowViewModel> GetGenreWithMovies(Guid id);

        public Task CreateGenre(GenreCreateViewModel viewModel);

        public Task<GenreEditViewModel> GetEditViewModel(Guid id);

        public Task UpdateGenre(Guid id, GenreEditViewModel viewModel);
        
        public Task<Genre> GetGenreToConfirmDelete(Guid id);

        public Task DeleteGenre(Guid id);
    }
    
    public class GenreService : IGenreService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        
        public GenreService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenreIndexViewModel> GetGenresList()
        {
            var genres = await _unitOfWork.Genres.GetAllOrderedByNameAscending().ToListAsync();

            return new GenreIndexViewModel
            {
                Genres = genres
            };
        }

        public async Task<GenreShowViewModel> GetGenreWithMovies(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                throw new GenreNotFoundException();
            }

            var movies = await _unitOfWork.Movies.GetMoviesWithGenresFromGenreOrderedByReleaseDate(genre.Id).ToListAsync();

            return new GenreShowViewModel
            {
                Genre = genre,
                Movies = movies
            };
        }

        public async Task CreateGenre(GenreCreateViewModel viewModel)
        {
            var genre = _mapper.Map<Genre>(viewModel);
            
            await _unitOfWork.Genres.Create(genre);
            await _unitOfWork.Save();
        }

        public async Task<GenreEditViewModel> GetEditViewModel(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                throw new GenreNotFoundException();
            }

            return _mapper.Map<GenreEditViewModel>(genre);
        }

        public async Task UpdateGenre(Guid id, GenreEditViewModel viewModel)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                throw new GenreNotFoundException();
            }

            _mapper.Map(viewModel, genre);
            
            _unitOfWork.Genres.Update(genre);
            await _unitOfWork.Save();
        }

        public async Task<Genre> GetGenreToConfirmDelete(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                throw new GenreNotFoundException();
            }

            return genre;
        }

        public async Task DeleteGenre(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                throw new GenreNotFoundException();
            }

            _unitOfWork.Genres.Delete(genre);
            await _unitOfWork.Save();
        }
    }
}
