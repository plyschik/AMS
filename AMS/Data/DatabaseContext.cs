using AMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
     
        public DbSet<Movie> Movies { get; set; }
        
        public DbSet<Genre> Genres { get; set; }
        
        public DbSet<MovieGenre> MovieGenres { get; set; }
        
        public DbSet<Person> Persons { get; set; }
        
        public DbSet<MoviePersonDirector> MoviePersonDirectors { get; set; }
        
        public DbSet<MoviePersonWriter> MoviePersonWriters { get; set; }
        
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

            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne<Movie>(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);
            
            modelBuilder.Entity<MovieGenre>()
                .HasOne<Genre>(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId);

            modelBuilder.Entity<Person>()
                .HasIndex(person => new { person.FirstName, person.LastName })
                .IsUnique();

            modelBuilder.Entity<MoviePersonDirector>()
                .HasKey(mpd => new { mpd.MovieId, mpd.PersonId });

            modelBuilder.Entity<MoviePersonDirector>()
                .HasOne<Movie>(mpd => mpd.Movie)
                .WithMany(m => m.MoviePersonDirectors)
                .HasForeignKey(mpd => mpd.MovieId);

            modelBuilder.Entity<MoviePersonDirector>()
                .HasOne<Person>(mpd => mpd.Person)
                .WithMany(p => p.MoviePersonDirectors)
                .HasForeignKey(mpd => mpd.PersonId);

            modelBuilder.Entity<MoviePersonWriter>()
                .HasKey(mpw => new { mpw.MovieId, mpw.PersonId });

            modelBuilder.Entity<MoviePersonWriter>()
                .HasOne<Movie>(mpw => mpw.Movie)
                .WithMany(m => m.MoviePersonWriters)
                .HasForeignKey(mpw => mpw.MovieId);
            
            modelBuilder.Entity<MoviePersonWriter>()
                .HasOne<Person>(mpw => mpw.Person)
                .WithMany(p => p.MoviePersonWriters)
                .HasForeignKey(mpw => mpw.PersonId);
        }
    }
}
