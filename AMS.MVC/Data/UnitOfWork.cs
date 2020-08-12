using System.Threading.Tasks;
using AMS.MVC.Repositories;

namespace AMS.MVC.Data
{
    public interface IUnitOfWork
    {
        public IMovieRepository MovieRepository { get; }
        
        public IGenreRepository GenreRepository { get; }

        public IPersonRepository PersonRepository { get; }
        
        public Task SaveChangesAsync();
    }
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext; 
        
        public IMovieRepository MovieRepository { get; }
        
        public IGenreRepository GenreRepository { get; }

        public IPersonRepository PersonRepository { get; }

        public UnitOfWork(
            DatabaseContext databaseContext,
            IMovieRepository movieRepository,
            IGenreRepository genreRepository,
            IPersonRepository personRepository
        )
        {
            _databaseContext = databaseContext;
            MovieRepository = movieRepository;
            GenreRepository = genreRepository;
            PersonRepository = personRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
