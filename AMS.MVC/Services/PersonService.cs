using System;
using System.Threading.Tasks;
using AMS.MVC.Data.Models;
using AMS.MVC.Exceptions.Person;
using AMS.MVC.Repositories;
using AMS.MVC.ViewModels.PersonViewModels;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Services
{
    public interface IPersonService
    {
        public Task<PersonIndexViewModel> GetPersonsList();

        public Task<PersonShowViewModel> GetPerson(Guid id);
        
        public Task CreatePerson(PersonCreateViewModel viewModel);

        public Task<PersonEditViewModel> GetPersonEdit(Guid id);

        public Task PersonEdit(Guid id, PersonEditViewModel viewModel);
        
        public Task<Person> GetPersonToConfirmDelete(Guid id);

        public Task DeletePerson(Guid id);
    }
    
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PersonIndexViewModel> GetPersonsList()
        {
            var persons = _unitOfWork.Persons.GetAllOrderedByLastNameAscending();

            return new PersonIndexViewModel
            {
                Persons = await persons.ToListAsync()
            };
        }

        public async Task<PersonShowViewModel> GetPerson(Guid id)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFound();
            }

            return new PersonShowViewModel
            {
                Person = person,
                MovieDirector = await _unitOfWork.Movies.GetMoviesWherePersonIsDirectorOrderedByReleaseDate(person.Id),
                MovieWriter = await _unitOfWork.Movies.GetMoviesWherePersonIsWriterOrderedByReleaseDate(person.Id),
                MovieStar = await _unitOfWork.Movies.GetMoviesWherePersonIsStarOrderedByReleaseDate(person.Id)
            };
        }

        public async Task CreatePerson(PersonCreateViewModel viewModel)
        {
            await _unitOfWork.Persons.Create(new Person
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth
            });
            await _unitOfWork.Save();
        }

        public async Task<PersonEditViewModel> GetPersonEdit(Guid id)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFound();
            }

            return new PersonEditViewModel
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth
            };
        }

        public async Task PersonEdit(Guid id, PersonEditViewModel viewModel)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFound();
            }

            person.FirstName = viewModel.FirstName;
            person.LastName = viewModel.LastName;
            person.DateOfBirth = viewModel.DateOfBirth;
                
            _unitOfWork.Persons.Update(person);
            await _unitOfWork.Save();
        }

        public async Task<Person> GetPersonToConfirmDelete(Guid id)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFound();
            }

            return person;
        }

        public async Task DeletePerson(Guid id)
        {
            var person = await _unitOfWork.Persons.GetBy(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFound();
            }

            _unitOfWork.Persons.Delete(person);
            await _unitOfWork.Save();
        }
    }
}
