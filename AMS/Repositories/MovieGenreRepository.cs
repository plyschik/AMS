using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repositories
{
    public interface IMovieGenreRepository
    {
        public Task Create(MovieGenre movieGenre);

        public Task<MovieGenre> Get(int movieId, int genreId);

        public Task Delete(MovieGenre movieGenre);

        public Task<bool> IsGenreAlreadyAttachedToMovie(int movieId, int genreId);
    }
    
    public class MovieGenreRepository : IMovieGenreRepository
    {
        private readonly DatabaseContext _databaseContext;

        public MovieGenreRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<MovieGenre> Get(int movieId, int genreId)
        {
            return await _databaseContext.MovieGenres.FirstOrDefaultAsync(
                mg => mg.MovieId == movieId && mg.GenreId == genreId
            );
        }
        
        public async Task Create(MovieGenre movieGenre)
        {
            await _databaseContext.MovieGenres.AddAsync(movieGenre);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task Delete(MovieGenre movieGenre)
        {
            _databaseContext.MovieGenres.Remove(movieGenre);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<bool> IsGenreAlreadyAttachedToMovie(int movieId, int genreId)
        {
            return await _databaseContext.MovieGenres.AnyAsync(
                mg => mg.MovieId == movieId && mg.GenreId == genreId
            );
        }
    }
}
