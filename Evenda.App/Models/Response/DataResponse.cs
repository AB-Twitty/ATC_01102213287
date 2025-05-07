namespace Evenda.App.Models
{
    public class DataResponse<TData> : BaseResponse
    {
        public TData Data { get; set; }
    }
}
