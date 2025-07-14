using backend.Application.Services;
using System.Net;
using System.Net.Mail;

namespace backend.Infrastructure.Repositories
{
    public class MailService : IMailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("alpaykuzu0@gmail.com", "hcom divw mlbm dtnm sgip");
                client.Port = 587;
                client.EnableSsl = true;
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("alpaykuzu0@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
