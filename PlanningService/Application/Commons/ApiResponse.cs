using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public object? Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(bool success, string message, object? data = null, string? errorCode = null)
        {
            Success = success;
            Message = message;
            Data = data;
            ErrorCode = errorCode;
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public new T? Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(bool success, string message, T? data = default, string? errorCode = null)
            : base(success, message, data, errorCode)
        {
            Data = data;
        }
    }

}
