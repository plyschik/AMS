using System.Linq;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;

namespace AMS.MVC.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        public IQueryable<Person> GetAllOrderedBy(string search, string sort, string order);
    }
    
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IQueryable<Person> GetAllOrderedBy(string search, string sort, string order)
        {
            IQueryable<Person> queryable = DatabaseContext.Persons;

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                
                queryable = queryable.Where(p => 
                    p.FirstName.ToLower().Contains(search) || p.LastName.ToLower().Contains(search)
                );
            }
            
            switch (sort)
            {
                case "first_name":
                    queryable = order == "asc"
                        ? queryable.OrderBy(p => p.FirstName)
                        : queryable.OrderByDescending(p => p.FirstName);
                    
                    break;
                case "last_name":
                    queryable = order == "asc"
                        ? queryable.OrderBy(p => p.LastName)
                        : queryable.OrderByDescending(p => p.LastName);
                    
                    break;
                case "date_of_birth":
                    queryable = order == "asc"
                        ? queryable.OrderBy(p => p.DateOfBirth)
                        : queryable.OrderByDescending(p => p.DateOfBirth);
                    
                    break;
                default:
                    queryable = queryable.OrderBy(m => m.LastName);
                    
                    break;
            }

            return queryable;
        }
    }
}
