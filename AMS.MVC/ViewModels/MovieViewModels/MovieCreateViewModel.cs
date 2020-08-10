using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AMS.MVC.ViewModels.MovieViewModels
{
    public class MovieCreateViewModel
    {
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Title { get; set; }
        
        [StringLength(360, MinimumLength = 2)]
        public string Description { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Release date")]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        public IList<SelectListItem> Genres { get; set; } = new List<SelectListItem>();

        public string[] SelectedGenres { get; set; } = {};
    }
}
