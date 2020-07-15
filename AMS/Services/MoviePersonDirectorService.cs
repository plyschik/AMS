using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IMoviePersonDirectorService
    {
        public Task<IEnumerable<PersonResponse>> GetDirectorsForMovie(int movieId);
    }
    
    public class MoviePersonDirectorService : IMoviePersonDirectorService
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;

        public MoviePersonDirectorService(IMapper mapper, IMovieRepository movieRepository)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<PersonResponse>> GetDirectorsForMovie(int movieId)
        {
            var movie = await _movieRepository.GetWithDirectors(movieId);

            if (movie == null)
            {
                throw new MovieNotFound("Movie not found!");
            }

            return _mapper.Map<IEnumerable<PersonResponse>>(movie.Directors);
        }
    }
}
