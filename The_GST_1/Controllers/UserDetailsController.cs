using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Repository_Logic;
using Repository_Logic.Dto;
using Repository_Logic.FileUploads.Interface;
using Repository_Logic.ModelView;
using Repository_Logic.UserOtherDatails.Interface;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Repository_Logic.DeleteLogsRepository.Interface;
using System.Web.Helpers;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Pipes;
using Repository_Logic.ErrorLogsRepository.Interface;

namespace The_GST_1.Controllers
{
    public class UserDetailsController : Controller
    {
        private readonly IErrorLogs _errorLogs;
        private readonly IExtraDetails extraDetails;
        private readonly Application_Db_Context _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly IFileRepository _fileRepository;
        private readonly IEmailSender _emailSender;
        private readonly IDeleteLogs _deleteLogs;
        private Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser> _IdentityUserManager;
        public UserDetailsController(IExtraDetails extraDetails, Application_Db_Context context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment,IFileRepository fileRepository, Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser> IdentityUserManager, IDeleteLogs deleteLogs, IErrorLogs errorLogs)
        {
            this.extraDetails = extraDetails;
            this._context = context;
            _environment = environment;
            _fileRepository = fileRepository;
            _IdentityUserManager = IdentityUserManager;
            _deleteLogs = deleteLogs;
            _errorLogs = errorLogs;
        }

        [Authorize(Roles = "Fellowship,CA")]

        public IActionResult GetUser(string id)
       {
            try
            {
              
                var UserData = extraDetails.GetUser(id);
                ViewBag.BusinessType = UserData.BusinessType;
                ViewBag.Country = UserData.Country;
                ViewBag.AdharPdfName = FileName(UserData.UploadAadhar);
                ViewBag.PanPdfName = FileName(UserData.UploadPAN);
                ViewBag.AdharPdfPath = UserData.UploadAadhar;
                ViewBag.PanPdfPath = UserData.UploadPAN;

                return View(UserData);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "An error occurred while getting the user details for editing";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }
        }
        public static string FileName(string Filename)
        {
            string[] parts = Filename.Split('_');
            string filename = parts.Length >= 2 ? parts[1] : Filename;

            return filename;
        }

        [Authorize(Roles = "Fellowship,CA")]

        public IActionResult GetUserView(string id)
        {

            try
            {
              
                var UserData = extraDetails.GetUser(id);

                return View(UserData);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "An error occurred while getting the user details";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }
        }

        [Authorize(Roles = "Fellowship,CA")]

        public IActionResult DeleteUser(string UserId)
        {
            try
            {
               
                var LoginSessionID = "null";

                if (User.Identity.IsAuthenticated)
                {
                    LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                DeleteLog_Dto deleteLog_Dto = new DeleteLog_Dto();
                deleteLog_Dto.UserID = UserId;
                deleteLog_Dto.UserName = _IdentityUserManager.Users.Where(x => x.Id == UserId).Select(x => x.UserName).FirstOrDefault();
                deleteLog_Dto.DeletedById = LoginSessionID;
                deleteLog_Dto.DeletedByName = _IdentityUserManager.Users.Where(x => x.Id == LoginSessionID).Select(x => x.UserName).FirstOrDefault();



                extraDetails.DeleteUser(UserId);
                _deleteLogs.Insert(deleteLog_Dto);
                var UserData = extraDetails.GetAllUser();

                //return RedirectToAction("UserList", "UserDetails", UserData);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "An error occurred while deleting the user";
                return Json(new { success = false, message = errorMessage });
            }
         

        }
        [Authorize(Roles = "Fellowship,CA")]

        public IActionResult EditUser( JoinUserTable_Dto userModelView)
        {
            try
            {
               

                var useremailcheck = extraDetails.GetUser(userModelView.Id);

                if (useremailcheck.Email != userModelView.Email)
                {
                    var data = _context.appUser.Any(x => x.Email == userModelView.Email);

                    //bool emailExists = _context.appUser.Any(x => x.Email == userModelView.Email);
                    //if(emailExists==false)
                    //{
                        extraDetails.UpdateUser(userModelView);
                        extraDetails.UpdateEmailConfirmation(userModelView.Id);
                        TempData["UpdateUser"] = "Update User and Send Confirmation Link Your Email:" + userModelView.Email;
                        return RedirectToAction("UpdateEmail_Confirmation", "EmailSending", new { Email = userModelView.Email, UserId = userModelView.Id });

                    //}
                    //else
                    //{
                    //    TempData["EmailTaken"] = "This  Email Alerady Taken:" + userModelView.Email;

                    //    return RedirectToAction("GetUser", "UserDetails", new { userModelView.Id });

                    //}

                }
                else
                {

                    extraDetails.UpdateUser(userModelView);
               
                    var UserData = extraDetails.GetAllUser();
                    TempData["UpdateUserData"] = "Update User Record:" + userModelView.FirstName;

                
                    return RedirectToAction("UserList", "UserDetails", UserData);

                    

                }




            }
            catch(Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "An error occurred while getting the user details for editing";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }

        }

     
        [HttpPost]
       public IActionResult CheckEmail(string email, string userid)
         {




            if (string.IsNullOrEmpty(email))
            {
                return Json(new { isValid = false, message = "Email is not provided. Please enter an email." });
            }
            else
            {
                if (!string.IsNullOrEmpty(userid))
                {
                    bool emailExists = _context.appUser.Any(x => x.Email == email && x.Id != userid);
                    if (emailExists)
                    {
                        return Json(false);
                    }
                    else
                    {
                        return Json(true);
                    }

                }
                else
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    bool emailExists = _context.appUser.Any(x => x.Email == email && x.Id != userId);
                    if (emailExists)
                    {
                        return Json(false);
                    }
                    else
                    {
                        return Json(true);
                    }
                }
               
            }

            //var Emailcheck = extraDetails.AvaibleEmail(email);
            // Check email existence
    
         }

