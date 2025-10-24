using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.Results
{
    public class Result<T>
    {
        [JsonPropertyName("success")]
        public bool IsSuccess { get; }

        [JsonPropertyName("error")]
        public Error? Error { get; }

        [JsonPropertyName("message")]
        public string? Message { get; }

        [JsonPropertyName("data")]
        public T? Data { get; }

        [JsonIgnore]
        public bool IsFailure => !IsSuccess;

        private Result(bool isSuccess, string? message, T? value, Error? error)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = value;
            Error = error;
        }

        [JsonConstructor]
        public Result(bool issuccess, string message, T data)
        {
            IsSuccess = issuccess;
            Message = message;
            Data = data;
        }

        public static Result<T> Success() => new(true, null, default(T), null);

        public static Result<T> Success(T value, string? message = null) => new(true, message, value, null);

        public static Result<T> Failure(Error error, string? message = null) => new(false, message, default(T), error);
    }
}
