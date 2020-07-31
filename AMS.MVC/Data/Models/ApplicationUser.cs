using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AMS.MVC.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Movie> Movies { get; set; }
    }
}
