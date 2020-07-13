using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repositories
{
    public interface IMovieRepository
    {
        public Task<IEnumerable<Movie>> GetAll();
        
        public Task<Movie> GetById(int id);
        
        public Task<Movie> Create(Movie movie);

        public Task<Movie> Update(Movie movie);

        public Task Delete(Movie movie);
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
            return await _databaseContext.Movies.OrderByDescending(movie => movie.Id).ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _databaseContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id);
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
    }
}
