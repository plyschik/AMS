using AMS.Data.Models;
using AMS.Data.Requests;
using AutoMapper;

namespace AMS
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MovieCreateRequest, Movie>();
        }
    }
}
