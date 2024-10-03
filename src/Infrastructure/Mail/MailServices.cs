using Application.Contracts;
using Domain.Primitives;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Infrastructure.Mail;

public class MailServices(IOptions<MailSettings> options) : IMailServices
{
    public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
    {
        try
        {
            SmtpClient client = new();
            MailAddress from = new(options.Value.SenderEmail);
            MailAddress to = new(toEmail);
            MailMessage mailMessage = new(from, to);
            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Body = message;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            await client.SendMailAsync(mailMessage);
            Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            mailMessage.Dispose();
            client.Dispose();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}