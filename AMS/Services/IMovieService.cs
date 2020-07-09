using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Requests;
using AMS.Data.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace AMS.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieResponse>> GetAll();
        
        Task<MovieResponse> GetById(int id);

        Task<MovieResponse> Create(MovieCreateRequest request);

        Task<MovieResponse> Update(int id, MovieUpdateRequest request);

        Task<Movie> GetMovie(int id);

        MovieUpdateRequest MergeMovieModelWithPatchDocument(Movie movie, JsonPatchDocument<MovieUpdateRequest> document);
        
        Task<MovieResponse> UpdatePartial(MovieUpdateRequest request);

        Task Delete(int id);
    }
}
