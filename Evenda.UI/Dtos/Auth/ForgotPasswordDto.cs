using System.ComponentModel.DataAnnotations;

namespace Evenda.UI.Dtos.Auth
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
