using System.Threading.Tasks;
using AMS.Data.Requests;

namespace AMS.Services
{
    public interface IUserService
    {
        public Task SignUp(SignUpRequest request);
    }
}