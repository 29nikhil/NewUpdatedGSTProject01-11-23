using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class JoinUserTable_Dto
    {




        public string Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
      
        public string FirstName { get; set; }

        public string userName { get; set; }

        [Required]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }



        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }




        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }




        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }




        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }




        [Required]
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        
        public string PhoneNumber { get; set; }



        [Required]
        [Display(Name = "User Status")]
        public string UserStatus { get; set; }

        [Required]

        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]

        public string Email { get; set; }
        public int UserOtherDetailsId { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = " GST Number Required")]
        // Other Details Table Data Feilds
        [Display(Name = "GST Number:")]
       

        public string? GSTNo { get; set; }


        [Required(ErrorMessage = "PAN Number is Required")]
        [Display(Name = "PAN NO")]
        public string? PANNo { get; set; }
        [Required]
        [Display(Name = "Adhar Card No")]
        public string? AdharNo { get; set; }

        [Required]
        [Display(Name = "Business Type")]
        public string? BusinessType { get; set; }
        [Required(ErrorMessage = "Please enter website url")]

        [Display(Name = "Website")]
        [Url]
        public string? website { get; set; }
        public string? UploadPAN { get; set; }
        public string? UploadAadhar { get; set; }
        public IFormFile? AdharFile { get; set; }
        public IFormFile? PanFile { get; set; }
        public bool? Confirm { get; set; }
        public string Password { get; set; }
    }
}
