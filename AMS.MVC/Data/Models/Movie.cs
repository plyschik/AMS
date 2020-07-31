using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.MVC.Data.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Title { get; set; }
        
        [StringLength(360, MinimumLength = 2)]
        public string Description { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        
        public ApplicationUser User { get; set; }
    }
}
