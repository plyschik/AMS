using AMS.MVC.Data.Models;
using ReflectionIT.Mvc.Paging;

namespace AMS.MVC.ViewModels.MovieViewModels
{
    public class MovieIndexViewModel
    {
        public PagingList<Movie> Movies { get; set; }
    }
}
