using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.MVC.Data.Models
{
    [Table("MovieDirectors")]
    public class MovieDirector
    {
        public Guid MovieId { get; set; }
        
        public Guid PersonId { get; set; }
        
        public Movie Movie { get; set; }
        
        public Person Person { get; set; }
    }
}
