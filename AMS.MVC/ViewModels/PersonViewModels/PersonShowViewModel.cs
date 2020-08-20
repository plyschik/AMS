using System.Collections.Generic;
using AMS.MVC.Data.Models;

namespace AMS.MVC.ViewModels.PersonViewModels
{
    public class PersonShowViewModel
    {
        public Person Person { get; set; }

        public IEnumerable<Movie> MovieWriter { get; set; }
        
        public IEnumerable<Movie> MovieDirector { get; set; }
        
        public IEnumerable<Movie> MovieStar { get; set; }
    }
}
