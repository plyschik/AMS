using System.Threading.Tasks;
using AMS.Data.Models;
using AMS.Data.Requests;
using AMS.Data.Responses;
using AMS.Exceptions;
using AMS.Repositories;

namespace AMS.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IJwtTokenService _jwtTokenService;

        public UserService(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task SignUp(SignUpRequest request)
        {
            if (await _userRepository.IsUserNameAlreadyTaken(request.Username))
            {
                throw new UserNameAlreadyTaken("Username is already taken!");
            }

            PasswordService.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);
            
            await _userRepository.Create(new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            });
        }

        public async Task<SignInResponse> SignIn(SignInRequest request)
        {
            var user = await _userRepository.GetByUserName(request.Username);

            if (user == null)
            {
                throw new WrongCredentials("Wrong credentials!");
            }

            if (!PasswordService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                 throw new WrongCredentials("Wrong credentials!");
            }

            var token = _jwtTokenService.GenerateJwtToken(user);

            return new SignInResponse()
            {
                Token = token
            };
        }
    }
}
