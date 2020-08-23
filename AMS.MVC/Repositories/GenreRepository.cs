using AMS.MVC.Data;
using AMS.MVC.Data.Models;

namespace AMS.MVC.Repositories
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
    }

    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}