using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Requests;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

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

        public async Task<IEnumerable<MovieResponse>> GetAll()
        {
            var movies = await _movieRepository.GetAll();

            return _mapper.Map<IEnumerable<MovieResponse>>(movies);
        }
        
        public async Task<MovieResponse> GetById(int id)
        {
            var movie = await _movieRepository.GetById(id);

            if (movie == null)
            {
                throw new MovieNotFound();
            }

            return _mapper.Map<MovieResponse>(movie);
        }

        public async Task<MovieResponse> Create(MovieCreateRequest request)
        {
            var movie = await _movieRepository.Create(_mapper.Map<Movie>(request));

            return _mapper.Map<MovieResponse>(movie);
        }

        public async Task<MovieResponse> Update(int id, MovieUpdateRequest request)
        {
            var movieToUpdate = await _movieRepository.GetById(id);

            if (movieToUpdate == null)
            {
                throw new MovieNotFound();
            }

            _mapper.Map(request, movieToUpdate);

            var movie = await _movieRepository.Update(movieToUpdate);
            
            return _mapper.Map<MovieResponse>(movie);
        }

        public async Task<Movie> GetMovie(int id)
        {
            var movie = await _movieRepository.GetById(id);

            if (movie == null)
            {
                throw new MovieNotFound();
            }

            return movie;
        }
        
        public MovieUpdateRequest MergeMovieModelWithPatchDocument(Movie movie, JsonPatchDocument<MovieUpdateRequest> document)
        {
            var movieUpdateRequest = _mapper.Map<MovieUpdateRequest>(movie);
            document.ApplyTo(movieUpdateRequest);

            return movieUpdateRequest;
        }
        
        public async Task<MovieResponse> UpdatePartial(MovieUpdateRequest movieToPatch, Movie movieFromDatabase)
        {
            _mapper.Map(movieToPatch, movieFromDatabase);
            
            await _movieRepository.Update(_mapper.Map<Movie>(movieFromDatabase));
            
            return _mapper.Map<MovieResponse>(movieFromDatabase);
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
