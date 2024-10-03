namespace Application.Contracts;

public interface IMailServices
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string message);
}