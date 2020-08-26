using System.Collections.Generic;
using AMS.MVC.Data.Models;

namespace AMS.MVC.ViewModels.GenreViewModels
{
    public class GenreIndexViewModel
    {
        public ICollection<Genre> Genres { get; set; }
    }
}
