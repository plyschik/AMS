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
            
            foreach (var index in Enumerable.Range(1, 5))
            {
                var user = new ApplicationUser
                {
                    Email = $"user{index}@ams.ams",
                    UserName = $"user{index}@ams.ams",
                    EmailConfirmed = true
                };
                
                await userManager.CreateAsync(user, "P4ssword!");
                await userManager.AddToRoleAsync(user, "User");
            }
            
            var managers = new List<ApplicationUser>();
            foreach (var index in Enumerable.Range(1, 3))
            {
                var user = new ApplicationUser
                {
                    Email = $"manager{index}@ams.ams",
                    UserName = $"manager{index}@ams.ams",
                    EmailConfirmed = true
                };
                
                await userManager.CreateAsync(user, "P4ssword!");
                await userManager.AddToRoleAsync(user, "Manager");
                
                managers.Add(user);
            }
            
            foreach (var index in Enumerable.Range(1, 2))
            {
                var user = new ApplicationUser
                {
                    Email = $"administrator{index}@ams.ams",
                    UserName = $"administrator{index}@ams.ams",
                    EmailConfirmed = true
                };
                
                await userManager.CreateAsync(user, "P4ssword!");
                await userManager.AddToRoleAsync(user, "Administrator");
            }

            var genres = new List<Genre>();
            while (genres.Count < 10)
            {
                var name = GenerateName();
                
                if (!genres.Exists(g => g.Name == name))
                {
                    var genre = new Genre
                    {
                        Name = name
                    };
                    
                    await databaseContext.Genres.AddAsync(genre);
                    await databaseContext.SaveChangesAsync();
                    
                    genres.Add(genre);
                }
            }
            
            var persons = new List<Person>();
            foreach (var _ in Enumerable.Range(0, 25))
            {
                var person = new Person
                {
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last(),
                    DateOfBirth = Faker.Identification.DateOfBirth()
                };
                
                await databaseContext.Persons.AddAsync(person);
                await databaseContext.SaveChangesAsync();
                
                persons.Add(person);
            }
            
            foreach (var _ in Enumerable.Range(0, 50))
            {
                var movie = new Movie
                {
                    Title = GenerateName(3, 6),
                    Description = string.Join(" ", Faker.Lorem.Sentences(4)),
                    ReleaseDate = Faker.Identification.DateOfBirth(),
                    User = managers[Faker.RandomNumber.Next(0, managers.Count - 1)]
                };
                
                await databaseContext.Movies.AddAsync(movie);
                await databaseContext.SaveChangesAsync();
                
                var genresAmount = Faker.RandomNumber.Next(2, 4);
                var directorsAmount = Faker.RandomNumber.Next(1, 2);
                var writersAmount = Faker.RandomNumber.Next(1, 3);
                var starsAmount = Faker.RandomNumber.Next(5, 10);

                var shuffledGenres = genres.OrderBy(x => Guid.NewGuid()).ToList();
                var shuffledPersons = persons.OrderBy(x => Guid.NewGuid()).ToList();

                foreach (var index in Enumerable.Range(0, genresAmount))
                {
                    await databaseContext.MovieGenres.AddAsync(new MovieGenre
                    {
                        Movie = movie,
                        Genre = shuffledGenres[index]
                    });
                    await databaseContext.SaveChangesAsync();
                }

                var shuffledPersonsSkip = 0;
                
                var directors = shuffledPersons
                    .Skip(shuffledPersonsSkip)
                    .Take(directorsAmount)
                    .Select(person => new MovieDirector
                    {
                        Movie = movie,
                        Person = person
                    });
                
                await databaseContext.MovieDirectors.AddRangeAsync(directors);
                await databaseContext.SaveChangesAsync();
                
                shuffledPersonsSkip += directorsAmount;
                
                var writers = shuffledPersons
                    .Skip(shuffledPersonsSkip)
                    .Take(writersAmount)
                    .Select(person => new MovieWriter
                    {
                        Movie = movie,
                        Person = person
                    });
                
                await databaseContext.MovieWriters.AddRangeAsync(writers);
                await databaseContext.SaveChangesAsync();
                
                shuffledPersonsSkip += starsAmount;

                var stars = shuffledPersons
                    .Skip(shuffledPersonsSkip)
                    .Take(starsAmount)
                    .Select(person => new MovieStar
                    {
                        Movie = movie,
                        Person = person,
                        Character = Faker.Name.FullName()
                    });
                
                await databaseContext.MovieStars.AddRangeAsync(stars);
                await databaseContext.SaveChangesAsync();
            }
        }

        private static string GenerateName(int minWords = 1, int maxWords = 1)
        {
            var name = string.Join(
                " ",
                Faker.Lorem.Words(new Random().Next(minWords, maxWords))
            );

            return name.First().ToString().ToUpper() + name.Substring(1);
        }
    }
}
