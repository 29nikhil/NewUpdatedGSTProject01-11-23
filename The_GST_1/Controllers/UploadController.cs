using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Microsoft.Web.Helpers;
using Repository_Logic.Dto;
using Repository_Logic.FileUploads.Interface;
using Repository_Logic.ModelView;
using System.IO;
using System.Net.Http.Headers;

namespace The_GST_1.Controllers
{
    [Authorize]
    public class UploadController : Controller
{
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly IFileRepository _fileRepository;
        private readonly Application_Db_Context _context;
        public UploadController(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, IFileRepository fileRepository, Application_Db_Context context)
        {
            _environment = environment;
            _fileRepository = fileRepository;
            _context = context;
          
        }
        string pathAdhar;
        [HttpPost]
        public IActionResult UploadImage(IFormFile FileUpload,string userId)
        {
            try
            {
               

                //var FilePAth = _fileRepository.UploadData(FileUpload);
                // _fileRepository.SaveFilePath(userId, FilePAth);
                return View();

            }
            catch (Exception ex) {
                return Json(new { success = false });



            }



            



        }

        [HttpPost]
        public IActionResult SaveFiles(UserOtherDetails_Dto user )
        {

            return Json(new { success = true });

        }
        [HttpGet]
        public PartialViewResult Upload_Document(string Userid) 
        {
            UserOtherDetails_Dto userdt = new UserOtherDetails_Dto();
            UserOtherDetails   user = _context.UserDetails.Where(x => x.UserId == Userid).FirstOrDefault();
            userdt.UploadAadhar = user.UploadAadhar;
            userdt.UploadPAN=user.UploadPAN;
            return PartialView(user);
        }
        
      
    }


}