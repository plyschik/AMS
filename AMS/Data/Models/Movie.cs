using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Data.Models
{
    [Table("movies")]
    public class Movie
    {
        [Key, Column("id"), Required]
        public int Id { get; set; }
        
        [Column("title"), Required, MinLength(2), MaxLength(120)]
        public string Title { get; set; }
        
        [Column("description"), MinLength(2), MaxLength(360)]
        public string Description { get; set; }
        
        [Column("release_date")]
        public DateTime ReleaseDate { get; set; }
        
        [Column("duration")]
        public long Duration { get; set; }
    }
}
