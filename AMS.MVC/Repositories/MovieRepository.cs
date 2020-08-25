using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Repositories
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        public Task<Movie> GetByIdWithRelations(Guid id);
        
        public Task<ICollection<Movie>> GetAllWithRelations();
        
        public Task<ICollection<Person>> GetStars(Guid movieId);
        
        public Task<ICollection<Movie>> GetMoviesWithGenresOrderedByReleaseDate();

        public Task<Movie> GetMovieWithGenresDirectorsWritersAndStars(Guid id);

        public Task<ICollection<Movie>> GetMoviesWithGenresFromGenreOrderedByReleaseDate(Guid genreId);
    }
    
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<Movie> GetByIdWithRelations(Guid id)
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
        
        public async Task<ICollection<Movie>> GetAllWithRelations()
        {
            return await DatabaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieDirectors)
                .ThenInclude(md => md.Person)
                .Include(mw => mw.MovieWriters)
                .ThenInclude(mw => mw.Person)
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();
        }
        
        public async Task<ICollection<Person>> GetStars(Guid movieId)
        {
            var movie = await DatabaseContext.Movies
                .Include(m => m.MovieStars)
                .ThenInclude(ms => ms.Person)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            return movie.MovieStars.Select(ms => ms.Person).ToList();
        }

        public async Task<ICollection<Movie>> GetMoviesWithGenresOrderedByReleaseDate()
        {
            return await DatabaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();
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

        public async Task<ICollection<Movie>> GetMoviesWithGenresFromGenreOrderedByReleaseDate(Guid genreId)
        {
            return await DatabaseContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId))
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();
        }
    }
}
