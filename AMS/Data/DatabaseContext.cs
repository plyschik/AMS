using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
     
        public DbSet<Movie> Movies { get; set; }
        
        public DbSet<Genre> Genres { get; set; }
        
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Username)
                .IsUnique();

            modelBuilder.Entity<Genre>()
                .HasIndex(genre => genre.Name)
                .IsUnique();
        }
    }
}
