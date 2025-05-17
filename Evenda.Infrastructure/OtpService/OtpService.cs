using Evenda.App.Contracts.IInfrastructure.IEmailSender;
using Evenda.App.Contracts.IInfrastructure.IOtpService;
using Evenda.App.Models;
using Evenda.App.Utils.Constants;
using Evenda.App.Utils.Enums;
using Evenda.Domain.Entities.UserEntities;
using Microsoft.Extensions.Caching.Memory;

namespace Evenda.Infrastructure.OtpService
{
    public class OtpService : IOtpService
    {
        #region Fields

        private readonly IMemoryCache _memoryCache;
        private readonly IEmailSender _emailSender;

        #endregion

        #region Ctor

        public OtpService(IMemoryCache memoryCache, IEmailSender emailSender)
        {
            _memoryCache = memoryCache;
            _emailSender = emailSender;
        }

        #endregion

        #region Utils

        private static string GetCacheKeyPrefix(OtpType otpType)
        {
            return otpType switch
            {
                OtpType.AccountVerification => Constants.ACCOUNT_VERIFICATION_KEY,
                OtpType.ForgotPassword => Constants.FORGOT_PASSWORD_KEY,
                OtpType.Login => Constants.LOGIN_KEY,
                _ => throw new ArgumentOutOfRangeException(nameof(otpType), $"Unexpected OTP type: {otpType}")
            };
        }

        public string GenerateSixDigitsRandomNumber()
        {
            var random = new Random();

            return random.Next(100000, 1000000).ToString();
        }

        #endregion

        #region Methods

        public async Task SendOtpToUser(User user, OtpType otpType)
        {
            var cacheKeyPrefix = GetCacheKeyPrefix(otpType);
            var cacheKey = $"{cacheKeyPrefix}{user.Id}";

            var subject = otpType switch
            {
                OtpType.AccountVerification => "Account Verification",
                OtpType.ForgotPassword => "Reset Password Request",
                OtpType.Login => "Login",
                _ => throw new ArgumentOutOfRangeException(nameof(otpType), $"Unexpected OTP type: {otpType}")
            };

            var emailTemplate = otpType switch
            {
                OtpType.ForgotPassword => EmailTemplate.ForgotPassword,
                _ => throw new ArgumentOutOfRangeException(nameof(otpType), $"Unexpected OTP type: {otpType}")
            };

            if (_memoryCache.Get<string>(cacheKey) != null)
            {
                _memoryCache.Remove(cacheKey);
            }

            var otp = GenerateSixDigitsRandomNumber();

            var email = new Email
            {
                ToEmailAddress = user.Email,
                ToName = user.FirstName + " " + user.LastName,
                Subject = subject,
            };

            try
            {
                await _emailSender.SendEmailAsync(
                    email: email,
                    emailTemplate: emailTemplate,
                    templateModel: new
                    {
                        Otp = otp,
                        ResetUrl = "https://localhost:44366/auth/reset-password",
                        ExpirationTime = Constants.OTP_EXPIRATION_TIME_IN_MINUTES
                    }
                );

                _memoryCache.Set(
                    key: cacheKey,
                    value: otp,
                    absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(Constants.OTP_EXPIRATION_TIME_IN_MINUTES)
                );
            }
            catch
            {
                throw new OperationCanceledException("Something went wrong. Please try again.");
            }
        }

        public bool ValidateOtpForUser(User user, OtpType otpType, string inputOtp)
        {
            var cacheKeyPrefix = GetCacheKeyPrefix(otpType);
            var cacheKey = $"{cacheKeyPrefix}{user.Id}";

            if (_memoryCache.TryGetValue<string>(cacheKey, out var cachedOtp))
            {
                if (cachedOtp == inputOtp)
                {
                    _memoryCache.Remove(cacheKey);
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
