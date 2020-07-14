using System;

namespace AMS.Data.Requests
{
    public class PersonUpdateRequest
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }
    }
}
