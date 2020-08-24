using System.Collections.Generic;
using AMS.MVC.Data.Models;

namespace AMS.MVC.ViewModels.MovieViewModels
{
    public class MovieIndexViewModel
    {
        public ICollection<Movie> Movies { get; set; }
    }
}
