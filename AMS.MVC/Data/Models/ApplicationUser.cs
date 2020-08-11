using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AMS.MVC.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IList<Movie> Movies { get; set; } = new List<Movie>();
    }
}
