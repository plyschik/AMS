using System;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Repositories
{
    public interface IMovieStarRepository : IBaseRepository<MovieStar>
    {
        public Task<MovieStar> GetMovieStarWithPersonByMovieIdAndPersonId(Guid movieId, Guid personId);

        public IQueryable<Person> GetStarsAsPersons(Guid movieId);
    }

    public class MovieStarRepository : BaseRepository<MovieStar>, IMovieStarRepository
    {
        public MovieStarRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<MovieStar> GetMovieStarWithPersonByMovieIdAndPersonId(Guid movieId, Guid personId)
        {
            return await DatabaseContext.MovieStars
                .Include(ms => ms.Person)
                .FirstOrDefaultAsync(ms => ms.MovieId == movieId && ms.PersonId == personId);
        }

        public IQueryable<Person> GetStarsAsPersons(Guid movieId)
        {
            return DatabaseContext.MovieStars
                .Include(ms => ms.Person)
                .Where(ms => ms.MovieId == movieId)
                .Select(ms => ms.Person);
        }
    }
}
