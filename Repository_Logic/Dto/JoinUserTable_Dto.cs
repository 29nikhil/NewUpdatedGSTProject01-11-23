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
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only alphabetical characters are allowed & not a Blank Spaces")]

        public string FirstName { get; set; }

        public string userName { get; set; }

        [Required(ErrorMessage = "Middle Name is required.")]
        [Display(Name = "Middle Name")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only alphabetical characters are allowed & not a Blank Spaces")]

        public string MiddleName { get; set; }



        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only alphabetical characters are allowed & not a Blank Spaces")]

        public string LastName { get; set; }



        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Address")]
        public string Address { get; set; }




        [Required(ErrorMessage = "Country is required.")]

        [Display(Name = "Country")]
        public string Country { get; set; }




        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }




        [Required(ErrorMessage = "City is required.")]

        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required ")]
        [StringLength(10, ErrorMessage = "The Mobile No Length must be 10 Required .", MinimumLength = 10)]
        [RegularExpression(@"^[1-9][0-9]{9}$",
             ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }



        [Required]
        [Display(Name = "User Status")]
        public string UserStatus { get; set; }

        [Required(ErrorMessage = "Email is required.")]


        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]

        public string Email { get; set; }
        public int UserOtherDetailsId { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = " GST Number Required")]
        // Other Details Table Data Feilds
        [Display(Name = "GST Number:")]
        [StringLength(15, ErrorMessage = "The Gst  No Length must be 15 Required .", MinimumLength = 15)]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Only alphabetical characters and numbers are allowed.")]

        public string? GSTNo { get; set; }


        [Required(ErrorMessage = "PAN Number is Required")]
        [Display(Name = "PAN NO")]
        [StringLength(10, ErrorMessage = "The Pan Card No Length must be 10 Required .", MinimumLength = 10)]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Only alphabetical characters and numbers are allowed.")]
        public string? PANNo { get; set; }
        [Required]
        [Display(Name = "Adhar Card No")]
        [StringLength(12, ErrorMessage = "The Adhar Card No Length must be 12 Required .", MinimumLength = 12)]

        public string? AdharNo { get; set; }

        [Required]
        [Display(Name = "Business Type")]
        public string? BusinessType { get; set; }
        [Required(ErrorMessage = "Please enter website url")]

        [Display(Name = "Website")]
        [Url]
        public string? website { get; set; }
        public string? ProfileImage { get; set; }
        public string? UploadPAN { get; set; }
       

        public string? UploadAadhar { get; set; }
        public IFormFile? ProfileImageFile { get; set; }
        public IFormFile? AdharFile { get; set; }
        public IFormFile? PanFile { get; set; }
        public bool? Confirm { get; set; }
        public string Password { get; set; }
    }
}
