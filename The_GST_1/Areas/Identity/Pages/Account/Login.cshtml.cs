// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Identity;
using Repository_Logic.Dto;
using Repository_Logic.LoginLogsDataRepository.Interface;
using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Web.Providers.Entities;
using Newtonsoft.Json;

namespace The_GST_1.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
        private readonly ILoginLogs _loginLogs;
        private readonly Application_Db_Context _context;
        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, ILoginLogs loginLogs,Application_Db_Context context)
        {
            _loginLogs = loginLogs;
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

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
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            string storedUserInfo = Request.Cookies["UserInfo"];

            if (!string.IsNullOrEmpty(storedUserInfo))
            {
                var userData = JsonConvert.DeserializeObject<InputModel>(storedUserInfo);

                // Populate the InputModel with the stored credentials
                Input = new InputModel
                {
                    Email = userData.Email,
                //    Password = userData.Password,
                    RememberMe = true
                };
                TempData["password"] = userData.Password;
            }


            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            LoginLogs_Dto loginLogs_Dto = new LoginLogs_Dto();
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)
                {
                    // User with the provided email does not exist
                    TempData["ErrorMessageLogin"] = "Username:" + Input.Email + " User does not exist.";

                    ModelState.AddModelError(string.Empty, "User does not exist.");
                    return Page();
                }

                try
                {
                    bool DeleteChecking = _context.appUser.Any(x => x.IsDeleted == true && x.Email == user.Email);
                    if (DeleteChecking )
                    {
                        TempData["ErrorMessageLogin"] = "Username:" + Input.Email + " User is deleted";
                        return Page();
                    }
                }
                catch (NullReferenceException ex)
                {
                    TempData["ErrorMessageLogin"] = "Username:" + Input.Email + " User does not exist.";
                    return Page();
                }

                //if (DeleteChecking!=null)
                //{
                //    TempData["ErrorMessageLogin"] = "Username:" + Input.Email + " Not Exist User.";

                //    // User's email is not confirmed
                //    ModelState.AddModelError(string.Empty, "This Use is Not Exist");
                //    return Page();
                //}
                // Check if the user's email is confirmed



                if (!user.EmailConfirmed)
                {
                    loginLogs_Dto.UserID = user.Id;
                    loginLogs_Dto.Message = "Email is not confirmed";
                    loginLogs_Dto.CurrentStatus = "Failed";
                    _loginLogs.insert(loginLogs_Dto);
                    TempData["ErrorMessageLogin"] = "Username:" + Input.Email + " Your Email not Confirmed Please Check Your Email.";

                    // User's email is not confirmed
                    ModelState.AddModelError(string.Empty, "Email is not confirmed. Please check your email for a confirmation link.");
                    return Page();
                }




                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                     loginLogs_Dto.UserID = user.Id;
                     loginLogs_Dto.Message = "User Logged in Successfully";
                     loginLogs_Dto.CurrentStatus = "Success";
                     
                    _loginLogs.insert(loginLogs_Dto);
                    _logger.LogInformation("User logged in.");
                    var userData = new
                    {
                        Email = Input.Email,
                        Password= Input.Password,
                        
                        // Other non-sensitive data
                    };
                    
                    var userDataJson = JsonConvert.SerializeObject(userData);

                    if (Input.RememberMe)
                    {
                        var cookieOptions = new CookieOptions
                        {
                            Expires = DateTime.Now.AddYears(1),
                            HttpOnly = false, // Set to true if you want to make the cookie accessible only through HTTP requests
                            Secure = true, // Set to true if you want to send the cookie only over HTTPS
                            SameSite = SameSiteMode.None // Adjust as needed
                          
                        };

                        HttpContext.Response.Cookies.Append("UserInfo", userDataJson, cookieOptions);
                    }
                    else
                    {
                        Response.Cookies.Delete("UserInfo");
                    }
                




                    return RedirectToAction("Index", "Home");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {

                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    loginLogs_Dto.UserID = user.Id;
                    loginLogs_Dto.Message = "Invalid Username & Password";
                    loginLogs_Dto.CurrentStatus = "Failed";
                    _loginLogs.insert(loginLogs_Dto);

                    TempData["ErrorMessageLogin"] = "Username:" + Input.Email + " Wrong Password enter correct password.";

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            if (Request.Cookies.TryGetValue("RememberMeCredentials", out var credentials))
            {
                var credentialsArray = credentials.Split('|');
                Input.Email = credentialsArray[0];
                Input.Password = credentialsArray[1];
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Home","Index");
            //return Page();
        }
    }
}
