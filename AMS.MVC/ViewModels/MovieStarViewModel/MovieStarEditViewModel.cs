using System.ComponentModel.DataAnnotations;

namespace AMS.MVC.ViewModels.MovieStarViewModel
{
    public class MovieStarEditViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Character { get; set; }
    }
}
