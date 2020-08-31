using System;
using AMS.MVC.Data.Models;
using AMS.MVC.ViewModels.GenreViewModels;
using AMS.MVC.ViewModels.MovieStarViewModel;
using AMS.MVC.ViewModels.MovieViewModels;
using AMS.MVC.ViewModels.PersonViewModels;
using AutoMapper;

namespace AMS.MVC
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Guid, string>().ConstructUsing(g => g.ToString());
            CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
            
            CreateMap<MovieCreateViewModel, Movie>();
            CreateMap<MovieEditViewModel, Movie>().ReverseMap();
            
            CreateMap<GenreCreateViewModel, Genre>();
            CreateMap<GenreEditViewModel, Genre>().ReverseMap();

            CreateMap<PersonCreateViewModel, Person>();
            CreateMap<PersonEditViewModel, Person>().ReverseMap();

            CreateMap<MovieStarCreateViewModel, MovieStar>()
                .ForMember(
                    destination => destination.PersonId,
                    options =>
                    {
                        options.MapFrom(source => source.SelectedPerson);
                    }
                );

            CreateMap<MovieStarEditViewModel, MovieStar>().ReverseMap();
        }
    }
}
