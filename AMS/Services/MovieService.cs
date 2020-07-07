using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Requests;
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

        public async Task<Movie> Create(MovieCreateRequest request)
        {
            var movie = _mapper.Map<Movie>(request);

            return await _movieRepository.Create(movie);
        }
    }
}
