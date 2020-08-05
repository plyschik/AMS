using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Name { get; set; }
    }
}
