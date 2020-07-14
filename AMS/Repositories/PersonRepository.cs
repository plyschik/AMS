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

        public Task<Person> Create(Person person);

        public Task<Person> Update(Person person);

        public Task<bool> IsPersonExists(int id);
        
        public Task<bool> IsPersonExists(string firstName, string lastName);
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

        public async Task<Person> Create(Person person)
        {
            await _databaseContext.Persons.AddAsync(person);
            await _databaseContext.SaveChangesAsync();

            return person;
        }

        public async Task<Person> Update(Person person)
        {
            _databaseContext.Persons.Update(person);
            await _databaseContext.SaveChangesAsync();

            return person;
        }

        public async Task<bool> IsPersonExists(int id)
        {
            return await _databaseContext.Persons.AnyAsync(person => person.Id.Equals(id));
        }

        public async Task<bool> IsPersonExists(string firstName, string lastName)
        {
            return await _databaseContext.Persons.AnyAsync(
                person => person.FirstName.Equals(firstName) && person.LastName.Equals(lastName) 
            );
        }
    }
}
