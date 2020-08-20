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
        
        public DbSet<MovieGenre> MovieGenres { get; set; }
        
        public DbSet<Person> Persons { get; set; }
        
        public DbSet<MovieDirector> MovieDirectors { get; set; }
        
        public DbSet<MovieWriter> MovieWriters { get; set; }
        
        public DbSet<MovieStar> MovieStars { get; set; }
        
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany(au => au.Movies)
                .WithOne(m => m.User)
                .IsRequired();

            builder.Entity<Genre>()
                .HasIndex(g => g.Name)
                .IsUnique();

            builder.Entity<MovieGenre>(entity =>
            {
                entity.HasKey(mg => new { mg.MovieId, mg.GenreId });

                entity.HasOne(mg => mg.Movie)
                    .WithMany(m => m.MovieGenres)
                    .HasForeignKey(mg => mg.MovieId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(mg => mg.Genre)
                    .WithMany(g => g.MovieGenres)
                    .HasForeignKey(mg => mg.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            builder.Entity<Person>()
                .HasIndex(p => new { p.FirstName, p.LastName })
                .IsUnique();

            builder.Entity<MovieDirector>(entity =>
            {
                entity.HasKey(md => new { md.MovieId, md.PersonId });
                
                entity.HasOne(md => md.Movie)
                    .WithMany(m => m.MovieDirectors)
                    .HasForeignKey(md => md.MovieId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(md => md.Person)
                    .WithMany(p => p.MovieDirectors)
                    .HasForeignKey(md => md.PersonId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MovieWriter>(entity =>
            {
                entity.HasKey(mw => new { mw.MovieId, mw.PersonId });
                
                entity.HasOne(mw => mw.Movie)
                    .WithMany(m => m.MovieWriters)
                    .HasForeignKey(mw => mw.MovieId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(mw => mw.Person)
                    .WithMany(p => p.MovieWriters)
                    .HasForeignKey(mw => mw.PersonId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MovieStar>(entity =>
            {
                entity.HasKey(ms => new { ms.MovieId, ms.PersonId });
                
                entity.HasOne(ms => ms.Movie)
                    .WithMany(m => m.MovieStars)
                    .HasForeignKey(ms => ms.MovieId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(ms => ms.Person)
                    .WithMany(p => p.MovieStars)
                    .HasForeignKey(ms => ms.PersonId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
