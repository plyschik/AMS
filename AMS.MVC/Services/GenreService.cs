using System;
using System.Threading.Tasks;
using AMS.MVC.Data.Models;
using AMS.MVC.Exceptions.Genre;
using AMS.MVC.Repositories;
using AMS.MVC.ViewModels.GenreViewModels;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Services
{
    public interface IGenreService
    {
        public Task<GenreIndexViewModel> GetGenresList();

        public Task<GenreShowViewModel> GetGenreWithMovies(Guid id);

        public Task CreateGenre(GenreCreateViewModel viewModel);

        public Task<GenreEditViewModel> GetGenreEdit(Guid id);

        public Task GenreEdit(Guid id, GenreEditViewModel viewModel);
        
        public Task<Genre> GetGenreToConfirmDelete(Guid id);

        public Task DeleteGenre(Guid id);
    }
    
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenreIndexViewModel> GetGenresList()
        {
            var genres = _unitOfWork.Genres.GetAllOrderedByNameAscending();

            return new GenreIndexViewModel
            {
                Genres = await genres.ToListAsync()
            };
        }

        public async Task<GenreShowViewModel> GetGenreWithMovies(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                throw new GenreNotFound();
            }

            var movies = await _unitOfWork.Movies.GetMoviesWithGenresFromGenreOrderedByReleaseDate(genre.Id);

            return new GenreShowViewModel
            {
                Genre = genre,
                Movies = movies
            };
        }

        public async Task CreateGenre(GenreCreateViewModel viewModel)
        {
            await _unitOfWork.Genres.Create(new Genre { Name = viewModel.Name });
            await _unitOfWork.Save();
        }

        public async Task<GenreEditViewModel> GetGenreEdit(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                throw new GenreNotFound();
            }

            return new GenreEditViewModel
            {
                Name = genre.Name
            };
        }

        public async Task GenreEdit(Guid id, GenreEditViewModel viewModel)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                throw new GenreNotFound();
            }

            genre.Name = viewModel.Name;

            _unitOfWork.Genres.Update(genre);
            await _unitOfWork.Save();
        }

        public async Task<Genre> GetGenreToConfirmDelete(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                throw new GenreNotFound();
            }

            return genre;
        }

        public async Task DeleteGenre(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetBy(g => g.Id == id);

            if (genre == null)
            {
                throw new GenreNotFound();
            }

            _unitOfWork.Genres.Delete(genre);
            await _unitOfWork.Save();
        }
    }
}
