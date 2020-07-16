using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repositories
{
    public interface IMoviePersonWriterRepository
    {
        public Task Create(MoviePersonWriter moviePersonWriter);

        public Task<bool> IsWriterAlreadyAttachedToMovie(int movieId, int personId);
    }
    
    public class MoviePersonWriterRepository : IMoviePersonWriterRepository
    {
        private readonly DatabaseContext _databaseContext;

        public MoviePersonWriterRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task Create(MoviePersonWriter moviePersonWriter)
        {
            await _databaseContext.MoviePersonWriters.AddAsync(moviePersonWriter);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<bool> IsWriterAlreadyAttachedToMovie(int movieId, int personId)
        {
            return await _databaseContext.MoviePersonWriters.AnyAsync(
                mpw => mpw.MovieId.Equals(movieId) && mpw.PersonId.Equals(personId)
            );
        }
    }
}