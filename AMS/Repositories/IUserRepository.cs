using System.Threading.Tasks;
using AMS.Data.Models;

namespace AMS.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> IsUserNameAlreadyTaken(string username);

        public Task Create(User user);
    }
}
