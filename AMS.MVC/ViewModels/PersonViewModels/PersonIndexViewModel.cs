using AMS.MVC.Data.Models;
using AMS.MVC.Helpers;

namespace AMS.MVC.ViewModels.PersonViewModels
{
    public class PersonIndexViewModel
    {
        public Paginator<Person> Paginator { get; set; }
    }
}
