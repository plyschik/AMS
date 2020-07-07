using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;

namespace AMS.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DatabaseContext _databaseContext;

        public MovieRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Movie> Create(Movie movie)
        {
            await _databaseContext.AddAsync(movie);
            await _databaseContext.SaveChangesAsync();

            return movie;
        }
    }
}
