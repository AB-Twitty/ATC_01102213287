using Evenda.App.Contracts.IInfrastructure.ITokenProvider;
using Evenda.App.Contracts.IPersistence.IUnitOfWork;
using Evenda.Domain.Entities.UserEntities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Evenda.Infrastructure.TokenProvider
{
    public class JwtTokenProvider : ITokenProvider
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        #endregion

        #region Ctor

        public JwtTokenProvider(IUnitOfWork unitOfWork, IOptions<JwtSettings> opts)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = opts.Value;
        }

        #endregion

        #region Methods

        public async Task<(string, DateTime)> GenerateAccessToken(User user)
        {
            var userRoles = user.Roles;
            var roles = userRoles.Select(x => x.SystemName);

            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString())
            }
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var expiresOn = jwtSecurityToken.ValidTo;

            return await Task.FromResult((token, expiresOn));
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public string ExtractUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "uid");
            if (userIdClaim == null)
                throw new Exception("User ID claim not found in token");
            return userIdClaim.Value;
        }

        #endregion
    }
}
