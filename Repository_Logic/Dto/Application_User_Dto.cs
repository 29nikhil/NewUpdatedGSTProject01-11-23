﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class Application_User_Dto
    {

        public string Id { get; set; }
        [Required]

        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only alphabetical characters are allowed & not a Blank Spaces")]
        public string? FirstName { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only alphabetical characters are allowed & not a Blank Spaces")]

        public string? MiddleName { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only alphabetical characters are allowed & not a Blank Spaces")]

        public string? LastName { get; set; }


        [Required]

        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                      ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(10, ErrorMessage = "The Mobile No Length must be 10 Required .", MinimumLength = 10)]

        public string? PhoneNumber { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? Country { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string? city { get; set; }
     
        public string? UserStatus { get; set; }


    }
}
