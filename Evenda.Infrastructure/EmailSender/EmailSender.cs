using Evenda.App.Contracts.IInfrastructure.IEmailSender;
using Evenda.App.Models;
using Evenda.App.Utils.Enums;
using Evenda.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Evenda.Infrastructure.EmailSender
{
    internal class EmailSender : IEmailSender
    {
        #region Fields

        private readonly EmailSettings _emailSettings;
        private readonly RazorViewToStringRenderer _razorRenderer;

        #endregion

        #region Ctor

        public EmailSender(IOptions<EmailSettings> opts, RazorViewToStringRenderer razorRenderer)
        {
            _emailSettings = opts.Value;
            _razorRenderer = razorRenderer;
        }

        #endregion

        #region Utils

        protected async Task<string> BuildMailBody(EmailTemplate emailTemplate, object? templateModel)
        {
            var body = string.Empty;

            var templatePath = emailTemplate switch
            {
                EmailTemplate.ForgotPassword => "Email_ForgotPassword.cshtml",
                _ => throw new ArgumentOutOfRangeException(nameof(emailTemplate), $"Unexpected email template: {emailTemplate}")
            };

            body = await _razorRenderer.RenderAsync(templatePath, templateModel);

            return body;
        }

        #endregion

        #region Methods

        public async Task SendEmailAsync(Email email, EmailTemplate emailTemplate, object? templateModel)
        {
            var smtpClient = new SmtpClient(_emailSettings.SmtpServer);
            smtpClient.Port = _emailSettings.SmtpPort;
            //smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Timeout = 600000;

            var body = await BuildMailBody(emailTemplate, templateModel);

            var mailMsg = new MailMessage
            {
                From = new MailAddress(_emailSettings.SmtpUser, "Evenda"),
                To = { new MailAddress(email.ToEmailAddress, email.ToName) },

                Subject = email.Subject,
                Body = body,
                IsBodyHtml = true,
            };

            smtpClient.Send(mailMsg);
        }

        #endregion
    }
}
