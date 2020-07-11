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
            CreateMap<MovieCreateRequest, Movie>();
            CreateMap<MovieUpdateRequest, Movie>().ReverseMap();
            CreateMap<Movie, MovieResponse>();
            CreateMap<User, MeResponse>();
        }
    }
}
