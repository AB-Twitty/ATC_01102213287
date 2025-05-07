namespace Evenda.UI.Contracts.IServices
{
    public interface IApiTokenService
    {
        Task<bool> TryRefreshToken();
        void SetTokens(string accessToken, string refreshToken);
    }
}
