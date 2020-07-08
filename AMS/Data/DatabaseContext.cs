using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Movie> Movies { get; set; }
    }
}
