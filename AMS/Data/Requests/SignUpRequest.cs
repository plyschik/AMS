using System.ComponentModel.DataAnnotations;

namespace AMS.Data.Requests
{
    public class SignUpRequest
    {
        [Required, MinLength(2), MaxLength(30)]
        public string UserName { get; set; }
        
        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}
