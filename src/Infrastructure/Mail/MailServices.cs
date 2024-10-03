using Application.Contracts;
using Domain.Primitives;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Mail;

public class MailServices(IOptions<MailSettings> options) : IMailServices
{
    public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
    {
        try
        {
            // Command-line argument must be the SMTP host.
            SmtpClient client = new();
            // Specify the email sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new(options.Value.SenderEmail);
            // Set destinations for the email message.
            MailAddress to = new(toEmail);
            // Specify the message content.
            MailMessage mailMessage = new(from, to);
            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Body = message;
            // Include some non-ASCII characters in body and subject.
            string someArrows = new(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            // // Set the method that is called back when the send operation ends.
            // client.SendCompleted += new
            // SendCompletedEventHandler(SendCompletedCallback);
            // The userState can be any object that allows your callback
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            // string userState = "test message1";
            await client.SendMailAsync(mailMessage);
            Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            // string answer = Console.ReadLine();
            // If the user canceled the send, and mail hasn't been sent yet,
            // then cancel the pending operation.
            // if (answer.StartsWith("c") && mailSent == false)
            // {
            //     client.SendAsyncCancel();
            // }
            // Clean up.
            mailMessage.Dispose();
            client.Dispose();
            return true;
        }
        catch
        {
            return false;
        }
    }
}