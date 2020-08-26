using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Repositories
{
    public interface IMovieStarRepository : IBaseRepository<MovieStar>
    {
        public Task<MovieStar> GetByWithPerson(Expression<Func<MovieStar, bool>> expression);
    }

    public class MovieStarRepository : BaseRepository<MovieStar>, IMovieStarRepository
    {
        public MovieStarRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
        
        public async Task<MovieStar> GetByWithPerson(Expression<Func<MovieStar, bool>> expression)
        {
            return await DatabaseContext.MovieStars
                .Include(ms => ms.Person)
                .FirstOrDefaultAsync(expression);
        }
    }
}
