using System.Threading.Tasks;
using AMS.MVC.Data;

namespace AMS.MVC.Repositories
{
    public interface IUnitOfWork
    {
        IMovieRepository Movies { get; }

        IGenreRepository Genres { get; }
        
        IPersonRepository Persons { get; }

        Task Save();
    }
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext;
        private IMovieRepository _movieRepository;
        private IGenreRepository _genreRepository;
        private IPersonRepository _personRepository;

        public IMovieRepository Movies
        {
            get
            {
                if (_movieRepository == null)
                {
                    _movieRepository = new MovieRepository(_databaseContext);
                }

                return _movieRepository;
            }
        }

        public IGenreRepository Genres
        {
            get
            {
                if (_genreRepository == null)
                {
                    _genreRepository = new GenreRepository(_databaseContext);
                }

                return _genreRepository;
            }
        }

        public IPersonRepository Persons
        {
            get
            {
                if (_personRepository == null)
                {
                    _personRepository = new PersonRepository(_databaseContext);
                }

                return _personRepository;
            }
        }

        public UnitOfWork(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task Save()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
