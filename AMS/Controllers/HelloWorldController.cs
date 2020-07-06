using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class HelloWorldController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Ok(new
            {
                Message = "Hello, world!"
            });
        }
    }
}
