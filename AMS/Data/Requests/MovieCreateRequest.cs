using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Data.Requests
{
    public class MovieCreateRequest
    {
        [Required, MinLength(2), MaxLength(120)]
        public string Title { get; set; }
        
        [Required, MinLength(2), MaxLength(360)]
        public string Description { get; set; }
        
        [Required, DataType(DataType.DateTime)]
        public DateTime? ReleaseDate { get; set; }
        
        [Required, Range(0, long.MaxValue)]
        public long? Duration { get; set; }
    }
}
