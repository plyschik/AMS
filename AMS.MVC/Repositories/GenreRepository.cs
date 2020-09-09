using System.Linq;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Repositories
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
        public IQueryable<Genre> GetAllOrderedBy(string sort, string order);
    }

    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IQueryable<Genre> GetAllOrderedBy(string sort, string order)
        {
            IQueryable<Genre> queryable = DatabaseContext.Genres.Include(g => g.MovieGenres);

            switch (sort)
            {
                case "name":
                    queryable = order == "asc"
                        ? queryable.OrderBy(g => g.Name)
                        : queryable.OrderByDescending(g => g.Name);
                    
                    break;
                default:
                    queryable = queryable.OrderBy(g => g.Name);
                    
                    break;
            }

            return queryable;
        }
    }
}