        [HttpPost]
        public IActionResult CheckGstNo(string GstNo, string userid)
        {
            if (string.IsNullOrEmpty(GstNo))
            {
                return Json(new { isValid = false, message = "Email is not provided. Please enter an email." });
            }
            else
            {
                if (!string.IsNullOrEmpty(userid))
                {
                    bool emailExists = _context.UserDetails.Any(x => x.GSTNo == GstNo && x.UserId != userid);
                    if (emailExists)
                     {
                        return Json(false);
                    }
                    else
                    {
                        return Json(true);
                    }
                }

                else
                {

                    bool emailExists = _context.UserDetails.Any(x => x.GSTNo == GstNo);
                    if (emailExists)
                    {
                        return Json(false);
                    }
                    else
                    {
                        return Json(true);
                    }
                }
               
            }

            //var Emailcheck = extraDetails.AvaibleEmail(email);
            // Check email existence

        }




        [Authorize(Roles = "Fellowship,CA")]

        public IActionResult UserList()
        {

            try
            {
               
                var UserData = extraDetails.GetAllUser();

                return View(UserData);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "An error occurred while displaying user details";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }
        }






        [Authorize(Roles = "Fellowship,CA")]


        public async Task<JsonResult> GetUserList1()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            var draw = Request.Form["draw"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var UsersInRole = extraDetails.GetAllUserList();
        //    var UsersInRole1 = extraDetails.GetAllUserList();

            //var UsersInRole = await _userManager.GetUsersInRoleAsync("Fellowship");
            totalRecord = UsersInRole.Count();
            //     search data when search value found
            if (!string.IsNullOrEmpty(searchValue))
            {
                UsersInRole = UsersInRole.Where(x => x.FirstName.ToLower().Contains(searchValue.ToLower()) || x.MiddleName.ToLower().Contains(searchValue.ToLower()) || x.LastName.ToLower().Contains(searchValue.ToLower())  || x.Country.ToLower().Contains(searchValue.ToLower()) || x.city.ToLower().Contains(searchValue.ToLower()) || x.Email.ToLower().Contains(searchValue.ToLower()) || x.Address.ToLower().Contains(searchValue.ToLower()) || x.GSTNo.ToLower().Contains(searchValue.ToLower()) || x.PANNo.ToLower().Contains(searchValue.ToLower()) || x.UploadAadhar.ToLower().Contains(searchValue.ToLower()) || x.UploadPAN.ToLower().Contains(searchValue.ToLower()) || x.BusinessType.ToLower().Contains(searchValue.ToLower()) || x.website.ToLower().Contains(searchValue.ToLower()) || x.PhoneNumber.Contains(searchValue)).ToList();
                //     UsersInRole = UsersInRole.Where(x => x.otherDetails.GSTNo.ToLower().Contains(searchValue.ToLower()) || x.otherDetails.PANNo.ToLower().Contains(searchValue.ToLower()) || x.otherDetails.UploadAadhar.ToLower().Contains(searchValue.ToLower()) || x.otherDetails.UploadPAN.ToLower().Contains(searchValue.ToLower()) || x.otherDetails.BusinessType.ToLower().Contains(searchValue.ToLower()) || x.otherDetails.website.ToLower().Contains(searchValue.ToLower()) || x.identityUser.Address.ToLower().Contains(searchValue.ToLower()) || x.identityUser.PhoneNumber.Contains(searchValue)).ToList();
            }


            filterRecord = UsersInRole.Count();



            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                if (sortColumnDirection == "asc")
                {
                    UsersInRole = UsersInRole.OrderBy(u => sortColumn switch
                    {
                        //"UploadAadhar" => u.otherDetails.UploadAadhar,
                        //"UploadPAN" => u.otherDetails.UploadPAN,
                        //"website" => u.otherDetails.website,
                        "BusinessType" => u.BusinessType,
                        "PANNo" => u.PANNo,
                        "GSTNo" => u.GSTNo,
                        "FirstName" => u.FirstName,
                        "MiddleName" => u.MiddleName,
                        "LastName" => u.LastName,
                        "Country" => u.Country,
                        "city" => u.city,
                        "Email" => u.Email,
                        "Address" => u.Address,
                        "PhoneNumber" => u.PhoneNumber,
                        "Id" => u.Id,
                        "Confirm" => u.Confirm.ToString()
                    }).ToList();
                }
                else
                {
                    // Handle descending sorting if needed
                    UsersInRole = UsersInRole.OrderByDescending(u => sortColumn switch
                    {

                        //"UploadAadhar" => u.otherDetails.UploadAadhar,
                        //"UploadPAN" => u.otherDetails.UploadPAN,
                        //"website" => u.otherDetails.website,
                        "BusinessType" => u.BusinessType,
                        "PANNo" => u.PANNo,
                        "GSTNo" => u.GSTNo,
                        "FirstName" => u.FirstName,
                        "MiddleName" => u.MiddleName,
                        "LastName" => u.LastName,
                        "Country" => u.Country,
                        "city" => u.city,
                        "Email" => u.Email,
                        "Address" => u.Address,
                        "PhoneNumber" => u.PhoneNumber,
                        "Id" => u.Id,
                        "Confirm" => u.Confirm.ToString()
                    }).ToList();
                }

            }
            //data = data.OrderBy(sortColumn) + " " + sortColumnDirection;
            var selectedColumns = UsersInRole.Select(u => new
            {
                //u.otherDetails.UploadAadhar,
                //u.otherDetails.UploadPAN,
                //u.otherDetails.website,
                u.BusinessType,
                u.PANNo,
                u.GSTNo,
                u.FirstName,
                u.MiddleName,
                u.LastName,
                u.Country,
                u.city,
                u.PhoneNumber,
                u.Email,
                u.Address,
                u.Id,
                u.Confirm


            }).ToList();

