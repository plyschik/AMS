using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repositories
{
    public interface IMoviePersonStarRepository
    {
        public Task<MoviePersonStar> Get(int movieId, int personId);
        
        public Task Create(MoviePersonStar moviePersonStar);
        
        public Task Delete(MoviePersonStar moviePersonStar);

        public Task<bool> IsStarAlreadyAttachedToMovie(int movieId, int personId);
    }
    
    public class MoviePersonStarRepository : IMoviePersonStarRepository
    {
        private readonly DatabaseContext _databaseContext;

        public MoviePersonStarRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<MoviePersonStar> Get(int movieId, int personId)
        {
            return await _databaseContext.MoviePersonStars.FirstOrDefaultAsync(
                mps => mps.MovieId.Equals(movieId) && mps.PersonId.Equals(personId)
            );
        }

        public async Task Create(MoviePersonStar moviePersonStar)
        {
            await _databaseContext.MoviePersonStars.AddAsync(moviePersonStar);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task Delete(MoviePersonStar moviePersonStar)
        {
            _databaseContext.MoviePersonStars.Remove(moviePersonStar);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<bool> IsStarAlreadyAttachedToMovie(int movieId, int personId)
        {
            return await _databaseContext.MoviePersonStars.AnyAsync(
                mps => mps.MovieId.Equals(movieId) && mps.PersonId.Equals(personId)
            );
        }
    }
}
