using System;
using System.Threading.Tasks;
using AMS.MVC.Data.Models;
using AMS.MVC.Exceptions.Person;
using AMS.MVC.Helpers;
using AMS.MVC.Repositories;
using AMS.MVC.ViewModels.PersonViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Services
{
    public interface IPersonService
    {
        public Task<PersonIndexViewModel> GetPersonsList(string search, string sort, string order, int page);

        public Task<PersonShowViewModel> GetPerson(Guid id);
        
        public Task CreatePerson(PersonCreateViewModel viewModel);

        public Task<PersonEditViewModel> GetEditViewModel(Guid id);

        public Task UpdatePerson(Guid id, PersonEditViewModel viewModel);
        
        public Task<Person> GetPersonToConfirmDelete(Guid id);

        public Task DeletePerson(Guid id);
    }
    
    public class PersonService : IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PersonIndexViewModel> GetPersonsList(string search, string sort, string order, int page)
        {
            return new PersonIndexViewModel
            {
                Paginator = await new Paginator<Person>().Create(
                    _unitOfWork.Persons.GetAllOrderedBy(search, sort, order),
                    page,
                    5
                )
            };
        }

        public async Task<PersonShowViewModel> GetPerson(Guid id)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFoundException();
            }

            return new PersonShowViewModel
            {
                Person = person,
                MovieDirector = await _unitOfWork.Movies.GetMoviesWherePersonIsDirectorOrderedByReleaseDate(person.Id).ToListAsync(),
                MovieWriter = await _unitOfWork.Movies.GetMoviesWherePersonIsWriterOrderedByReleaseDate(person.Id).ToListAsync(),
                MovieStar = await _unitOfWork.Movies.GetMoviesWherePersonIsStarOrderedByReleaseDate(person.Id).ToListAsync()
            };
        }

        public async Task CreatePerson(PersonCreateViewModel viewModel)
        {
            var person = _mapper.Map<Person>(viewModel);
            
            await _unitOfWork.Persons.Create(person);
            await _unitOfWork.Save();
        }

        public async Task<PersonEditViewModel> GetEditViewModel(Guid id)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFoundException();
            }

            return _mapper.Map<PersonEditViewModel>(person);
        }

        public async Task UpdatePerson(Guid id, PersonEditViewModel viewModel)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFoundException();
            }

            _mapper.Map(viewModel, person);

            _unitOfWork.Persons.Update(person);
            await _unitOfWork.Save();
        }

        public async Task<Person> GetPersonToConfirmDelete(Guid id)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFoundException();
            }

            return person;
        }

        public async Task DeletePerson(Guid id)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFoundException();
            }

            _unitOfWork.Persons.Delete(person);
            await _unitOfWork.Save();
        }
    }
}
