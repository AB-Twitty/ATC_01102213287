using Evenda.App.Models;
using System.Net;

namespace Evenda.Services.Services.Base
{
    public abstract class BaseService
    {
        private Dictionary<string, string[]> ParseErrors(IDictionary<string, string> errors)
        {
            return errors.Select(x => new KeyValuePair<string, string[]>(x.Key, new[] { x.Value })).ToDictionary(x => x.Key, x => x.Value);
        }

        protected virtual BaseResponse Success(string message)
        {
            return new BaseResponse
            {
                StatusCode = HttpStatusCode.OK,
                Message = message
            };
        }

        protected virtual BaseResponse Error(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, IDictionary<string, IList<string>>? errors = null)
        {
            return new BaseResponse
            {
                StatusCode = statusCode,
                Message = message,
                Errors = errors
            };
        }

        protected virtual BaseResponse NotFound(string message = "Resource not found")
        {
            return new BaseResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = message
            };
        }

        protected virtual BaseResponse Created(string message = "Resource created")
        {
            return new BaseResponse
            {
                StatusCode = HttpStatusCode.Created,
                Message = message
            };
        }

        protected virtual BaseResponse NoContent(string message = "No content")
        {
            return new BaseResponse
            {
                StatusCode = HttpStatusCode.NoContent,
                Message = message
            };
        }

        protected virtual BaseResponse Unauthorized(string message = "Unauthorized")
        {
            return new BaseResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = message
            };
        }





        protected virtual DataResponse<TData> ValidationError<TData>(IDictionary<string, IList<string>> errors)
        {
            return new DataResponse<TData>
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = "One or more validation errors occurred.",
                Errors = errors
            };
        }

        protected virtual DataResponse<TData> Unauthorized<TData>(string message = "Unauthorized")
        {
            return new DataResponse<TData>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = message
            };
        }

        protected virtual DataResponse<TData> Success<TData>(TData data, string message = "Request was successful")
        {
            return new DataResponse<TData>
            {
                StatusCode = HttpStatusCode.OK,
                Message = message,
                Data = data
            };
        }

        protected virtual DataResponse<TData> Error<TData>(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, IDictionary<string, IList<string>>? errors = null)
        {
            return new DataResponse<TData>
            {
                StatusCode = statusCode,
                Message = message,
                Errors = errors
            };
        }

        protected virtual DataResponse<TData> Created<TData>(TData data, string message = "Resource created")
        {
            return new DataResponse<TData>
            {
                StatusCode = HttpStatusCode.Created,
                Message = message,
                Data = data
            };
        }

        protected virtual DataResponse<TData> NotFound<TData>(string message = "Resource not found")
        {
            return new DataResponse<TData>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = message
            };
        }

        protected virtual DataResponse<TData> BadRequest<TData>(string message = "Bad Request")
        {
            return new DataResponse<TData>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = message
            };
        }
    }
}