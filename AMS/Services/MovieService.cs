using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Requests;
using AMS.Data.Responses;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMapper _mapper;
        
        private readonly IMovieRepository _movieRepository;

        public MovieService(
            IMapper mapper,
            IMovieRepository movieRepository
        )
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
        }

        public async Task<MovieGetResponse> GetById(int id)
        {
            var movie = await _movieRepository.GetById(id);

            return _mapper.Map<MovieGetResponse>(movie);
        }

        public async Task<MovieCreatedResponse> Create(MovieCreateRequest request)
        {
            var movie = await _movieRepository.Create(_mapper.Map<Movie>(request));

            return _mapper.Map<MovieCreatedResponse>(movie);
        }
    }
}
