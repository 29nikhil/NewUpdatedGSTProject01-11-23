using Azure.Core;
using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;
using Repository_Logic.Dto;
using Repository_Logic.ErrorLogsRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Providers.Entities;


namespace Repository_Logic.ErrorLogsRepository.Implementation
{
    public class ErrorLogsImplementation : IErrorLogs
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Application_Db_Context _context;
         private readonly IEmailSender _sender;
        public ErrorLogsImplementation(Application_Db_Context context, IWebHostEnvironment webHostEnvironment, IEmailSender sender)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _sender = sender;
        }
        public async void InsertErrorLog(ErrorLog_Dto errorLog)
        {
            ErrorLog errorDetails = new ErrorLog();
            errorDetails.Date = DateTime.Now;
            errorDetails.Message = errorLog.Message;
            errorDetails.StackTrace = errorLog.StackTrace;
            _context.errorLogs.Add(errorDetails);
            _context.SaveChanges();
           
        }

        //public async Task SendErrorDetailsThroughEmail(ErrorLog_Dto errorDetails)
        //{
        //    var webRoot = _webHostEnvironment.WebRootPath;

        //    var pathToFile = Path.Combine(webRoot, "EmailTamplates", "ErrorMessage.html")
        //     .Replace('\\', '/'); // Replace backslashes with forward slashes

        //    //Email Tamplate Rendering
        //    //string Message = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
          
        //    var builder = new BodyBuilder();

        //    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
        //    {
        //        builder.HtmlBody = SourceReader.ReadToEnd();
        //    }

        //    string messageBody = builder.HtmlBody.Replace("{Subject}", "Error Details")
        //                                         .Replace("{Date}", $"{DateTime.Now:dddd, d MMMM yyy HH:mm:ss}")
        //                                         .Replace("{ErrorLocation}", errorDetails.StackTrace)
        //                                         .Replace("{ErrorMessage}", errorDetails.Message)
        //                                         .Replace("{ErrorDate}", $"{errorDetails.Date}");
                                                
        //    await _sender.SendEmailAsync("sur@narola.email", "Error occurred on " + $"{DateTime.Now:dddd, d MMMM yyyy h:mm:ss tt}", messageBody);

        //}
    }
}