using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class HelloWorldController : Controller
    {
        [HttpGet("/api/helloworld")]
        public IActionResult Index()
        {
            return Ok(new
            {
                Message = "Hello, world!"
            });
        }
    }
}
