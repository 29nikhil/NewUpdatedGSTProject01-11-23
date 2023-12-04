using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data_Access_Layer.Models
{
    public class Application_User : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }



        [Required]
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }



        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string  Address { get; set; }




        [Required]
        [Display(Name = "Country")]
        public string? Country { get; set; }




        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }




        [Required]
        [Display(Name = "City")]
        public string?   city { get; set; }


        public string? ProfilePic { get; set; }



        [Required]
        [Display(Name = "User Status")]
        public string? UserStatus { get; set; }

        public static implicit operator Application_User(List<Application_User> v)
        {
            throw new NotImplementedException();
        }
      
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        public bool? IsTestData { get; set; }

        public bool? IsDeleted { get; set; } = false;


    }
}
