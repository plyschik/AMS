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
        public Task<IList<Genre>> GetAll();

        public Task<Genre> GetById(Guid id);

        public void Create(Genre genre);

        public void Update(Genre genre);

        public void Delete(Genre genre);

        public Task<bool> Exists(Guid id);
    }

    public class GenreRepository : IGenreRepository
    {
        private readonly DatabaseContext _databaseContext;

        public GenreRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IList<Genre>> GetAll()
        {
            return await _databaseContext.Genres.OrderBy(genre => genre.Name).ToListAsync();
        }

        public async Task<Genre> GetById(Guid id)
        {
            return await _databaseContext.Genres.FirstOrDefaultAsync(genre => genre.Id == id);
        }

        public void Create(Genre genre)
        {
            _databaseContext.Genres.Add(genre);
        }

        public void Update(Genre genre)
        {
            _databaseContext.Genres.Update(genre);
        }

        public void Delete(Genre genre)
        {
            _databaseContext.Genres.Remove(genre);
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _databaseContext.Genres.AnyAsync(genre => genre.Id == id);
        }
    }
}
