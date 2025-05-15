namespace Evenda.UI.Contracts.IServices
{
    public interface IApiTokenService
    {
        Task<(bool, string)> TryRefreshToken();
    }
}
