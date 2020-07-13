using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IMovieGenreService
    {
        public Task<IEnumerable<GenreResponse>> GetGenresForMovie(int movieId);
    }
    
    public class MovieGenreService : IMovieGenreService
    {
        private readonly IMapper _mapper;
        
        private readonly IMovieRepository _movieRepository;

        public MovieGenreService(IMapper mapper, IMovieRepository movieRepository)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<GenreResponse>> GetGenresForMovie(int movieId)
        {
            var movie = await _movieRepository.GetWithGenres(movieId);

            if (movie == null)
            {
                throw new MovieNotFound();
            }

            return _mapper.Map<IEnumerable<GenreResponse>>(movie.Genres);
        }
    }
}
