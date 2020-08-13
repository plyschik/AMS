using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.MVC.ViewModels.PersonViewModels
{
    public class PersonEditViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }
    }
}
