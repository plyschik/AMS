using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IMovieGenreService
    {
        public Task<IEnumerable<GenreResponse>> GetGenresForMovie(int movieId);

        public Task AttachGenreToMovie(int movieId, int genreId);
    }
    
    public class MovieGenreService : IMovieGenreService
    {
        private readonly IMapper _mapper;
        
        private readonly IMovieRepository _movieRepository;

        private readonly IGenreRepository _genreRepository;

        private readonly IMovieGenreRepository _movieGenreRepository;

        public MovieGenreService(
            IMapper mapper,
            IMovieRepository movieRepository,
            IGenreRepository genreRepository,
            IMovieGenreRepository movieGenreRepository
        )
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _movieGenreRepository = movieGenreRepository;
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

        public async Task AttachGenreToMovie(int movieId, int genreId)
        {
            if (!await _movieRepository.IsMovieExists(movieId))
            {
                throw new MovieNotFound("Movie not found!");
            }

            if (!await _genreRepository.IsGenreExists(genreId))
            {
                throw new GenreNotFound("Genre not found!");
            }
            
            if (await _movieGenreRepository.IsGenreAlreadyAttachedToMovie(movieId, genreId))
            {
                throw new GenreAlreadyAttachedToMovie("Genre already attached to this movie!");
            }
            
            await _movieGenreRepository.Create(new MovieGenre
            {
                MovieId = movieId,
                GenreId = genreId
            });
        }
    }
}
