using Evenda.App.Utils.Enums;
using Evenda.Domain.Entities.UserEntities;

namespace Evenda.App.Contracts.IInfrastructure.IOtpService
{
    public interface IOtpService
    {
        Task SendOtpToUser(User user, OtpType otpType);
        bool ValidateOtpForUser(User user, OtpType otpType, string inputOtp);
    }
}
