using System.Collections.Generic;
using AMS.MVC.Data.Models;

namespace AMS.MVC.ViewModels.GenreViewModels
{
    public class GenreShowViewModel
    {
        public Genre Genre { get; set; }
        
        public IEnumerable<Movie> Movies { get; set; }
    }
}
