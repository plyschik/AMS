using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Repositories
{
    public interface IGenreRepository
    {
        public Task<IEnumerable<Genre>> GetAll();

        public Task<Genre> Create(Genre genre);
    }

    public class GenreRepository : IGenreRepository
    {
        private readonly DatabaseContext _databaseContext;

        public GenreRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _databaseContext.Genres.OrderBy(genre => genre.Name).ToListAsync();
        }

        public async Task<Genre> Create(Genre genre)
        {
            await _databaseContext.Genres.AddAsync(genre);
            await _databaseContext.SaveChangesAsync();

            return genre;
        }
    }
}
