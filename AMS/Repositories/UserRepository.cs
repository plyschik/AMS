using System.Threading.Tasks;
using AMS.Data;
using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repositories
{
    public interface IUserRepository
    {
        public Task Create(User user);

        public Task<User> GetByUserName(string username);
        
        public Task<bool> IsUserNameAlreadyTaken(string username);
    }
    
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _databaseContext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task Create(User user)
        {
            await _databaseContext.Users.AddAsync(user);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<User> GetByUserName(string username)
        {
            return await _databaseContext.Users.FirstOrDefaultAsync(user => user.Username == username);
        }

        public async Task<bool> IsUserNameAlreadyTaken(string username)
        {
            return await _databaseContext.Users.AnyAsync(
                user => user.Username.ToLower().Contains(username.ToLower())
            );
        }
    }
}
