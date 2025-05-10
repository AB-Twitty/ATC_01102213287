namespace Evenda.UI.Dtos.Auth
{
    public class AuthDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; } = new();
        public string AccessToken { get; set; }
        public DateTimeOffset AccessTokenExpirationDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpirationDate { get; set; }
    }
}
