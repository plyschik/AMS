using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IMoviePersonDirectorService
    {
        public Task<IEnumerable<PersonResponse>> GetDirectorsForMovie(int movieId);

        public Task AttachDirectorToMovie(int movieId, int personId);

        public Task DetachDirectorFromMovie(int movieId, int personId);
    }
    
    public class MoviePersonDirectorService : IMoviePersonDirectorService
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMoviePersonDirectorRepository _moviePersonDirectorRepository;

        public MoviePersonDirectorService(
            IMapper mapper,
            IMovieRepository movieRepository,
            IPersonRepository personRepository,
            IMoviePersonDirectorRepository moviePersonDirectorRepository
        )
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
            _personRepository = personRepository;
            _moviePersonDirectorRepository = moviePersonDirectorRepository;
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

        public async Task AttachDirectorToMovie(int movieId, int personId)
        {
            if (!await _movieRepository.IsMovieExists(movieId))
            {
                throw new MovieNotFound("Movie not found!");
            }

            if (!await _personRepository.IsPersonExists(personId))
            {
                throw new PersonNotFound("Person not found!");
            }

            if (await _moviePersonDirectorRepository.IsDirectorAlreadyAttachedToMovie(movieId, personId))
            {
                throw new DirectorAlreadyAttachedToMovie("Director already attached to this movie!");
            }

            await _moviePersonDirectorRepository.Create(new MoviePersonDirector
            {
                MovieId = movieId,
                PersonId = personId
            });
        }

        public async Task DetachDirectorFromMovie(int movieId, int personId)
        {
            var moviePersonDirector = await _moviePersonDirectorRepository.Get(movieId, personId);

            if (moviePersonDirector == null)
            {
                throw new MoviePersonDirectorNotFound("Director is not attached to this movie.");
            }

            await _moviePersonDirectorRepository.Delete(moviePersonDirector);
        }
    }
}
