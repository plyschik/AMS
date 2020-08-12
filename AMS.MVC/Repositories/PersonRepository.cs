using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Repositories
{
    public interface IPersonRepository
    {
        public Task<ICollection<Person>> GetAll();
        
        public Task<Person> GetById(Guid id);
        
        public void Create(Person person);

        public void Update(Person person);

        public void Delete(Person person);

        public Task<bool> Exists(Guid id);
    }
    
    public class PersonRepository : IPersonRepository
    {
        private readonly DatabaseContext _databaseContext;

        public PersonRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ICollection<Person>> GetAll()
        {
            return await _databaseContext.Persons.ToListAsync();
        }

        public async Task<Person> GetById(Guid id)
        {
            return await _databaseContext.Persons.FirstOrDefaultAsync(person => person.Id == id);
        }

        public void Create(Person person)
        {
            _databaseContext.Persons.Add(person);
        }

        public void Update(Person person)
        {
            _databaseContext.Persons.Update(person);
        }

        public void Delete(Person person)
        {
            _databaseContext.Persons.Remove(person);
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _databaseContext.Persons.AnyAsync(person => person.Id == id);
        }
    }
}
