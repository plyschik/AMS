using System.Threading.Tasks;
using AMS.Data.Models;

namespace AMS.Repositories
{
    public interface IUserRepository
    {
        public Task Create(User user);

        public Task<User> GetByUserName(string username);
        
        public Task<bool> IsUserNameAlreadyTaken(string username);
    }
}
