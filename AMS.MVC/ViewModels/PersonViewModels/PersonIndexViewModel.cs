using System.Collections.Generic;
using AMS.MVC.Data.Models;

namespace AMS.MVC.ViewModels.PersonViewModels
{
    public class PersonIndexViewModel
    {
        public ICollection<Person> Persons { get; set; }
    }
}
