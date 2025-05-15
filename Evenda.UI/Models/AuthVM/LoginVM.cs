using System.ComponentModel.DataAnnotations;

namespace Evenda.UI.Models.AuthVM
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;

        public string? ReturnUrl { get; set; } = string.Empty;
    }
}
