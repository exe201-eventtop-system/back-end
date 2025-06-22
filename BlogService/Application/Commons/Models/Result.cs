using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public List<Error> Errors { get; }
        public string? Message { get; }
        public T? Data { get; }
        
        public bool IsFailure => !IsSuccess;

        private Result(bool isSuccess, string? message, T? value, Error error)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = value;
            Errors = new List<Error>() { error};
        }

        private Result(bool isSuccess, string? message, T? value, List<Error>? errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = value;
            Errors = errors;
        }

        public static Result<T> Success() => new(true, null, default(T), default(List<Error>));

        public static Result<T> Success(T value, string? message = null) => new(true, message, value, default(List<Error>));

        public static Result<T> Failure(Error error, string? message = null) => new(false, message, default(T), error);

        public static Result<T> Failure(List<Error> errors, string? message = null) => new(false, message, default(T), errors);
    }
}
