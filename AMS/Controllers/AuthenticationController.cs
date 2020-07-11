using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AMS.Data.Requests;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            try
            {
                await _userService.SignUp(request);
                
                return Created("", new
                {
                    Message = "Account successfully created."
                });
            }
            catch (Exception exception)
            {
                return BadRequest(new
                {
                    exception.Message
                });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            try
            {
                var response = await _userService.SignIn(request);

                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var username = User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
            var user = await _userService.Me(username);
            
            return Ok(user);
        }
    }
}
