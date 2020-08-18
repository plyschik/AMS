using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.MVC.Data.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "#")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Title { get; set; }
        
        [StringLength(360, MinimumLength = 2)]
        public string Description { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Release date")]
        public DateTime ReleaseDate { get; set; }
        
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        
        public ICollection<MovieDirector> MovieDirectors { get; set; } = new List<MovieDirector>();

        public ICollection<MovieWriter> MovieWriters { get; set; } = new List<MovieWriter>();
    }
}
