using System.Threading.Tasks;
using AMS.Data.Requests;
using AMS.Data.Responses;

namespace AMS.Services
{
    public interface IMovieService
    {
        Task<MovieGetResponse> GetById(int id);
        
        Task<MovieCreatedResponse> Create(MovieCreateRequest request);
    }
}
