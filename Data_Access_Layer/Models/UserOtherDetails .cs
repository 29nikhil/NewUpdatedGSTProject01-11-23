using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data_Access_Layer.Models
{
    public class UserOtherDetails
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserOtherDetailsId { get; set; }
       
        public string? UserId { get; set; }

        [Required(ErrorMessage = " GST Number Required")]
        // Other Details Table Data Feilds
        [Display(Name = "GST Number:")]
        //[StringLength(15, ErrorMessage = "GST Number must be 15 characters.")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Only alphabetical characters and numbers are allowed.")]
        [StringLength(15, ErrorMessage = "The Pan Card No Length must be 10 Required .", MinimumLength = 15)]

        public string? GSTNo { get; set; }
         



        [Required]
        [Display(Name = "PAN NO")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Only alphabetical characters and numbers are allowed.")]
        [StringLength(10, ErrorMessage = "The Pan Card No Length must be 10 Required .", MinimumLength = 10)]
        public string? PANNo { get; set; }

        [StringLength(12, ErrorMessage = "The Adhar Card No Length .", MinimumLength = 12)]

        [Required]
        [Display(Name = "Adhar Card No")]

        public string? AdharNo { get; set; }

        [Required]
        [Display(Name = "Business Type")]
        public string? BusinessType { get; set; }




        [Required]
        [Display(Name = "Website")]
        public string? website { get; set; }




        [Required]
        [Display(Name = "Upload PAN")]
        public string? UploadPAN { get; set; }




        [Required]
        [Display(Name = "Upload Aadhar")]
        public string? UploadAadhar { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        public bool? IsTestData { get; set; }

        public bool? IsDeleted { get; set; }

        public static implicit operator DbSet<object>(UserOtherDetails v)
        {
            throw new NotImplementedException();
        }
    }
}
