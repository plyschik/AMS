using System;
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

            if (databaseContext.ApplicationUsers.Any() || databaseContext.Movies.Any())
            {
                return;
            }

            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "User",
                NormalizedName = "User"
            });
            
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Manager",
                NormalizedName = "Manager"
            });
            
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Administrator",
                NormalizedName = "Administrator"
            });

            var user1 = new ApplicationUser()
            {
                Email = "user1@ams.ams",
                UserName = "user1@ams.ams",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(user1, "P4ssword!");
            await userManager.AddToRoleAsync(user1, "User");
            
            var user2 = new ApplicationUser()
            {
                Email = "user2@ams.ams",
                UserName = "user2@ams.ams",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(user2, "P4ssword!");
            await userManager.AddToRoleAsync(user2, "User");
            
            var user3 = new ApplicationUser()
            {
                Email = "user3@ams.ams",
                UserName = "user3@ams.ams",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(user3, "P4ssword!");
            await userManager.AddToRoleAsync(user3, "User");

            var manager = new ApplicationUser()
            {
                Email = "manager@ams.ams",
                UserName = "manager@ams.ams",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(manager, "P4ssword!");
            await userManager.AddToRoleAsync(manager, "Manager");

            var administrator = new ApplicationUser()
            {
                Email = "administrator@ams.ams",
                UserName = "administrator@ams.ams",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(administrator, "P4ssword!");
            await userManager.AddToRoleAsync(administrator, "Administrator");

            var movies = new[]
            {
                new Movie()
                {
                    Title = "The Shawshank Redemption",
                    Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    ReleaseDate = DateTime.Parse("13-09-1994"),
                    User = user2
                },
                new Movie()
                {
                    Title = "The Godfather",
                    Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                    ReleaseDate = DateTime.Parse("14-03-1972"),
                    User = user3
                },
                new Movie()
                {
                    Title = "The Dark Knight",
                    Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                    ReleaseDate = DateTime.Parse("14-07-2008"),
                    User = user3
                },
                new Movie()
                {
                    Title = "The Lord of the Rings: The Return of the King",
                    Description = "Gandalf and Aragorn lead the World of Men against Sauron's army to draw his gaze from Frodo and Sam as they approach Mount Doom with the One Ring.",
                    ReleaseDate = DateTime.Parse("03-12-2003"),
                    User = user1
                },
                new Movie()
                {
                    Title = "Pulp Fiction",
                    Description = "The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.",
                    ReleaseDate = DateTime.Parse("23-09-1994"),
                    User = user1
                },
                new Movie()
                {
                    Title = "12 Angry Men",
                    Description = "A jury holdout attempts to prevent a miscarriage of justice by forcing his colleagues to reconsider the evidence.",
                    ReleaseDate = DateTime.Parse("10-04-1957"),
                    User = user3
                },
                new Movie()
                {
                    Title = "Inception",
                    Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                    ReleaseDate = DateTime.Parse("13-07-2010"),
                    User = user2
                },
                new Movie()
                {
                    Title = "Fight Club",
                    Description = "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much, much more.",
                    ReleaseDate = DateTime.Parse("21-09-1999"),
                    User = user1
                },
                new Movie()
                {
                    Title = "Forrest Gump",
                    Description = "The presidencies of Kennedy and Johnson, the events of Vietnam, Watergate and other historical events unfold through the perspective of an Alabama man with an IQ of 75, whose only desire is to be reunited with his childhood sweetheart.",
                    ReleaseDate = DateTime.Parse("23-06-1994"),
                    User = user1
                },
                new Movie()
                {
                    Title = "Matrix",
                    Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                    ReleaseDate = DateTime.Parse("24-03-1999"),
                    User = user2
                }
            };

            await databaseContext.Movies.AddRangeAsync(movies);
            await databaseContext.SaveChangesAsync();
        }
    }
}