using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Requests;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IGenreService
    {
        public Task<GenreResponse> GetById(int id);
        
        public Task<GenreResponse> Create(GenreCreateRequest request);
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
    }
}
