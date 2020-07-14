using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repositories
{
    public interface IPersonRepository
    {
        public Task<IEnumerable<Person>> GetAll();

        public Task<Person> GetById(int id);
    }
    
    public class PersonRepository : IPersonRepository
    {
        private readonly DatabaseContext _databaseContext;

        public PersonRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _databaseContext.Persons.ToListAsync();
        }

        public async Task<Person> GetById(int id)
        {
            return await _databaseContext.Persons.FirstOrDefaultAsync(person => person.Id == id);
        }
    }
}
