using Evenda.UI.Models.Response;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Evenda.UI.ApiClients
{
    public abstract class ApiClient
    {
        #region Fields

        protected readonly HttpClient _httpClient;

        #endregion

        #region Ctor

        public ApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Api");
        }

        #endregion

        #region Utils

        private void HandleResponse(BaseResponse response)
        {
            if (response.Success) return;

            switch (response.StatusCode)
            {
                case HttpStatusCode.UnprocessableEntity:
                    throw new ValidationException("Validation error occurred", null, response.Errors);
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException(response.Message);
                    break;
            }
        }

        #endregion

        #region Generic Methods

        protected async Task<DataResponse<TData>> GetAsync<TData>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonSerializer.Deserialize<DataResponse<TData>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new JsonException("Failed to deserialize response");

            HandleResponse(baseResponse);

            return baseResponse;
        }

        protected async Task<DataResponse<TData>> PostAsync<TData, TDto>(string url, TDto data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonSerializer.Deserialize<DataResponse<TData>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new JsonException("Failed to deserialize response");

            HandleResponse(baseResponse);

            return baseResponse;
        }

        #endregion

        #region Non-Generic Methods

        protected async Task<BaseResponse> PostAsync<TDto>(string url, TDto data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonSerializer.Deserialize<BaseResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new JsonException("Failed to deserialize response");

            HandleResponse(baseResponse);

            return baseResponse;
        }

        #endregion
    }
}
