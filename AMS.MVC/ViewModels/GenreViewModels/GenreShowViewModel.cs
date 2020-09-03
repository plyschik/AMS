using AMS.MVC.Data.Models;
using AMS.MVC.Helpers;

namespace AMS.MVC.ViewModels.GenreViewModels
{
    public class GenreShowViewModel
    {
        public Genre Genre { get; set; }
        
        public Paginator<Movie> Paginator { get; set; }
    }
}
