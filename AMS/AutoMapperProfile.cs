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
            CreateMap<Movie, MovieCreatedResponse>();
            CreateMap<Movie, MovieGetResponse>();
        }
    }
}
