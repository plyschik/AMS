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
    }
}
