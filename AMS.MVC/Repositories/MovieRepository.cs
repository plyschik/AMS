using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;

namespace AMS.MVC.Repositories
{
    public interface IMovieRepository
    {
        public Task<Movie> Create(Movie movie);
    }
    
    public class MovieRepository : IMovieRepository
    {
        private readonly DatabaseContext _databaseContext;

        public MovieRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Movie> Create(Movie movie)
        {
            await _databaseContext.Movies.AddAsync(movie);
            await _databaseContext.SaveChangesAsync();

            return movie;
        }
    }
}
