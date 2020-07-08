using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Requests;
using AMS.Data.Responses;

namespace AMS.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieGetResponse>> GetAll();
        
        Task<MovieGetResponse> GetById(int id);
        
        Task<MovieCreatedResponse> Create(MovieCreateRequest request);

        Task<MovieGetResponse> Update(int id, MovieUpdateRequest request);
    }
}
