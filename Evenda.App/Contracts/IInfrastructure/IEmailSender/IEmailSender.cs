using Evenda.App.Models;
using Evenda.App.Utils.Enums;

namespace Evenda.App.Contracts.IInfrastructure.IEmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Email email, EmailTemplate emailTemplate, object? templateModel);
    }
}
