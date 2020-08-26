using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AMS.MVC.ViewModels.MovieStarViewModel
{
    public class MovieStarCreateViewModel
    {
        public IList<SelectListItem> Persons { get; set; } = new List<SelectListItem>();
        
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Character { get; set; }
        
        [Display(Name = "Person")]
        public string SelectedPerson { get; set; }
    }
}
