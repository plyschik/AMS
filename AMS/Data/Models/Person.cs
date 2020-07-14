using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
