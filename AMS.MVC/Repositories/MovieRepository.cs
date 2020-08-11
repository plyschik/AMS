using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Repositories
{
    public interface IMovieRepository
    {
        public Task<IList<Movie>> GetAll();
        
        public Task<IList<Movie>> GetAllWithGenres();

        public Task<Movie> GetById(Guid id);
        
        public Task<Movie> GetByIdWithGenres(Guid id);
        
        public Task<Movie> Create(Movie movie);

        public Task<Movie> Update(Movie movie);

        public Task Delete(Movie movie);

        public Task<bool> Exists(Guid id);
    }
    
    public class MovieRepository : IMovieRepository
    {
        private readonly DatabaseContext _databaseContext;

        public MovieRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IList<Movie>> GetAll()
        {
            return await _databaseContext.Movies
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();
        }
        
        public async Task<IList<Movie>> GetAllWithGenres()
        {
            return await _databaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(m => m.Genre)
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();
        }

        public async Task<Movie> GetById(Guid id)
        {
            return await _databaseContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id);
        }
        
        public async Task<Movie> GetByIdWithGenres(Guid id)
        {
            return await _databaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(movie => movie.Id == id);
        }

        public async Task<Movie> Create(Movie movie)
        {
            await _databaseContext.Movies.AddAsync(movie);
            await _databaseContext.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie> Update(Movie movie)
        {
            _databaseContext.Movies.Update(movie);
            await _databaseContext.SaveChangesAsync();

            return movie;
        }

        public async Task Delete(Movie movie)
        {
            _databaseContext.Movies.Remove(movie);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _databaseContext.Movies.AnyAsync(movie => movie.Id == id);
        }
    }
}
