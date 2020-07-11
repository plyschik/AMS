using System;
using System.Threading.Tasks;
using AMS.Data.Requests;
using AMS.Services;
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
    }
}
