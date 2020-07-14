using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IPersonService
    {
        public Task<IEnumerable<PersonResponse>> GetAll();

        public Task<PersonResponse> GetById(int id);
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
    }
}
