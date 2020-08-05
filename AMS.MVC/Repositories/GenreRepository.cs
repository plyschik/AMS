using System;
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

        public Task<Genre> GetById(Guid id);

        public Task<Genre> Create(Genre genre);

        public Task<Genre> Update(Genre genre);

        public Task<bool> Exists(Guid id);
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

        public async Task<Genre> GetById(Guid id)
        {
            return await _databaseContext.Genres.FirstOrDefaultAsync(genre => genre.Id == id);
        }

        public async Task<Genre> Create(Genre genre)
        {
            await _databaseContext.Genres.AddAsync(genre);
            await _databaseContext.SaveChangesAsync();

            return genre;
        }

        public async Task<Genre> Update(Genre genre)
        {
            _databaseContext.Genres.Update(genre);
            await _databaseContext.SaveChangesAsync();
            
            return genre;
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _databaseContext.Genres.AnyAsync(genre => genre.Id == id);
        }
    }
}
