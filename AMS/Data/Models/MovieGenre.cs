using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Data.Models
{
    [Table("movie_genre")]
    public class MovieGenre
    {
        [Column("movie_id")]
        public int MovieId { get; set; }
        
        [Column("genre_id")]
        public int GenreId { get; set; }

        public Movie Movie { get; set; }
        
        public Genre Genre { get; set; }
    }
}
