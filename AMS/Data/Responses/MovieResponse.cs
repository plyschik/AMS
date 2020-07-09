using System;

namespace AMS.Data.Responses
{
    public class MovieResponse
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
    
        public DateTime ReleaseDate { get; set; }
    
        public long Duration { get; set; }
    }
}
