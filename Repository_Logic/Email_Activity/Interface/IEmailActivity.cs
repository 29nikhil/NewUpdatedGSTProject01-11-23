using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Email_Activity.Interface
{
    public interface IEmailActivity
    {
        public  Task SendEmailReConfirmationLink(string Email, string id);
        public string GenerateEmailChangeUrl(string firstName, string email, string callbackUrl);
        public string GenerateEmailReConfirmationUrl(string firstName, string email, string callbackUrl);

        public Task SendEmailAsync(string email, string subject, string htmlMessage);




    }
}
