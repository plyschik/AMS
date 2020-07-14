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
    public interface IPersonService
    {
        public Task<IEnumerable<PersonResponse>> GetAll();

        public Task<PersonResponse> GetById(int id);

        public Task<PersonResponse> Create(PersonCreateRequest request);

        public Task<PersonResponse> Update(int id, PersonUpdateRequest request);

        public Task<Person> GetPerson(int id);

        public PersonUpdateRequest MergePersonModelWithPatchDocument(
            Person person,
            JsonPatchDocument<PersonUpdateRequest> document
        );
        
        public Task<PersonResponse> UpdatePartial(PersonUpdateRequest personToPatch, Person personFromDatabase);
    }
    
    public class PersonService : IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;

        public PersonService(IMapper mapper, IPersonRepository personRepository)
        {
            _mapper = mapper;
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<PersonResponse>> GetAll()
        {
            var persons = await _personRepository.GetAll();

            return _mapper.Map<IEnumerable<PersonResponse>>(persons);
        }

        public async Task<PersonResponse> GetById(int id)
        {
            var person = await _personRepository.GetById(id);

            if (person == null)
            {
                throw new PersonNotFound("Person not found!");
            }

            return _mapper.Map<PersonResponse>(person);
        }

        public async Task<PersonResponse> Create(PersonCreateRequest request)
        {
            if (await _personRepository.IsPersonExists(request.FirstName, request.LastName))
            {
                throw new PersonAlreadyExists("This person already exists!");
            }
            
            var person = await _personRepository.Create(_mapper.Map<Person>(request));

            return _mapper.Map<PersonResponse>(person);
        }

        public async Task<PersonResponse> Update(int id, PersonUpdateRequest request)
        {
            var personToUpdate = await _personRepository.GetById(id);

            if (personToUpdate == null)
            {
                throw new PersonNotFound("Person not found!");
            }

            if (await _personRepository.IsPersonExists(request.FirstName, request.LastName))
            {
                throw new PersonAlreadyExists("This person already exists!");
            }

            _mapper.Map(request, personToUpdate);

            var person = await _personRepository.Update(personToUpdate);

            return _mapper.Map<PersonResponse>(person);
        }

        public async Task<Person> GetPerson(int id)
        {
            var person = await _personRepository.GetById(id);

            if (person == null)
            {
                throw new PersonNotFound("Person not found!");
            }

            return person;
        }
        
        public PersonUpdateRequest MergePersonModelWithPatchDocument(Person person, JsonPatchDocument<PersonUpdateRequest> document)
        {
            var personUpdateRequest = _mapper.Map<PersonUpdateRequest>(person);
            document.ApplyTo(personUpdateRequest);

            return personUpdateRequest;
        }
        
        public async Task<PersonResponse> UpdatePartial(PersonUpdateRequest personToPatch, Person personFromDatabase)
        {
            if (await _personRepository.IsPersonExists(personToPatch.FirstName, personToPatch.LastName))
            {
                throw new PersonAlreadyExists("Person already exists!");
            }
            
            _mapper.Map(personToPatch, personFromDatabase);
            
            await _personRepository.Update(personFromDatabase);
            
            return _mapper.Map<PersonResponse>(personFromDatabase);
        }
    }
}
