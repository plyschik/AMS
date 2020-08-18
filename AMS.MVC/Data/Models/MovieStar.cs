using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.MVC.Data.Models
{
    public class MovieStar
    {
        public Guid MovieId { get; set; }
        
        public Guid PersonId { get; set; }
        
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Character { get; set; }

        public Movie Movie { get; set; }
        
        public Person Person { get; set; }
    }
}
