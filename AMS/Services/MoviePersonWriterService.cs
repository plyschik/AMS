using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IMoviePersonWriterService
    {
        public Task<IEnumerable<PersonResponse>> GetWritersForMovie(int movieId);
    }
    
    public class MoviePersonWriterService : IMoviePersonWriterService
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;

        public MoviePersonWriterService(IMapper mapper, IMovieRepository movieRepository)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<PersonResponse>> GetWritersForMovie(int movieId)
        {
            var movie = await _movieRepository.GetWithWriters(movieId);

            if (movie == null)
            {
                throw new MovieNotFound("Movie not found!");
            }

            return _mapper.Map<IEnumerable<PersonResponse>>(movie.Writers);
        }
    }
}
