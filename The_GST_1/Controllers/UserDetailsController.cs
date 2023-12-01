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

namespace The_GST_1.Controllers
{
    public class UserDetailsController : Controller
    {
        private readonly IExtraDetails extraDetails;
        private readonly Application_Db_Context _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly IFileRepository _fileRepository;
        private readonly IEmailSender _emailSender;
        private readonly IDeleteLogs _deleteLogs;
        private Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser> _IdentityUserManager;
        public UserDetailsController(IExtraDetails extraDetails, Application_Db_Context context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment,IFileRepository fileRepository, Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser> IdentityUserManager, IDeleteLogs deleteLogs)
        {
            this.extraDetails = extraDetails;
            this._context = context;
            _environment = environment;
            _fileRepository = fileRepository;
            _IdentityUserManager = IdentityUserManager;
            _deleteLogs = deleteLogs;
        }

        [Authorize(Roles = "Fellowship,CA")]

        public IActionResult GetUser(string id)
        {
           
            var UserData= extraDetails.GetUser(id);
            ViewBag.BusinessType=UserData.BusinessType;
            ViewBag.Country=UserData.Country;
            return View(UserData);
        }
        [Authorize(Roles = "Fellowship,CA")]

        public IActionResult GetUserView(string id)
        {

            var UserData = extraDetails.GetUser(id);

            return View(UserData);
        }

        [Authorize(Roles = "Fellowship,CA")]

        public IActionResult DeleteUser(string UserId)
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


            try
            {
                extraDetails.DeleteUser(UserId);
                _deleteLogs.Insert(deleteLog_Dto);
                var UserData = extraDetails.GetAllUser();

                return RedirectToAction("UserList", "UserDetails", UserData);

            }
            catch
            {
                return View();

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

                    extraDetails.UpdateUser(userModelView);
                    extraDetails.UpdateEmailConfirmation(userModelView.Id);
                    TempData["UpdateUser"] = "Update User and Send Confirmation Link Your Email:" + userModelView.Email;
                    return RedirectToAction("UpdateEmail_Confirmation", "EmailSending", new { Email = userModelView.Email, UserId = userModelView.Id });
                }
                else
                {

                    extraDetails.UpdateUser(userModelView);
               
                    var UserData = extraDetails.GetAllUser();
                    TempData["UpdateUserData"] = "Update User Record:" + userModelView.FirstName;

                 

                        return RedirectToAction("UserList", "UserDetails", UserData);

                    

                }




            }
            catch
            {
                return View();

            }

        }
        [Authorize(Roles = "Fellowship,CA")]

        public IActionResult UserList()
        {

            
            var UserData = extraDetails.GetAllUser();
        
            return View(UserData);
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

        [Authorize(Roles = "Fellowship,CA,User")]
        public IActionResult DocumentsView(string Id)
        {

            var data = _context.UserDetails.Where(x => x.UserId ==Id).FirstOrDefault();


            return PartialView("_DocumentsView", data);
        }
        public IActionResult ViewPdfPan(string PanPath)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PanCard", PanPath);
           var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // var a= _fileRepository.ViewFilesPan(UserId);
              return File(fileStream, "application/pdf");
        }
        public IActionResult ViewPdfAdhar(string AdharPath)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AdharCard", AdharPath);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // var a = _fileRepository.ViewFilesAdhar(UserId););
            return File(fileStream, "application/pdf");
        }
     
    }
}
