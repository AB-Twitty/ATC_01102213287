using Evenda.UI.Models.Response;

namespace Evenda.UI.Exceptions
{
    public class ApiException : Exception
    {
        public BaseResponse ApiResponse { get; set; }

        public ApiException(BaseResponse response) : base(response.Message)
        {
            ApiResponse = response;
        }
    }
}
