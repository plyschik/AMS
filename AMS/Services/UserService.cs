using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Requests;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AMS.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task SignUp(SignUpRequest request)
        {
            if (await _userRepository.IsUserNameAlreadyTaken(request.UserName))
            {
                throw new UserNameAlreadyTaken("Username is already taken!");
            }

            PasswordService.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);
            
            await _userRepository.Create(new User
            {
                UserName = request.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            });
        }

        public async Task<SignInResponse> SignIn(SignInRequest request)
        {
            var user = await _userRepository.GetByUserName(request.UserName);

            if (user == null)
            {
                throw new WrongCredentials("Wrong credentials!");
            }

            if (!PasswordService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                 throw new WrongCredentials("Wrong credentials!");
            }

            return new SignInResponse()
            {
                Token = "Token"
            };
        }
    }
}
