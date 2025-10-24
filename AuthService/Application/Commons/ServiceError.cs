using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons
{
    public sealed record ServiceError(string Code, string? Description = null, Dictionary<string, object>? Information = null)
    {
        public const string NotFound = "NotFoundError";
        public const string Unhandled = "InternalError";
        public const string Validation = "ValidationError";
        public const string Existed = "EntityExistedError";
        public const string Unauthorized = "InvalidPermissionError";    

        public static readonly ServiceError None = new(string.Empty);

        public static ServiceError NotFoundError(string description) => new(NotFound, description);
        public static ServiceError UnhandledException(string description) => new(Unhandled, description);
        public static ServiceError ValidationFailed(string description) => new(Validation, description);
        public static ServiceError ExistedError(string description) => new(Existed, description);
        public static ServiceError UnauthorizedError(string description) => new(Unauthorized, description);
    }
}
