using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Repositories
{
    public interface IMovieRepository
    {
        public Task<IEnumerable<Movie>> GetAll();

        public Task<Movie> GetById(Guid id);
        
        public Task<Movie> Create(Movie movie);
    }
    
    public class MovieRepository : IMovieRepository
    {
        private readonly DatabaseContext _databaseContext;

        public MovieRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await _databaseContext.Movies.ToListAsync();
        }

        public async Task<Movie> GetById(Guid id)
        {
            return await _databaseContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id);
        }

        public async Task<Movie> Create(Movie movie)
        {
            await _databaseContext.Movies.AddAsync(movie);
            await _databaseContext.SaveChangesAsync();

            return movie;
        }
    }
}
