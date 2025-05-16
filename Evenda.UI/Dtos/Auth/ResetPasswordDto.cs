using System.ComponentModel.DataAnnotations;

namespace Evenda.UI.Dtos.Auth
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = " ")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = @"Password must be at least 8 characters long.
                    |||Password must contain at least one lowercase letter.
                    |||Password must contain at least one uppercase letter.
                    |||Password must contain at least one number.
                    |||Password must contain at least one special character."
        )]
        public string NewPassword { get; set; }
        [Required]
        [Range(100000, 999999)]
        public string Otp { get; set; }
    }
}
