using System.Threading.Tasks;
using AMS.MVC.Repositories;

namespace AMS.MVC.Data
{
    public interface IUnitOfWork
    {
        public IMovieRepository MovieRepository { get; }
        
        public IGenreRepository GenreRepository { get; }
        
        public Task SaveChangesAsync();
    }
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext; 
        
        public IMovieRepository MovieRepository { get; }
        
        public IGenreRepository GenreRepository { get; }

        public UnitOfWork(
            DatabaseContext databaseContext,
            IMovieRepository movieRepository,
            IGenreRepository genreRepository
        )
        {
            _databaseContext = databaseContext;
            MovieRepository = movieRepository;
            GenreRepository = genreRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
