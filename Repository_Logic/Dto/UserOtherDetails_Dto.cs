using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class UserOtherDetails_Dto
    {

        public int UserOtherDetailsId { get; set; }
        public string? UserId { get; set; }
        public string? GSTNo { get; set; }

        public string? PANNo { get; set; }
        public string? AdharNo { get; set; }
        public string? BusinessType { get; set; }
        public string? website { get; set; }
        public string? UploadPAN { get; set; }
        public string? UploadAadhar { get; set; }
      
        [FileExtensions(Extensions = "jpg,png,gif,jpeg,bmp,svg")]
        public IFormFile? File { get; set; }


        [DisplayName("Upload Adhar File")]
        [DataType(DataType.Upload)]
        public IFormFile UploadAdharPath { get; set; }
        [DisplayName("Upload Pan File")]
        [DataType(DataType.Upload)]

        public IFormFile UploadPanPath { get; set; }

    

    }
}
