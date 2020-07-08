using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Data.Requests
{
    public class MovieUpdateRequest
    {
        [Required, MinLength(2), MaxLength(120)]
        public string Title { get; set; }
        
        [MinLength(2), MaxLength(360)]
        public string Description { get; set; }
        
        public DateTime ReleaseDate { get; set; }
        
        public long Duration { get; set; }
    }
}
