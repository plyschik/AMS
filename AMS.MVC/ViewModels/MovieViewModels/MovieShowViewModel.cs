using System.Collections.Generic;
using AMS.MVC.Data.Models;

namespace AMS.MVC.ViewModels.MovieViewModels
{
    public class MovieShowViewModel
    {
        public Movie Movie { get; set; }
        
        public ICollection<Genre> Genres { get; set; }
        
        public ICollection<Person> Directors { get; set; }
        
        public ICollection<Person> Writers { get; set; }
        
        public ICollection<MovieStar> Stars { get; set; }
    }
}
