using Evenda.UI.Models.AuthVM;

namespace Evenda.UI.Dtos.Auth
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginDto(LoginVM loginVM)
        {
            Email = loginVM.Email;
            Password = loginVM.Password;
        }
    }
}
