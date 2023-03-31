using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Trippin_Website.Logic_classes
{
    public class EmailSender
    {
        private EmailSenderHelper senderHelper;
        public EmailSender()
        {
        }
        public async Task EmailConfirmation(string toEmail, string ToName, string callBackUrl)
        {
            senderHelper = new EmailSenderHelper(callBackUrl);
            var apiKey = $"{senderHelper.ApiKey}";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress($"{senderHelper.From}", $"{senderHelper.FromName}");
            var subject = $"Salut {ToName}! Confirma contul tau pe Trippin";
            var to = new EmailAddress($"{toEmail}", $"{ToName}");
            var plainTextContent = "";
            var htmlContent = senderHelper.ConfirmationHtmlBody;

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);
        }
    }
}