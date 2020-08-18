using System;

namespace AMS.MVC.Data.Models
{
    public class MovieWriter
    {
        public Guid MovieId { get; set; }
        
        public Guid PersonId { get; set; }
        
        public Movie Movie { get; set; }
        
        public Person Person { get; set; }
    }
}
