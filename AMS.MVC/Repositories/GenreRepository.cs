using System.Linq;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;

namespace AMS.MVC.Repositories
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
        public IQueryable<Genre> GetAllOrderedByNameAscending();
    }

    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IQueryable<Genre> GetAllOrderedByNameAscending()
        {
            return DatabaseContext.Genres
                .OrderBy(g => g.Name);
        }
    }
}
