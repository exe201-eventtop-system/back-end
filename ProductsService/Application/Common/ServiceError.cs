using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public sealed record ServiceError(string ErrorCode, string Message)
    {
        public static readonly string NotFoundErrorCode = "NotFound";
        public static readonly string ValidationErrorCode = "ValidationError";
        public static readonly string OperationFailedErrorCode = "OperationFailedError";
        public static readonly string UnknownErrorCode = "UnknownError";

        /// <summary>
        ///     The service operation executed successfully
        /// </summary>
        public static readonly ServiceError None = new(string.Empty, string.Empty);

        /// <summary>
        ///     Create a new error when the service operation failed because an entity record was not found.
        /// </summary>
        /// <param name="message">custom error message</param>
        /// <returns></returns>
        public static ServiceError NotFound(string message)
        {
            return new ServiceError(NotFoundErrorCode, message);
        }

        /// <summary>
        ///     Create a new error when the service operation failed because a validation check failed.
        /// </summary>
        /// <param name="message">custom error message</param>
        /// <returns></returns>
        public static ServiceError ValidationError(string message)
        {
            return new ServiceError(ValidationErrorCode, message);
        }

        /// <summary>
        ///     Create a new error when the service operation failed because the service actively throw an error.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ServiceError OperationFailed(string message)
        {
            return new ServiceError(OperationFailedErrorCode, message);
        }


        /// <summary>
        ///     Create a new error when the service operation failed because an unknown reason.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ServiceError UnknownError(string message)
        {
            return new ServiceError(UnknownErrorCode, message);
        }
    }
}
