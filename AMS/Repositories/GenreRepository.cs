using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repositories
{
    public interface IGenreRepository
    {
        public Task<Genre> Create(Genre genre);

        public Task<bool> IsGenreAlreadyExists(string name);
    }
    
    public class GenreRepository : IGenreRepository
    {
        private readonly DatabaseContext _dbContext;

        public GenreRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Genre> Create(Genre genre)
        {
            await _dbContext.Genres.AddAsync(genre);
            await _dbContext.SaveChangesAsync();

            return genre;
        }

        public async Task<bool> IsGenreAlreadyExists(string name)
        {
            return await _dbContext.Genres.AnyAsync(
                genre => genre.Name.ToLower().Equals(name.ToLower())
            );
        }
    }
}
