using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AMS.MVC.Data.Validation;

namespace AMS.MVC.Data.Models
{
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "#")]
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [GenreNameUnique]
        public string Name { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
