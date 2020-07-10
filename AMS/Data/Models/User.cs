using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Data.Models
{
    [Table("users")]
    public class User
    {
        [Key, Column("id"), Required]
        public int Id { get; set; }
        
        [Column("username"), Required, MinLength(2), MaxLength(30)]
        public string UserName { get; set; }

        [Column("password_hash"), Required]
        public byte[] PasswordHash { get; set; }
        
        [Column("password_salt"), Required]
        public byte[] PasswordSalt { get; set; }
    }
}
