using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IMoviePersonWriterService
    {
        public Task<IEnumerable<PersonResponse>> GetWritersForMovie(int movieId);

        public Task AttachWriterToMovie(int movieId, int personId);

        public Task DetachWriterFromMovie(int movieId, int personId);
    }
    
    public class MoviePersonWriterService : IMoviePersonWriterService
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMoviePersonWriterRepository _moviePersonWriterRepository;

        public MoviePersonWriterService(
            IMapper mapper,
            IMovieRepository movieRepository,
            IPersonRepository personRepository,
            IMoviePersonWriterRepository moviePersonWriterRepository
        )
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
            _personRepository = personRepository;
            _moviePersonWriterRepository = moviePersonWriterRepository;
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

        public async Task AttachWriterToMovie(int movieId, int personId)
        {
            if (!await _movieRepository.IsMovieExists(movieId))
            {
                throw new MovieNotFound("Movie not found!");
            }

            if (!await _personRepository.IsPersonExists(personId))
            {
                throw new PersonNotFound("Person not found!");
            }

            if (await _moviePersonWriterRepository.IsWriterAlreadyAttachedToMovie(movieId, personId))
            {
                throw new WriterAlreadyAttachedToMovie("Writer already attached to this movie!");
            }

            await _moviePersonWriterRepository.Create(new MoviePersonWriter
            {
                MovieId = movieId,
                PersonId = personId
            });
        }

        public async Task DetachWriterFromMovie(int movieId, int personId)
        {
            var moviePersonWriter = await _moviePersonWriterRepository.Get(movieId, personId);

            if (moviePersonWriter == null)
            {
                throw new MoviePersonWriterNotFound("Writer is not attached to this movie.");
            }

            await _moviePersonWriterRepository.Delete(moviePersonWriter);
        }
    }
}
