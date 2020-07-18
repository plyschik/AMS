using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IMoviePersonStarService
    {
        public Task<IEnumerable<PersonResponse>> GetStarsForMovie(int movieId);

        public Task AttachStarToMovie(int movieId, int personId);
    }
    
    public class MoviePersonStarService : IMoviePersonStarService
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMoviePersonStarRepository _moviePersonStarRepository;

        public MoviePersonStarService(IMapper mapper, IMovieRepository movieRepository, IPersonRepository personRepository, IMoviePersonStarRepository moviePersonStarRepository)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
            _personRepository = personRepository;
            _moviePersonStarRepository = moviePersonStarRepository;
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

        public async Task AttachStarToMovie(int movieId, int personId)
        {
            if (!await _movieRepository.IsMovieExists(movieId))
            {
                throw new MovieNotFound("Movie not found!");
            }

            if (!await _personRepository.IsPersonExists(personId))
            {
                throw new PersonNotFound("Person not found!");
            }

            if (await _moviePersonStarRepository.IsStarAlreadyAttachedToMovie(movieId, personId))
            {
                throw new StarAlreadyAttachedToMovie("Star already attached to this movie!");
            }

            await _moviePersonStarRepository.Create(new MoviePersonStar
            {
                MovieId = movieId,
                PersonId = personId
            });
        }
    }
}
