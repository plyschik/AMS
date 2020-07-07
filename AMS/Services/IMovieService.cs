using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Requests;

namespace AMS.Services
{
    public interface IMovieService
    {
        Task<Movie> Create(MovieCreateRequest request);
    }
}
