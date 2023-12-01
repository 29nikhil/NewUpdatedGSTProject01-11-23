using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GST_Web.EmailSender
{
   

 
        public class EmailSender : IEmailSender
        {
            public EmailSender()
            {

            }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {


            try
            {
                string senderEmail = "bookbookshop50@gmail.com";
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
        }

        //public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        //{
        //    string fromMail = "[bookbookshop50@gmail.com]";
        //    string fromPassword = "[APPPASSWORD]";

        //    MailMessage message = new MailMessage();
        //    message.From = new MailAddress(fromMail);
        //    message.Subject = subject;
        //    message.To.Add(new MailAddress(email));
        //    message.Body = "<html><body> " + htmlMessage + " </body></html>";
        //    message.IsBodyHtml = true;

        //    var smtpClient = new SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential(fromMail, fromPassword),
        //        EnableSsl = true,
        //    };
        //    smtpClient.Send(message);
        //}








//        using (MailMessage mail = new MailMessage())
//{
//    mail.From = new MailAddress("email@gmail.com");
//    mail.To.Add("somebody@domain.com");
//    mail.Subject = "Hello World";
//    mail.Body = "<h1>Hello</h1>";
//    mail.IsBodyHtml = true;
//    mail.Attachments.Add(new Attachment("C:\\file.zip"));

//    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
//    {
//        smtp.Credentials = new NetworkCredential("email@gmail.com", "password");
//    smtp.EnableSsl = true;
//        smtp.Send(mail);
//    }
//}

    }
    }

