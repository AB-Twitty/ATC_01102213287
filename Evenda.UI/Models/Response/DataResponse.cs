namespace Evenda.UI.Models.Response
{
    public class DataResponse<TData> : BaseResponse
    {
        public TData Data { get; set; }
    }
}
