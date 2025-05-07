namespace Evenda.App.Contracts
{
    public interface IWorkContext
    {
        string GetAccessToken();
        string GetCurrentUserId();
    }
}
