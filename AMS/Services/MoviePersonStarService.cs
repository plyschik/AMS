using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IMoviePersonStarService
    {
        public Task<IEnumerable<PersonResponse>> GetStarsForMovie(int movieId);
    }
    
    public class MoviePersonStarService : IMoviePersonStarService
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;

        public MoviePersonStarService(IMapper mapper, IMovieRepository movieRepository)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<PersonResponse>> GetStarsForMovie(int movieId)
        {
            var movie = await _movieRepository.GetWithStars(movieId);

            if (movie == null)
            {
                throw new MovieNotFound("Movie not found!");
            }

            return _mapper.Map<IEnumerable<PersonResponse>>(movie.Stars);
        }
    }
}
