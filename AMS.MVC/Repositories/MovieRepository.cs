using AMS.MVC.Data;
using AMS.MVC.Data.Models;

namespace AMS.MVC.Repositories
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
    }
    
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
