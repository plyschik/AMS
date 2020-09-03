using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.MVC.Data.Models
{
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "GUID")]
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        
        public ICollection<MovieDirector> MovieDirectors { get; set; } = new List<MovieDirector>();
        
        public ICollection<MovieWriter> MovieWriters { get; set; } = new List<MovieWriter>();

        public ICollection<MovieStar> MovieStars { get; set; } = new List<MovieStar>();
    }
}
