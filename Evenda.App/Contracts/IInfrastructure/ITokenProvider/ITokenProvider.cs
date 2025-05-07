using Evenda.Domain.Entities.UserEntities;

namespace Evenda.App.Contracts.IInfrastructure.ITokenProvider
{
    public interface ITokenProvider
    {
        Task<(string, DateTime)> GenerateAccessToken(User user);
        string GenerateRefreshToken();
        string ExtractUserIdFromToken(string token);
    }
}
