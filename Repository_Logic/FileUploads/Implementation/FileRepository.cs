using Azure;
using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository_Logic.Dto;
using Repository_Logic.FileUploads.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repository_Logic.FileUploads.Implementation
{
    public class FileRepository : IFileRepository
    {

        Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public string Filepath;
        private Application_Db_Context _context;
        public FileRepository(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, Application_Db_Context context)
        {
            _environment = environment;
            _context = context;

        }
        public FilePathReturn_Dto UploadData(IFormFile UploadAdharFile, IFormFile UploadPanFile)
        {


            string AdharFilePath = SaveAdharFile(UploadAdharFile);
            string PanFilePath = SavePanFile(UploadPanFile);
            FilePathReturn_Dto FilePathReturn_ = new FilePathReturn_Dto();
            FilePathReturn_.UploadAdharPath = AdharFilePath;
            FilePathReturn_.UploadPanPath = PanFilePath;
           
            return FilePathReturn_;
        }

        public string SaveAdharFile(IFormFile UploadAdharFile)
        {



            if (UploadAdharFile == null || UploadAdharFile.Length <= 0)
            {
                return null;
            }

            // Generate a unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + UploadAdharFile.FileName;
            var filePath = Path.Combine(_environment.WebRootPath, "AdharCard", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Filepath = filePath;
                UploadAdharFile.CopyTo(stream);
                // ViewData["Path"];


            }
            return uniqueFileName;
        }
        public string SavePanFile(IFormFile UploadPanFile)
        {

            if (UploadPanFile == null || UploadPanFile.Length <= 0)
            {
                return null;
            }

            // Generate a unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + UploadPanFile.FileName;
            var filePath = Path.Combine(_environment.WebRootPath, "PanCard", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Filepath = filePath;
                UploadPanFile.CopyTo(stream);
                // ViewData["Path"];

                return uniqueFileName;
            }
            

        }

        public string UpdateAdhar( IFormFile UploadnewFile)
        {

            if (UploadnewFile == null || UploadnewFile.Length <= 0)
            {
                return null;
            }

            // Generate a unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + UploadnewFile.FileName;
            var filePath = Path.Combine(_environment.WebRootPath, "AdharCard", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Filepath = filePath;
                UploadnewFile.CopyTo(stream);
                // ViewData["Path"];


            }
            return uniqueFileName;
        }

        public string UpdatePan( IFormFile UploadnewFile)
        {
            if (UploadnewFile == null || UploadnewFile.Length <= 0)
            {
                return null;
            }

            // Generate a unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + UploadnewFile.FileName;
            var filePath = Path.Combine(_environment.WebRootPath, "PanCard", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Filepath = filePath;
                UploadnewFile.CopyTo(stream);
                // ViewData["Path"];

                return uniqueFileName;
            }
        }

        public string DeletePan(string  UploadPanFile)
        {
            var oldFilePath = Path.Combine(_environment.WebRootPath, "PanCard", UploadPanFile);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);

                // string FileStatus = "Deleted";
                string FileStatus = "Delete";

                return FileStatus;

            }
            else
            {
                string FileStatus = "NotDelete";

                return FileStatus;
            }
        }

        public string DeleteAdhar(string UploadAdharFile)
        {
            var oldFilePath = Path.Combine(_environment.WebRootPath, "AdharCard", UploadAdharFile);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);


                string FileStatus = "Delete";
                return FileStatus;
            }
            else
            {
                string FileStatus = "NotDelete";

                return FileStatus;
            }
        }
    }
}