namespace backend.Application.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
