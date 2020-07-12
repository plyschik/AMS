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
    public interface IGenreService
    {
        public Task<IEnumerable<GenreResponse>> GetAll();
        
        public Task<GenreResponse> GetById(int id);
        
        public Task<GenreResponse> Create(GenreCreateRequest request);

        public Task<GenreResponse> Update(int id, GenreUpdateRequest request);
        
        public Task<Genre> GetGenre(int id);

        public GenreUpdateRequest MergeGenreModelWithPatchDocument(Genre genre, JsonPatchDocument<GenreUpdateRequest> document);
        
        public Task<GenreResponse> UpdatePartial(GenreUpdateRequest request);

        public Task Delete(int id);
    }
    
    public class GenreService : IGenreService
    {
        private readonly IMapper _mapper;
        
        private readonly IGenreRepository _genreRepository;

        public GenreService(IMapper mapper, IGenreRepository genreRepository)
        {
            _mapper = mapper;
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<GenreResponse>> GetAll()
        {
            var genres = await _genreRepository.GetAll();

            return _mapper.Map<IEnumerable<GenreResponse>>(genres);
        }
        
        public async Task<GenreResponse> GetById(int id)
        {
            var genre = await _genreRepository.GetById(id);

            if (genre == null)
            {
                throw new GenreNotFound();
            }

            return _mapper.Map<GenreResponse>(genre);
        }

        public async Task<GenreResponse> Create(GenreCreateRequest request)
        {
            if (await _genreRepository.IsGenreAlreadyExists(request.Name))
            {
                throw new GenreAlreadyExists("Genre already exists!");
            }
            
            var genre = await _genreRepository.Create(_mapper.Map<Genre>(request));

            return _mapper.Map<GenreResponse>(genre);
        }

        public async Task<GenreResponse> Update(int id, GenreUpdateRequest request)
        {
            var genreToUpdate = await _genreRepository.GetById(id);

            if (genreToUpdate == null)
            {
                throw new GenreNotFound();
            }

            _mapper.Map(request, genreToUpdate);

            var genre = await _genreRepository.Update(genreToUpdate);

            return _mapper.Map<GenreResponse>(genre);
        }
        
        public async Task<Genre> GetGenre(int id)
        {
            var genre = await _genreRepository.GetById(id);

            if (genre == null)
            {
                throw new GenreNotFound();
            }

            return genre;
        }
        
        public GenreUpdateRequest MergeGenreModelWithPatchDocument(Genre genre, JsonPatchDocument<GenreUpdateRequest> document)
        {
            var genreUpdateRequest = _mapper.Map<GenreUpdateRequest>(genre);
            document.ApplyTo(genreUpdateRequest);

            return genreUpdateRequest;
        }
        
        public async Task<GenreResponse> UpdatePartial(GenreUpdateRequest request)
        {
            var genre = await _genreRepository.Update(_mapper.Map<Genre>(request));
            
            return _mapper.Map<GenreResponse>(genre);
        }
        
        public async Task Delete(int id)
        {
            var genre = await _genreRepository.GetById(id);

            if (genre == null)
            {
                throw new GenreNotFound();
            }

            await _genreRepository.Delete(genre);
        }
    }
}
