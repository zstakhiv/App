using EPlast.BussinessLayer.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public class EmailConfirmation:IEmailConfirmation
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Адміністрація сайту", "andriishainoha2001@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("andriishainoha2001@gmail.com", "andrii123");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
