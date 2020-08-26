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
        public IQueryable<Movie> GetMoviesWithGenresOrderedByReleaseDate();

        public Task<Movie> GetMovieWithGenresDirectorsWritersAndStars(Guid id);

        public IQueryable<Movie> GetMoviesWithGenresFromGenreOrderedByReleaseDate(Guid genreId);

        public IQueryable<Movie> GetMoviesWherePersonIsDirectorOrderedByReleaseDate(Guid personId);
        
        public IQueryable<Movie> GetMoviesWherePersonIsWriterOrderedByReleaseDate(Guid personId);
        
        public IQueryable<Movie> GetMoviesWherePersonIsStarOrderedByReleaseDate(Guid personId);
    }
    
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IQueryable<Movie> GetMoviesWithGenresOrderedByReleaseDate()
        {
            return DatabaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .OrderByDescending(m => m.ReleaseDate);
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

        public IQueryable<Movie> GetMoviesWithGenresFromGenreOrderedByReleaseDate(Guid genreId)
        {
            return DatabaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId))
                .OrderByDescending(m => m.ReleaseDate);
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
