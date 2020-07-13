using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AMS.Data.Models
{
    [Table("genres")]
    public class Genre
    {
        [Key, Column("id"), Required]
        public int Id { get; set; }
        
        [Column("name"), Required, MinLength(2), MaxLength(30)]
        public string Name { get; set; }
        
        [JsonIgnore]
        public IList<MovieGenre> MovieGenres { get; set; }
    }
}
