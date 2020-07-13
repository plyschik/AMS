using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repositories
{
    public interface IGenreRepository
    {
        public Task<IEnumerable<Genre>> GetAll();
        
        public Task<Genre> GetById(int id);
            
        public Task<Genre> Create(Genre genre);

        public Task<Genre> Update(Genre genre);

        public Task Delete(Genre genre);
        
        public Task<bool> IsGenreExists(int id);
        
        public Task<bool> IsGenreExists(string name);
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

        public async Task<Genre> GetById(int id)
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

        public async Task Delete(Genre genre)
        {
            _databaseContext.Genres.Remove(genre);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<bool> IsGenreExists(int id)
        {
            return await _databaseContext.Genres.AnyAsync(genre => genre.Id == id);
        }

        public async Task<bool> IsGenreExists(string name)
        {
            return await _databaseContext.Genres.AnyAsync(
                genre => genre.Name.ToLower().Equals(name.ToLower())
            );
        }
    }
}
