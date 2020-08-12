using System.Threading.Tasks;
using AMS.MVC.Data;
using Microsoft.AspNetCore.Mvc;

namespace AMS.MVC.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var persons = await _unitOfWork.PersonRepository.GetAll();
            
            return View(persons);
        }
    }
}
