using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DatabaseContext _databaseContext;

        public MovieRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await _databaseContext.Movies.ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _databaseContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id);
        }

        public async Task<Movie> Create(Movie movie)
        {
            await _databaseContext.AddAsync(movie);
            await _databaseContext.SaveChangesAsync();

            return movie;
        }
    }
}
