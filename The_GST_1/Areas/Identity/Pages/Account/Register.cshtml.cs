// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repository_Logic;
using Repository_Logic.Dto;
using Repository_Logic.FileUploads.Implementation;
using Repository_Logic.FileUploads.Interface;
using Repository_Logic.RegistorLogsRepository.Interface;
using Repository_Logic.UserOtherDatails.implementation;
using Repository_Logic.UserOtherDatails.Interface;

namespace The_GST_1.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "CA,Fellowship")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IdentityRole _roleManeger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly Application_Db_Context _aa;
        private readonly IExtraDetails _extraDetails;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileRepository _fileRepository;
        private readonly IRegisterLogs _resistorLogs;
        //private IRepository<UserOtherDetails> genericRepository = null;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            Application_Db_Context aa,
            IExtraDetails extraDetails,
            IWebHostEnvironment webHostEnvironment,IFileRepository  fileRepository, IRegisterLogs resistorLogs
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.roleManager = roleManager;
            _aa = aa;
            _extraDetails = extraDetails;
            _webHostEnvironment = webHostEnvironment;
            _fileRepository = fileRepository;
            _resistorLogs = resistorLogs;
            //this.genericRepository = new Repository<UserOtherDetails>();


        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }
        public InputModel Input2 { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 
           

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string MiddleName { get; set; }

            [Required]
            public string LastName { get; set; }
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            [Required(ErrorMessage = "Phone Number Required!")]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                      ErrorMessage = "Entered phone format is not valid.")]
            public string PhoneNumber { get; set; }
            [Required]
            [Display(Name = "Address")]
            public string Address { get; set; }
            [Required]
            [Display(Name = "Country")]
            
            public string Country { get; set; }

            [Display(Name = "Date")]
            public DateTime Date { get; set; }
            [Required]

            [Display(Name = "City")]
            public string City { get; set; }
          


            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required(ErrorMessage = " GST Number Required")]
            // Other Details Table Data Feilds
            [Display(Name = "GST Number:")]
            [StringLength(15, ErrorMessage = "GST Number must be 15 characters.")]
            public string GSTNo { get; set; }
            
            [Required(ErrorMessage = "PAN Number is Required")]
            [Display(Name = "PAN NO")]
            public string PANNo { get; set; }
            [StringLength(12, ErrorMessage = "The Adhar Card No Length .", MinimumLength = 12)]

            [Required]
            [Display(Name = "Adhar Card No")]
            public string AdharNo { get; set; }
            [Required]
            [Display(Name = "Business Type")]
            public string BusinessType { get; set; }
            [Required(ErrorMessage = "Please enter website url")]

            [Display(Name = "Website")]
            [Url]
            public string website { get; set; }
            [Required]
            [Display(Name = "Website")]
            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }


            [Display(Name = "Upload PAN")]
            public string UploadPAN { get; set; }

            [Display(Name = "Upload Aadhar")]
            public string UploadAadhar { get; set; }
            public string Roles { get; set; }
            [NotMapped]
            [DisplayName("Upload Adhar File")]
            [DataType(DataType.Upload)]
            [Required]
            public IFormFile UploadAdharPath { get; set; }
            [DisplayName("Upload Pan File")]
            [DataType(DataType.Upload)]
            [Required]
            public IFormFile UploadPanPath { get; set; }

            public Blob imagedata { get; set; }
        }

        public class InputModel2
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 


            [Required]
            public string FirstName { get; set; }

            [Required]
            public string MiddleName { get; set; }

            [Required]
            public string LastName { get; set; }
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            [Required(ErrorMessage = "Phone Number Required!")]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                      ErrorMessage = "Entered phone format is not valid.")]
            public string PhoneNumber { get; set; }
            [Required]
            [Display(Name = "Address")]
            public string Address { get; set; }
            [Required]
            [Display(Name = "Country")]

            public string Country { get; set; }

            [Display(Name = "Date")]
            public DateTime Date { get; set; }
            [Required]

            [Display(Name = "City")]
            public string City { get; set; }



            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required(ErrorMessage = " GST Number Required")]
            // Other Details Table Data Feilds
            [Display(Name = "GST Number:")]
            [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GST Number.")]
            [StringLength(15, ErrorMessage = "GST Number must be 15 characters.")]
            public string GSTNo { get; set; }

            [Required(ErrorMessage = "PAN Number is Required")]
            [Display(Name = "PAN NO")]
            public string PANNo { get; set; }

            [Required]
            [Display(Name = "Business Type")]
            public string BusinessType { get; set; }
            [Required(ErrorMessage = "Please enter website url")]

            [Display(Name = "Website")]
            [Url]
            public string website { get; set; }
            [Required]
            [Display(Name = "Website")]
            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }


            [Display(Name = "Upload PAN")]
            public string UploadPAN { get; set; }

            [Display(Name = "Upload Aadhar")]
            public string UploadAadhar { get; set; }
            public string Roles { get; set; }
            [NotMapped]
            [DisplayName("Upload File")]
            [DataType(DataType.Upload)]
            [FileExtensions(Extensions = "jpg,png,gif,jpeg,bmp,svg")]
            public IFormFile File { get; set; }
            public Blob imagedata { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
          //  ViewData["RoleList"] = new[] { "CA", "Fellowship", "User" };
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null )
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // var user = CreateUser();
               
                 var user = new Application_User { FirstName = Input.FirstName, MiddleName = Input.MiddleName, LastName = Input.LastName, PhoneNumber=Input.PhoneNumber, Address = Input.Address, Country = Input.Country,Date=DateTime.Now ,city=Input.City,UserStatus="Not Return" };
               // string filepath = _fileRepository.sendFilePath();

                var user2 = new UserOtherDetails_Dto { GSTNo=Input.GSTNo, PANNo=Input.PANNo,AdharNo=Input.AdharNo, UserId=user.Id, BusinessType=Input.BusinessType, website=Input.website, UploadPAN=Input.UploadPAN,UploadAadhar =Input.UploadAadhar ,UploadAdharPath=Input.UploadAdharPath,UploadPanPath=Input.UploadPanPath  };
                var aa = Input.imagedata;
               
                //   string wwwRootPath = _webHostEnvironment.WebRootPath;

                //   string fileName = Path.GetFileNameWithoutExtension(user2.File.FileName);
                //   string extension = Path.GetExtension(user2.File.FileName);
                ////   user2.File = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                //   string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                //   using (var fileStream = new FileStream(path, FileMode.Create))
                //   {
                //       await user2.File.CopyToAsync(fileStream);
                //   }
                var a = Input.Roles;
               
               
                TempData["dd"]=user.Id;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
              
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                //await _userManager.AddToRoleAsync(user,);
              

                
                var result = await _userManager.CreateAsync(user, Input.Password);
                


                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User registered successfully!";

                    var defaultrole = roleManager.FindByNameAsync(Input.Roles).Result;
                    if (a == "User")
                    {

                        _extraDetails.Add(user2);
                        var loginUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                         var logs=await _resistorLogs.SaveRegistorLogsUser(user2.UserId, loginUser);

                    }
                    IdentityResult roleresult = await _userManager.AddToRoleAsync(user, defaultrole.Name);
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                         "/Account/ConfirmEmail",
                         pageHandler: null,
                         values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                         protocol: Request.Scheme);
                        
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                       
                        
                        
                        
                        $"Hi {HtmlEncoder.Default.Encode(Input.FirstName)} </br>" +
                        $"Your THE GST BOOK Account Successfully Open  [customer portal] account. Click the button below to proceed.<br/>" +
                        $"Click the button below to proceed." +
                        $" Your Password is  {HtmlEncoder.Default.Encode(Input.Password)}" +
                        $"" +
                        $"" +
                        $"" +
                        $"" +
                        $"" +
                        $" Please confirm your account by <a class='btn btn-primary' href='{HtmlEncoder.Default.Encode(callbackUrl)}'>click on</a>.");

                    return RedirectToAction("UserList", "UserDetails", new { id = userId });

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    return LocalRedirect(returnUrl);
                    //}

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                   
                    if(error.Description.Contains("Username "))
                    {
                     TempData["ErrorMessage"] = "Username:"+Input.Email+ " is already taken";

                    }
                    else
                    {
                        TempData["Email"] = Input.Email;
                        TempData["ErrorMessage"] = error.Description;

                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }


       








    }
}
