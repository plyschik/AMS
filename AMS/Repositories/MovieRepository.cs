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
        
        public Task<Movie> GetWithGenres(int id);

        public Task<Movie> GetWithDirectors(int id);

        public Task<Movie> GetWithWriters(int id);

        public Task<Movie> GetWithStars(int id);

        public Task<bool> IsMovieExists(int id);
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

        public async Task<Movie> GetWithGenres(int id)
        {
            var movie = await _databaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movie;
        }

        public async Task<Movie> GetWithDirectors(int id)
        {
            var movie = await _databaseContext.Movies
                .Include(m => m.MoviePersonDirectors)
                .ThenInclude(mpd => mpd.Person)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movie;
        }

        public async Task<Movie> GetWithWriters(int id)
        {
            var movie = await _databaseContext.Movies
                .Include(m => m.MoviePersonWriters)
                .ThenInclude(mpw => mpw.Person)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));

            return movie;
        }

        public async Task<Movie> GetWithStars(int id)
        {
            var movie = await _databaseContext.Movies
                .Include(m => m.MoviePersonStars)
                .ThenInclude(mps => mps.Person)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));

            return movie;
        }

        public async Task<bool> IsMovieExists(int id)
        {
            return await _databaseContext.Movies.AnyAsync(movie => movie.Id == id);
        }
    }
}