            var FellowshipList = selectedColumns.Skip(skip).Take(pageSize).ToList();
            var returnObj = new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = FellowshipList
            };
            return Json(returnObj);
        }

        [AllowAnonymous]
        public IActionResult DocumentsView(string Id)
        {

            var data = _context.UserDetails.Where(x => x.UserId ==Id).FirstOrDefault();


            return PartialView("_DocumentsView", data);
        }
        [AllowAnonymous]
        public IActionResult ViewPanCard(string PanPath)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PanCard", PanPath);

                //if (!System.IO.File.Exists(filePath))
                //{
                //    return NotFound();
                //}

                var fileExtension = Path.GetExtension(PanPath)?.ToLower();
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                string contentType;

                switch (fileExtension)
                {
                    case ".pdf":
                        contentType = "application/pdf";
                        return File(fileStream, "application/pdf");
                        break;
                    case ".jpg":

                    case ".jpeg":
                        contentType = "image/jpeg";
                        return File(fileStream, "image/jpeg");
                        break;
                    default:
                        // Handle unsupported file types
                        return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "An error occurred while displaying user details";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });
            }



            
        }
        [AllowAnonymous]

        public IActionResult ViewAdharCard(string AdharPath)
        {
            try
            {
               
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AdharCard", AdharPath);
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileExtension = Path.GetExtension(AdharPath)?.ToLower();

                // var a = _fileRepository.ViewFilesAdhar(UserId););
                string contentType;

                switch (fileExtension)
                {
                    case ".pdf":
                        contentType = "application/pdf";
                        return File(fileStream, "application/pdf");
                        break;
                    case ".jpg":

                    case ".jpeg":
                        contentType = "image/jpeg";
                        return File(fileStream, "image/jpeg");
                        break;
                    default:
                        // Handle unsupported file types
                        return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "An error occurred while displaying user details";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });
            }
            


           
        }



     
    }
}
