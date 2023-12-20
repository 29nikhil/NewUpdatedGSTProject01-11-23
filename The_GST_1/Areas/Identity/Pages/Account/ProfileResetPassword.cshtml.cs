// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web.Providers.Entities;
using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit;
using Repository_Logic.UserOtherDatails.Interface;

namespace The_GST_1.Areas.Identity.Pages.Account
{
    public class ProfileResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        IWebHostEnvironment _webHostEnvironment;
        private readonly IExtraDetails _extra;
        private readonly Application_Db_Context _context;

         public ProfileResetPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment, IExtraDetails extra, Application_Db_Context context)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            _extra = extra;
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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        public IActionResult OnGet(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                // Handle the case when the email parameter is not provided
                return RedirectToPage("/SomeFallbackPage");
            }
            Input = new InputModel
            {
                Email = email,
                //    Password = userData.Password,
               
            };
            // Use the email parameter as needed (e.g., display it on the page, etc.)
            ViewData["Email"] = email;

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    TempData["ErrorMessageResetPassword"] ="  Your Email is Not Confirm add Valid Email ,Please Confirm Your Email";

                    // Don't reveal that the user does not exist or is not confirmed
                    return Page();
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                protocol: Request.Scheme);

                var userdata = _context.appUser.Where(x=>x.Id==user.Id).FirstOrDefault();

                var webRoot = _webHostEnvironment.WebRootPath;

                var pathToFile = Path.Combine(webRoot, "EmailTamplates", "ResetPassword.html")
                 .Replace('\\', '/'); // Replace backslashes with forward slashes

                //Email Tamplate Rendering
                //string Message = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                var subject = "Change Your Password ";
                var builder = new BodyBuilder();

                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                string messageBody = builder.HtmlBody.Replace("{Subject}", subject)
                                                     .Replace("{Date}", $"{DateTime.Now:dddd, d MMMM yyyy}")
                                                     .Replace("{Email}", user.Email)
                                                     .Replace("{FirstName}", userdata.FirstName )
                                                     //.Replace("{GstNo}", userdata.GSTNo)

                                                       .Replace("{FullName}", userdata.FirstName + " " + userdata.MiddleName + " " + userdata.LastName)
                                                     .Replace("{ConfirmationLink}", callbackUrl);







                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                   messageBody);

                return RedirectToPage("Home","Index");
            }

            return Page();
        }
    }
}
