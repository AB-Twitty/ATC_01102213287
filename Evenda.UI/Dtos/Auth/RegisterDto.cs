using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

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
    [Compare(nameof(ConfirmPassword), ErrorMessage = " ")]
    public string Password { get; set; }

    [Required(ErrorMessage = " ")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
