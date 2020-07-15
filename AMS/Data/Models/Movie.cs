using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

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
        
        [JsonIgnore]
        public IList<MovieGenre> MovieGenres { get; set; }
        
        [JsonIgnore]
        public IList<MoviePersonDirector> MoviePersonDirectors { get; set; }

        [NotMapped]
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public IEnumerable<Genre> Genres => MovieGenres.Select(mg => mg.Genre);
        
        [NotMapped]
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public IEnumerable<Person> Directors => MoviePersonDirectors.Select(mpd => mpd.Person);
    }
}
