using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace AMS.MVC.Controllers
{
    public class ErrorController : Controller
    {
        [Route("[controller]/{code}")]
        public IActionResult Index(int code)
        {
            ViewData["code"] = code;
            ViewData["description"] = ReasonPhrases.GetReasonPhrase(code);

            return View();
        }
    }
}
