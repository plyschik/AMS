using AMS.Data.Models;
using AMS.Data.Requests;
using AMS.Data.Responses;
using AutoMapper;

namespace AMS
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, MeResponse>();
            
            CreateMap<MovieCreateRequest, Movie>();
            CreateMap<MovieUpdateRequest, Movie>().ReverseMap();
            CreateMap<Movie, MovieResponse>();
            
            CreateMap<GenreCreateRequest, Genre>();
            CreateMap<GenreUpdateRequest, Genre>().ReverseMap();
            CreateMap<Genre, GenreResponse>();

            CreateMap<PersonCreateRequest, Person>();
            CreateMap<PersonUpdateRequest, Person>().ReverseMap();
            CreateMap<Person, PersonResponse>();
        }
    }
}
