using AMS.MVC.Data.Models;
using AMS.MVC.Helpers;

namespace AMS.MVC.ViewModels.GenreViewModels
{
    public class GenreIndexViewModel
    {
        public Paginator<Genre> Paginator { get; set; }
    }
}
