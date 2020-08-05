using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AMS.MVC.Data.Validation
{
    public class GenreNameUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext
        )
        {
            var databaseContext = (DatabaseContext) validationContext.GetService(typeof(DatabaseContext));
            var genreExists = databaseContext.Genres.Any(genre => genre.Name.ToLower().Equals(value.ToString().ToLower()));

            if (genreExists)
            {
                return new ValidationResult($"Genre with name \"{value}\" already exists!");
            }
            
            return ValidationResult.Success;
        }
    }
}
