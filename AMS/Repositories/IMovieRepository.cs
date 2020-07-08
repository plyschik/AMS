using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Models;

namespace AMS.Repositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAll();
        
        Task<Movie> GetById(int id);
        
        Task<Movie> Create(Movie movie);

        Task<Movie> Update(Movie movie);

        Task Delete(Movie movie);
    }
}
