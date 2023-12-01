using Microsoft.AspNetCore.Identity;
using Repository_Logic.Email_Activity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Azure.Core;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Web.Providers.Entities;
using Repository_Logic.UserOtherDatails.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Data_Access_Layer.Models;

namespace Repository_Logic.Email_Activity.Implemenatation
{


    public class EmailActivity :IEmailActivity, IEmailSender
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _sender;
        private readonly IExtraDetails _extra;

        public EmailActivity(UserManager<IdentityUser> userManager, IEmailSender sender, IExtraDetails extra)
        {
            _userManager = userManager;
            _sender = sender;
            _extra = extra;
        }

        public string GenerateEmailChangeUrl(string firstName, string email, string callbackUrl)
        {
            string emailContent = $@"Hi {HtmlEncoder.Default.Encode(firstName)}  ,<br>
            Your THE GST BOOK Account has been Email successfully  Changed. Click the button below to proceed:<br><br>
            Your Email is: {HtmlEncoder.Default.Encode(email)}<br><br>
            <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='display: inline-block; background-color: #007bff; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 5px; margin-top: 10px;'>Confirm Your Account</a><br><br>
            If the button above does not work, you can also confirm your account by copying and pasting the following URL into your web browser:<br>";


            return emailContent;
        }
      
        public string GenerateEmailReConfirmationUrl(string firstName, string email, string callbackUrl)
        {









            string htmlContent = @"
<!DOCTYPE html>
<html>
<head>
    <link href='https://fonts.googleapis.com/css?family=Quicksand:400,700' rel='stylesheet'>
    <style>
        body {
            background-color: #98DFEA;
            margin: 0 auto;
            text-align: center;
            padding: 50px;
        }

        .banner {
            width: 400px;
            height: 600px;
            margin: 0 auto;
            background-image: url('http://i.imgur.com/0ggd0lT.jpg');
            -webkit-box-shadow: 10px 10px 20px 0px rgba(0, 0, 0, 0.25);
            -moz-box-shadow: 10px 10px 20px 0px rgba(0, 0, 0, 0.25);
            box-shadow: 10px 10px 20px 0px rgba(0, 0, 0, 0.25);
        }

        .inner-banner {
            position: relative;
            top: 30%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 300px;
            height: 200px;
        }

        .banner p {
            font-family: 'Quicksand', sans-serif;
            font-size: 32px;
            font-weight: 700;
            color: #25283D;
        }

        button {
            width: 200px;
            height: 50px;
            background-color: #07BEB8;
            color: white;
            font-family: 'Quicksand', sans-serif;
            font-weight: 700;
            font-size: 15px;
            letter-spacing: 5px;
            border: none;
        }

        button:hover {
            background-color: #98DFEA;
            color: #07BEB8;
            -webkit-box-shadow: 10px 10px 20px 0px rgba(0, 0, 0, 0.25);
            -moz-box-shadow: 10px 10px 20px 0px rgba(0, 0, 0, 0.25);
            box-shadow: 10px 10px 20px 0px rgba(0, 0, 0, 0.25);
        }
    </style>
</head>
<body>
    <div class='banner'>
        <div class='inner-banner'>
            <p>THE GST BOOK.</p> </br>
            Hi ";
            htmlContent += HtmlEncoder.Default.Encode(firstName);
            htmlContent += ",<br> Click the button below to proceed:<br><br>Your Email is: ";
            htmlContent += HtmlEncoder.Default.Encode(email);
            htmlContent += @"<br><br><a href='";
            htmlContent += HtmlEncoder.Default.Encode(callbackUrl);
            htmlContent += @"' style=' width: 200px;
            height: 50px;
            background-color: #07BEB8;
            color: white;
            font-family: 'Quicksand', sans-serif;
            font-weight: 700;
            font-size: 15px;
            letter-spacing: 5px;
            border: none; '>Confirm Your Account</a><br><br>If the button above does not work, you can also confirm your account by copying and pasting the following URL into your web browser:<br>
</div>
</div>
<a href='#'><button>GRAB THE DEALS</button></a>
</body>
</html>";
            return htmlContent;



        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                string senderEmail = "bookbookshop50 @gmail.com";
                string SenderPassword = "bxfqvcvodrusewzc";

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, SenderPassword);
                MailMessage mailMessage = new MailMessage(senderEmail, email, subject, htmlMessage);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);

            }

            catch (Exception ex)
            {

            }
            return null;
        }
    

        public async Task SendEmailReConfirmationLink(string Email, string id)
        {
           
        }

        public string UpdateEmail(string Email)
        {
            throw new NotImplementedException();
        }
    }
}
