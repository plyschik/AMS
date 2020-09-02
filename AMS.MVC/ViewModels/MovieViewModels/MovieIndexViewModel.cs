using AMS.MVC.Data.Models;
using AMS.MVC.Helpers;

namespace AMS.MVC.ViewModels.MovieViewModels
{
    public class MovieIndexViewModel
    {
        public Paginator<Movie> Paginator { get; set; }
    }
}
