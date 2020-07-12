using System.ComponentModel.DataAnnotations;

namespace AMS.Data.Requests
{
    public class GenreCreateRequest
    {
        [Required, MinLength(2), MaxLength(30)]
        public string Name { get; set; }
    }
}
