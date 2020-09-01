using AMS.MVC.Data.Models;
using ReflectionIT.Mvc.Paging;

namespace AMS.MVC.ViewModels.PersonViewModels
{
    public class PersonIndexViewModel
    {
        public PagingList<Person> Persons { get; set; }
    }
}
