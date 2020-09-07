using System;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Repositories
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        public IQueryable<Movie> GetMoviesWithGenresOrderedBy(string search, string sort, string order);

        public Task<Movie> GetMovieWithGenresDirectorsWritersAndStars(Guid id);

        public IQueryable<Movie> GetMoviesWithGenresFromGenreOrderedBy(Guid genreId, string search, string sort, string order);

        public IQueryable<Movie> GetMoviesWherePersonIsDirectorOrderedByReleaseDate(Guid personId);
        
        public IQueryable<Movie> GetMoviesWherePersonIsWriterOrderedByReleaseDate(Guid personId);
        
        public IQueryable<Movie> GetMoviesWherePersonIsStarOrderedByReleaseDate(Guid personId);
    }
    
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IQueryable<Movie> GetMoviesWithGenresOrderedBy(string search, string sort, string order)
        {
            IQueryable<Movie> queryable = DatabaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                
                queryable = queryable.Where(m => 
                    m.Title.ToLower().Contains(search) || m.Description.ToLower().Contains(search)
                );
            }
            
            switch (sort)
            {
                case "title":
                    queryable = order == "asc"
                        ? queryable.OrderBy(m => m.Title)
                        : queryable.OrderByDescending(m => m.Title);
                    
                    break;
                case "release_date":
                    queryable = order == "asc"
                        ? queryable.OrderBy(m => m.ReleaseDate)
                        : queryable.OrderByDescending(m => m.ReleaseDate);
                    
                    break;
                default:
                    queryable = queryable.OrderByDescending(m => m.ReleaseDate);
                    
                    break;
            }

            return queryable;
        }

        public async Task<Movie> GetMovieWithGenresDirectorsWritersAndStars(Guid id)
        {
            return await DatabaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieDirectors)
                .ThenInclude(md => md.Person)
                .Include(mw => mw.MovieWriters)
                .ThenInclude(mw => mw.Person)
                .Include(ms => ms.MovieStars)
                .ThenInclude(ms => ms.Person)
                .FirstOrDefaultAsync(movie => movie.Id == id);
        }

        public IQueryable<Movie> GetMoviesWithGenresFromGenreOrderedBy(Guid genreId, string search, string sort, string order)
        {
            IQueryable<Movie> queryable = DatabaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId));

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                
                queryable = queryable.Where(m => 
                    m.Title.ToLower().Contains(search) || m.Description.ToLower().Contains(search)
                );
            }
            
            switch (sort)
            {
                case "title":
                    queryable = order == "asc"
                        ? queryable.OrderBy(m => m.Title)
                        : queryable.OrderByDescending(m => m.Title);
                    
                    break;
                case "release_date":
                    queryable = order == "asc"
                        ? queryable.OrderBy(m => m.ReleaseDate)
                        : queryable.OrderByDescending(m => m.ReleaseDate);
                    
                    break;
                default:
                    queryable = queryable.OrderByDescending(m => m.ReleaseDate);
                    
                    break;
            }
            
            return queryable;
        }

        public IQueryable<Movie> GetMoviesWherePersonIsDirectorOrderedByReleaseDate(Guid personId)
        {
            return DatabaseContext.Movies
                .Include(m => m.MovieDirectors)
                .Where(m => m.MovieDirectors.Any(md => md.PersonId == personId))
                .OrderByDescending(m => m.ReleaseDate);
        }

        public IQueryable<Movie> GetMoviesWherePersonIsWriterOrderedByReleaseDate(Guid personId)
        {
            return DatabaseContext.Movies
                .Include(m => m.MovieWriters)
                .Where(m => m.MovieWriters.Any(mw => mw.PersonId == personId))
                .OrderByDescending(m => m.ReleaseDate);
        }

        public IQueryable<Movie> GetMoviesWherePersonIsStarOrderedByReleaseDate(Guid personId)
        {
            return DatabaseContext.Movies
                .Include(m => m.MovieStars)
                .Where(m => m.MovieStars.Any(ms => ms.PersonId == personId))
                .OrderByDescending(m => m.ReleaseDate);
        }
    }
}
