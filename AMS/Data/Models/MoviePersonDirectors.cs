using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Data.Models
{
    [Table("movie_person_directors")]
    public class MoviePersonDirector
    {
        [Column("movie_id")]
        public int MovieId { get; set; }
        
        [Column("person_id")]
        public int PersonId { get; set; }

        public Movie Movie { get; set; }
        
        public Person Person { get; set; }
    }
}
