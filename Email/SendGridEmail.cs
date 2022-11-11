using SendGrid;
using SendGrid.Helpers.Mail;

namespace TDM.Email
{
    internal class SendGridEmail
    {
        private readonly IConfiguration Configuration;

        public SendGridEmail(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        internal async Task<Response> Execute(string toEmail, string body)
        {
            var apiKey = Configuration["SendGrid:SENDGRID_API_KEY"];
            var client = new SendGridClient(apiKey);
            string fromEmail = Configuration["SendGrid:FROM_EMAIL"];
            var from = new EmailAddress(fromEmail, fromEmail + " from 3D models manager");
            var subject = "Email confirmation OTP for 3D Models Manager";
            var to = new EmailAddress(toEmail, toEmail);
            var plainTextContent = body;
            var htmlContent = "<strong>"+body+"</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return await client.SendEmailAsync(msg);
        }
    }
}