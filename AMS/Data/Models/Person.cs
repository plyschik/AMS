using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AMS.Data.Models
{
    [Table("persons")]
    public class Person
    {
        [Key, Column("id"), Required]
        public int Id { get; set; }
        
        [Column("first_name"), Required, MinLength(2), MaxLength(30)]
        public string FirstName { get; set; }
        
        [Column("last_name"), Required, MinLength(2), MaxLength(30)]
        public string LastName { get; set; }
        
        [Column("date_of_birth", TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        
        [JsonIgnore]
        public IList<MoviePersonDirector> MoviePersonDirectors { get; set; }
        
        [JsonIgnore]
        public IList<MoviePersonWriter> MoviePersonWriters { get; set; }
        
        [JsonIgnore]
        public IList<MoviePersonStar> MoviePersonStars { get; set; }
    }
}
