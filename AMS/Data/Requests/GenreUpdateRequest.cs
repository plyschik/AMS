using System.ComponentModel.DataAnnotations;

namespace AMS.Data.Requests
{
    public class GenreUpdateRequest
    {
        [Required, MinLength(2), MaxLength(30)]
        public string Name { get; set; }
    }
}
