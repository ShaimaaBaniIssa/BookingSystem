
using BookingSystem.Utility;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BookingSystem.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        public EmailSender(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }
        public Task SendEmail(string to, string subject, string body, byte[] document)
        {
            var attachment = new Attachment(new MemoryStream(document), "invoice.pdf", "application/pdf");

            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress(to));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.From = new MailAddress(_mailSettings.SenderEmail);
            mailMessage.IsBodyHtml = false;
            Attachment data = attachment;
            mailMessage.Attachments.Add(data);

            SmtpClient smtp = new SmtpClient(_mailSettings.Host, _mailSettings.Port)
            {
                EnableSsl = _mailSettings.EnableSsl,
                Credentials = new NetworkCredential(_mailSettings.SenderEmail, _mailSettings.Password),
            };


            return smtp.SendMailAsync(mailMessage);

        }

       
    }
}
