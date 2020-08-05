using AMS.MVC.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Data
{
    public class DatabaseContext : IdentityDbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        
        public DbSet<Movie> Movies { get; set; }
        
        public DbSet<Genre> Genres { get; set; }
        
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<ApplicationUser>()
                .HasMany(user => user.Movies)
                .WithOne(movie => movie.User)
                .IsRequired();

            builder.Entity<Genre>()
                .HasIndex(genre => genre.Name)
                .IsUnique();
        }
    }
}
