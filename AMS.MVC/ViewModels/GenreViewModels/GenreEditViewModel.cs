using System.ComponentModel.DataAnnotations;
using AMS.MVC.Data.Validation;

namespace AMS.MVC.ViewModels.GenreViewModels
{
    public class GenreEditViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [GenreNameUnique]
        public string Name { get; set; }
    }
}
