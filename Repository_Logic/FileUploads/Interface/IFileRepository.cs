using Microsoft.AspNetCore.Http;
using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.FileUploads.Interface
{
    public interface IFileRepository
    {
       
        //Both Documents Adhar and Pan Car Upload Storege then return File Name 
        FilePathReturn_Dto UploadData(IFormFile UploadAdharFile, IFormFile UploadPanFile);
       
        string UpdateAdhar( IFormFile UploadnewFile);
        string UpdatePan(  IFormFile UploadnewFile);
        string DeletePan(string UploadAdharFile);
        string DeleteAdhar(string UploadPanFile);
        string UploadProfilePic(IFormFile UploadProfilePic);
        string UpdateProfilePic(IFormFile UploadProfilePic);
        public string DeleteOldProfilePic(string UploadProfilePic);

    }
}
