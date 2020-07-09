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
        
        [Column("description"), Required, MinLength(2), MaxLength(360)]
        public string Description { get; set; }
        
        [Column("release_date"), Required, DataType(DataType.DateTime)]
        public DateTime? ReleaseDate { get; set; }
        
        [Column("duration"), Required, Range(0, long.MaxValue)]
        public long? Duration { get; set; }
    }
}
