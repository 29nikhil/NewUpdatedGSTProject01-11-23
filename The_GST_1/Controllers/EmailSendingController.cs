﻿using GST_Web.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Repository_Logic.UserOtherDatails.Interface;
using Repository_Logic.Email_Activity.Interface;
using Data_Access_Layer.Models;
using Repository_Logic.Dto;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.ModelView;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace The_GST_1.Controllers
{
    [Authorize]
    public class EmailSendingController : Controller
    {



        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _sender;
        private readonly IExtraDetails _extra;
        private readonly IEmailActivity _emailActivity;
        private readonly IFellowshipRepository _fellowship;
        string odlUrl = "EmailSending/Email_Confirmatioin_link?";
        string newUrl = "Identity/Account/ConfirmEmail?";

        public EmailSendingController(UserManager<IdentityUser> userManager, IEmailSender sender, IExtraDetails extra, IEmailActivity emailActivity,IFellowshipRepository fellowship)
        {
            _userManager = userManager;
            _sender = sender;
            _extra = extra;
            _emailActivity = emailActivity;
            _fellowship = fellowship;
        }
       

        public async Task<IActionResult> Email_ReConfirmation_link()
        {

            string Email = Request.Query["email"];
            string UserId = Request.Query["id"];


            if (!ModelState.IsValid)
            {
                return View();
            }
            var userdata = _extra.GetUser(UserId);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
                return View();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/EmailSending/Email_Confirmatioin_link?",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            string odlUrl = "EmailSending/Email_ReConfirmation_link?";
            string newUrl = "Identity/Account/ConfirmEmail?";

            string Urldata = callbackUrl.Replace(odlUrl, newUrl).ToString();
            //   string emailTemplate = System.IO.File.ReadAllText("../wwwroot/EmailTamplates/ReConfirmation.html");
            //  emailTemplate = emailTemplate.Replace("{ConfirmationLink}", Urldata);
            string emailContent1 = _emailActivity.GenerateEmailReConfirmationUrl(userdata.FirstName, Email, Urldata);

            await _sender.SendEmailAsync(
                Email,
                "Confirm your email", emailContent1);
            TempData["EmailsendingReconfirmation"] = "Send Confirmation Link User Email:"+Email;

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check  Your New Email ID email.");
            return RedirectToAction("UserList", "UserDetails");
        }

        //public async Task<IActionResult> Email_Confirmatioin_linkFellowship()
        //{

        //    string Email = Request.Query["email"];
        //    string UserId = Request.Query["id"];


        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    var userdata = _fellowship.GetFellowShipṚeccord(UserId);
        //    var user = await _userManager.FindByEmailAsync(Email);
        //    if (user == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
        //        return View();
        //    }

        //    var userId = await _userManager.GetUserIdAsync(user);
        //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //    var callbackUrl = Url.Page(
        //        "/Account/ConfirmEmail",
        //        pageHandler: null,
        //        values: new { userId = userId, code = code },
        //        protocol: Request.Scheme);

        //    string odlUrl = "EmailSending/Email_Confirmatioin_link?";
        //    string newUrl = "Identity/Account/ConfirmEmail?";
        //    string Urldata = callbackUrl.Replace(odlUrl, newUrl).ToString();
        //    //   string emailTemplate = System.IO.File.ReadAllText("../wwwroot/EmailTamplates/ReConfirmation.html");
        //    //  emailTemplate = emailTemplate.Replace("{ConfirmationLink}", Urldata);
        //    string emailContent1 = _emailActivity.GenerateEmailReConfirmationUrl(userdata.FirstName, Email, Urldata);

        //    await _sender.SendEmailAsync(Email, "Confirm your email", emailContent1);
        //    TempData["EmailsendingReconfirmation"] = "Send Confirmation Link User Email:" + Email;

        //    ModelState.AddModelError(string.Empty, "Verification email sent. Please check  Your  email."+Email);
        //    return RedirectToAction("FellowshipList", "Fellowship");
        //}


        //User Email Id Change then send Reconfirm Email Id
        //public async Task<IActionResult> UserEmailIdUpdateUser(string Email, string UserId)
        //{


         
        //        var userdata = _extra.GetUser(UserId);

        //        var user = await _userManager.FindByEmailAsync(Email);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
        //            return View();
        //        }

        //        var userId = await _userManager.GetUserIdAsync(user);
        //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //        var callbackUrl = Url.Page(
        //            "/Account/ConfirmEmail",
        //            pageHandler: null,
        //            values: new { userId = userId, code = code },
        //            protocol: Request.Scheme);

        //        string odlUrl = "EmailSending/Email_Confirmatioin_link?";
        //        string newUrl = "Identity/Account/ConfirmEmail?";
        //        string Urldata = callbackUrl.Replace(odlUrl, newUrl).ToString();
              
        //        string emailContent1 = _emailActivity.GenerateEmailChangeUrl(userdata.FirstName, Email, Urldata);

        //        await _sender.SendEmailAsync(
        //            Email,
        //            "Confirm your email", emailContent1);


        //        ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");




        //        var userId1 = "";

        //        if (User.Identity.IsAuthenticated)
        //        {
        //            userId1 = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        }
        //        if (userId1 == userId)
        //        {

        //            TempData["EmailsendingReconfirmationUserModel"] = "Send Confirmation Link User Email Your New Email ID:" + Email;
        //            return RedirectToAction("ViewProfile", "UserSideModel", new { id = userId });
        //        }
        //        else
        //        {

        //            return RedirectToAction("UserList", "UserDetails");

        //        }
           
        //}









            public async Task<IActionResult> UpdateEmail_Confirmation(string Email, string UserId )
            {


            
              var userdata = _extra.GetUser(UserId);

                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
                    return View();
                }

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = userId, code = code },
                    protocol: Request.Scheme);

                //string odlUrl = "EmailSending/Email_Confirmatioin_link?";
                //string newUrl = "Identity/Account/ConfirmEmail?";
                string Urldata = callbackUrl.Replace(odlUrl, newUrl).ToString();
               
                string emailContent1 = _emailActivity.GenerateEmailChangeUrl(userdata.FirstName, Email, Urldata);

                await _sender.SendEmailAsync( Email,"Confirm your email", emailContent1);
                

                ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");




                var userId1 = "";

                if (User.Identity.IsAuthenticated)
                {
                    userId1 = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                if (userId1 == userId)
                {

                    TempData["EmailsendingReconfirmationUserModel"]= "Send Confirmation Link User Email Your New Email ID:" + Email;
                    return RedirectToAction("ViewProfile", "UserSideModel", new { id = userId });
                }
                else
                {
                TempData["EmailsendingReconfirmationUser"] = "Send Confirmation Link User Email Your New Email ID:" + Email;

                return RedirectToAction("UserList", "UserDetails");

                }

            }
    }
}
