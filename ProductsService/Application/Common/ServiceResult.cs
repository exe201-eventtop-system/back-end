using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class ServiceResult<T>
    {
        private readonly T? _value;

        private ServiceResult(T value)
        {
            Value = value;
            IsSuccess = true;
            Error = ServiceError.None;
        }
        private ServiceResult(ServiceError error)
        {
            if (error == ServiceError.None)
            {
                throw new ArgumentException("invalid error", nameof(error));
            }
            IsSuccess = false;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public T Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new InvalidOperationException("there is no value for failure");
                }
                return _value!;
            }

            private init => _value = value;
        }
        public ServiceError Error { get; }
        public static ServiceResult<T> Success(T value)
        {
            return new ServiceResult<T>(value);
        }
        public static ServiceResult<T> Failure(ServiceError error)
        {
            return new ServiceResult<T>(error);
        }
    }
}
