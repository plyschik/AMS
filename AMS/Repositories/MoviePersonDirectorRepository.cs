using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repositories
{
    public interface IMoviePersonDirectorRepository
    {
        public Task Create(MoviePersonDirector moviePersonDirector);

        public Task<MoviePersonDirector> Get(int movieId, int personId);

        public Task Delete(MoviePersonDirector moviePersonDirector);
        
        public Task<bool> IsDirectorAlreadyAttachedToMovie(int movieId, int personId);
    }
    
    public class MoviePersonDirectorRepository : IMoviePersonDirectorRepository
    {
        private readonly DatabaseContext _databaseContext;

        public MoviePersonDirectorRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task Create(MoviePersonDirector moviePersonDirector)
        {
            await _databaseContext.MoviePersonDirectors.AddAsync(moviePersonDirector);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<MoviePersonDirector> Get(int movieId, int personId)
        {
            return await _databaseContext.MoviePersonDirectors.FirstOrDefaultAsync(
                mpd => mpd.MovieId.Equals(movieId) && mpd.PersonId.Equals(personId)
            );
        }

        public async Task Delete(MoviePersonDirector moviePersonDirector)
        {
            _databaseContext.MoviePersonDirectors.Remove(moviePersonDirector);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<bool> IsDirectorAlreadyAttachedToMovie(int movieId, int personId)
        {
            return await _databaseContext.MoviePersonDirectors.AnyAsync(
                mpd => mpd.MovieId.Equals(movieId) && mpd.PersonId.Equals(personId)
            );
        }
    }
}
