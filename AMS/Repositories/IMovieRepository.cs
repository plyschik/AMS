using System.Threading.Tasks;
using AMS.Data.Models;

namespace AMS.Repositories
{
    public interface IMovieRepository
    {
        Task<Movie> Create(Movie movie);
    }
}
