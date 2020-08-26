using System.Linq;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;

namespace AMS.MVC.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        public IQueryable<Person> GetAllOrderedByLastNameAscending();
    }
    
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IQueryable<Person> GetAllOrderedByLastNameAscending()
        {
            return DatabaseContext.Persons
                .OrderBy(p => p.LastName);
        }
    }
}
