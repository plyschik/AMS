using System.Threading.Tasks;
using AMS.Data.Requests;
using AMS.Data.Responses;

namespace AMS.Services
{
    public interface IUserService
    {
        public Task SignUp(SignUpRequest request);

        public Task<SignInResponse> SignIn(SignInRequest request);
    }
}
