using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data.Responses;
using AMS.Repositories;
using AutoMapper;

namespace AMS.Services
{
    public interface IPersonService
    {
        public Task<IEnumerable<PersonResponse>> GetAll();
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
    }
}
