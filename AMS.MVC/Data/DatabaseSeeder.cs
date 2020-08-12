using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace AMS.MVC.Data
{
    public static class DatabaseSeeder
    {
        public static async Task Seed(
            DatabaseContext databaseContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager
        )
        {
            await databaseContext.Database.EnsureCreatedAsync();

            if (databaseContext.ApplicationUsers.Any())
            {
                return;
            }

            await roleManager.CreateAsync(new IdentityRole
            {
                Name = "User",
                NormalizedName = "User"
            });
            
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = "Manager",
                NormalizedName = "Manager"
            });
            
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "Administrator"
            });

            var user1 = new ApplicationUser
            {
                Email = "user1@ams.ams",
                UserName = "user1@ams.ams",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(user1, "P4ssword!");
            await userManager.AddToRoleAsync(user1, "User");
            
            var user2 = new ApplicationUser
            {
                Email = "user2@ams.ams",
                UserName = "user2@ams.ams",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(user2, "P4ssword!");
            await userManager.AddToRoleAsync(user2, "User");
            
            var user3 = new ApplicationUser
            {
                Email = "user3@ams.ams",
                UserName = "user3@ams.ams",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(user3, "P4ssword!");
            await userManager.AddToRoleAsync(user3, "User");

            var manager = new ApplicationUser
            {
                Email = "manager@ams.ams",
                UserName = "manager@ams.ams",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(manager, "P4ssword!");
            await userManager.AddToRoleAsync(manager, "Manager");

            var administrator = new ApplicationUser
            {
                Email = "administrator@ams.ams",
                UserName = "administrator@ams.ams",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(administrator, "P4ssword!");
            await userManager.AddToRoleAsync(administrator, "Administrator");

            IDictionary<string, Movie> movies = new Dictionary<string, Movie>
            {
                {
                    "the_shawshank_redemption",
                    new Movie
                    {
                        Title = "The Shawshank Redemption",
                        Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                        ReleaseDate = DateTime.Parse("13-09-1994"),
                        User = user2
                    }
                },
                {
                    "the_godfather",
                    new Movie
                    {
                        Title = "The Godfather",
                        Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                        ReleaseDate = DateTime.Parse("14-03-1972"),
                        User = user3
                    }
                },
                {
                    "the_dark_knight",
                    new Movie
                    {
                        Title = "The Dark Knight",
                        Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                        ReleaseDate = DateTime.Parse("14-07-2008"),
                        User = user3
                    }
                },
                {
                    "the_lord_of_the_rings_the_return_of_the_king",
                    new Movie
                    {
                        Title = "The Lord of the Rings: The Return of the King",
                        Description = "Gandalf and Aragorn lead the World of Men against Sauron's army to draw his gaze from Frodo and Sam as they approach Mount Doom with the One Ring.",
                        ReleaseDate = DateTime.Parse("03-12-2003"),
                        User = user1
                    }
                },
                {
                    "pulp_fiction",
                    new Movie
                    {
                        Title = "Pulp Fiction",
                        Description = "The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.",
                        ReleaseDate = DateTime.Parse("23-09-1994"),
                        User = user1
                    }
                },
                {
                    "12_angry_men",
                    new Movie
                    {
                        Title = "12 Angry Men",
                        Description = "A jury holdout attempts to prevent a miscarriage of justice by forcing his colleagues to reconsider the evidence.",
                        ReleaseDate = DateTime.Parse("10-04-1957"),
                        User = user3
                    }
                },
                {
                    "inception",
                    new Movie
                    {
                        Title = "Inception",
                        Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                        ReleaseDate = DateTime.Parse("13-07-2010"),
                        User = user2
                    }
                },
                {
                    "fight_club",
                    new Movie
                    {
                        Title = "Fight Club",
                        Description = "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much, much more.",
                        ReleaseDate = DateTime.Parse("21-09-1999"),
                        User = user1
                    }
                },
                {
                    "forrest_gump",
                    new Movie
                    {
                        Title = "Forrest Gump",
                        Description = "The presidencies of Kennedy and Johnson, the events of Vietnam, Watergate and other historical events unfold through the perspective of an Alabama man with an IQ of 75, whose only desire is to be reunited with his childhood sweetheart.",
                        ReleaseDate = DateTime.Parse("23-06-1994"),
                        User = user1
                    }
                },
                {
                    "matrix",
                    new Movie
                    {
                        Title = "Matrix",
                        Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                        ReleaseDate = DateTime.Parse("24-03-1999"),
                        User = user2
                    }
                }
            };

            await databaseContext.Movies.AddRangeAsync(movies.Values);
            await databaseContext.SaveChangesAsync();

            IDictionary<string, Genre> genres = new Dictionary<string, Genre>
            {
                {
                    "drama",
                    new Genre
                    {
                        Name = "Drama"
                    }
                },
                {
                    "crime",
                    new Genre
                    {
                        Name = "Crime"
                    }
                },
                {
                    "action",
                    new Genre
                    {
                        Name = "Action"
                    }
                },
                {
                    "adventure",
                    new Genre
                    {
                        Name = "Adventure"
                    }
                },
                {
                    "sci-fi",
                    new Genre
                    {
                        Name = "Sci-Fi"
                    }
                },
                {
                    "romance",
                    new Genre
                    {
                        Name = "Romance"
                    }
                },
            };
            
            await databaseContext.Genres.AddRangeAsync(genres.Values);
            await databaseContext.SaveChangesAsync();

            var movieGenres = new[]
            {
                new MovieGenre
                {
                    Movie = movies["the_shawshank_redemption"],
                    Genre = genres["drama"]
                },
                new MovieGenre
                {
                    Movie = movies["the_godfather"],
                    Genre = genres["crime"]
                },
                new MovieGenre
                {
                    Movie = movies["the_godfather"],
                    Genre = genres["drama"]
                },
                new MovieGenre
                {
                    Movie = movies["the_dark_knight"],
                    Genre = genres["action"]
                },
                new MovieGenre
                {
                    Movie = movies["the_dark_knight"],
                    Genre = genres["crime"]
                },
                new MovieGenre
                {
                    Movie = movies["the_dark_knight"],
                    Genre = genres["drama"]
                },
                new MovieGenre
                {
                    Movie = movies["the_lord_of_the_rings_the_return_of_the_king"],
                    Genre = genres["action"]
                },
                new MovieGenre
                {
                    Movie = movies["the_lord_of_the_rings_the_return_of_the_king"],
                    Genre = genres["adventure"]
                },
                new MovieGenre
                {
                    Movie = movies["the_lord_of_the_rings_the_return_of_the_king"],
                    Genre = genres["drama"]
                },
                new MovieGenre
                {
                    Movie = movies["pulp_fiction"],
                    Genre = genres["crime"]
                },
                new MovieGenre
                {
                    Movie = movies["pulp_fiction"],
                    Genre = genres["drama"]
                },
                new MovieGenre
                {
                    Movie = movies["12_angry_men"],
                    Genre = genres["crime"]
                },
                new MovieGenre
                {
                    Movie = movies["12_angry_men"],
                    Genre = genres["drama"]
                },
                new MovieGenre
                {
                    Movie = movies["inception"],
                    Genre = genres["action"]
                },
                new MovieGenre
                {
                    Movie = movies["inception"],
                    Genre = genres["adventure"]
                },
                new MovieGenre
                {
                    Movie = movies["inception"],
                    Genre = genres["sci-fi"]
                },
                new MovieGenre
                {
                    Movie = movies["fight_club"],
                    Genre = genres["drama"]
                },
                new MovieGenre
                {
                    Movie = movies["forrest_gump"],
                    Genre = genres["drama"]
                },
                new MovieGenre
                {
                    Movie = movies["forrest_gump"],
                    Genre = genres["romance"]
                },
                new MovieGenre
                {
                    Movie = movies["matrix"],
                    Genre = genres["action"]
                },
                new MovieGenre
                {
                    Movie = movies["matrix"],
                    Genre = genres["sci-fi"]
                }
            };

            await databaseContext.MovieGenres.AddRangeAsync(movieGenres);
            await databaseContext.SaveChangesAsync();

            IDictionary<string, Person> persons = new Dictionary<string, Person>
            {
                {
                    "morgan_freeman",
                    new Person
                    {
                        FirstName = "Morgan",
                        LastName = "Freeman",
                        DateOfBirth = DateTime.Parse("01-06-1937")
                    }
                },
                {
                    "tim_robbins",
                    new Person
                    {
                        FirstName = "Tim",
                        LastName = "Robbins",
                        DateOfBirth = DateTime.Parse("16-10-1958")
                    }
                },
                {
                    "al_pacino",
                    new Person
                    {
                        FirstName = "Al",
                        LastName = "Pacino",
                        DateOfBirth = DateTime.Parse("25-04-1940")
                    }
                },
                {
                    "james_caan",
                    new Person
                    {
                        FirstName = "James",
                        LastName = "Caan",
                        DateOfBirth = DateTime.Parse("26-03-1940")
                    }
                },
                {
                    "christian_bale",
                    new Person
                    {
                        FirstName = "Christian",
                        LastName = "Bale",
                        DateOfBirth = DateTime.Parse("30-01-1974")
                    }
                },
                {
                    "michael_caine",
                    new Person
                    {
                        FirstName = "Michael",
                        LastName = "Caine",
                        DateOfBirth = DateTime.Parse("14-03-1933")
                    }
                },
                {
                    "maggie_gyllenhaal",
                    new Person
                    {
                        FirstName = "Maggie",
                        LastName = "Gyllenhaal",
                        DateOfBirth = DateTime.Parse("16-11-1977")
                    }
                },
                {
                    "ali_astin",
                    new Person
                    {
                        FirstName = "Ali",
                        LastName = "Astin",
                        DateOfBirth = DateTime.Parse("27-11-1996")
                    }
                },
                {
                    "orlando_bloom",
                    new Person
                    {
                        FirstName = "Orlando",
                        LastName = "Bloom",
                        DateOfBirth = DateTime.Parse("13-01-1977")
                    }
                },
                {
                    "tom_hanks",
                    new Person
                    {
                        FirstName = "Tom",
                        LastName = "Hanks",
                        DateOfBirth = DateTime.Parse("09-07-1956")
                    }
                }
            };
            
            await databaseContext.Persons.AddRangeAsync(persons.Values);
            await databaseContext.SaveChangesAsync();
        }
    }
}
