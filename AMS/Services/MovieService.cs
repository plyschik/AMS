using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Requests;
using AMS.Data.Responses;
using AMS.Exceptions;
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

        public async Task<IEnumerable<MovieGetResponse>> GetAll()
        {
            var movies = await _movieRepository.GetAll();

            return _mapper.Map<IEnumerable<MovieGetResponse>>(movies);
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

        public async Task<MovieGetResponse> Update(int id, MovieUpdateRequest request)
        {
            var movieToUpdate = await _movieRepository.GetById(id);

            if (movieToUpdate == null)
            {
                throw new MovieNotFound();
            }

            _mapper.Map(request, movieToUpdate);

            var movie = await _movieRepository.Update(movieToUpdate);
            
            return _mapper.Map<MovieGetResponse>(_mapper.Map<MovieGetResponse>(movie));
        }

        public async Task Delete(int id)
        {
            var movieToDelete = await _movieRepository.GetById(id);

            if (movieToDelete == null)
            {
                throw new MovieNotFound();
            }

            await _movieRepository.Delete(movieToDelete);
        }
    }
}
