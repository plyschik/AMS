using AMS.MVC.Data.Models;
using ReflectionIT.Mvc.Paging;

namespace AMS.MVC.ViewModels.GenreViewModels
{
    public class GenreIndexViewModel
    {
        public PagingList<Genre> Genres { get; set; }
    }
}
