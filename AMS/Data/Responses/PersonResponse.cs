using System;

namespace AMS.Data.Responses
{
    public class PersonResponse
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }
    }
}
