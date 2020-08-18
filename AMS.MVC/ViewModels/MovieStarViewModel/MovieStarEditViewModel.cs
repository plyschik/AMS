using System.ComponentModel.DataAnnotations;

namespace AMS.MVC.ViewModels.MovieStarViewModel
{
    public class MovieStatEditViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Character { get; set; }
    }
}
