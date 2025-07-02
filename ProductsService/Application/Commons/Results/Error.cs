using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Results
{
    public sealed record Error(string Code, string? Description = null, List<ErrorDetail>? Detail = null)
    {
        public const string Invalid = "InvalidRequestError";
        public const string Unauthenticated = "InvalidCredentialError";
        public const string Unauthorized = "InvalidPermissionError";
        public const string NotFound = "NotFoundError";
        public const string Unhandled = "InternalError";

        public static readonly Error None = new(string.Empty);

        public static Error InvalidError(string description, List<ErrorDetail>? detail = null) => new(Invalid, description, detail);
        public static Error UnauthenticatedError(string description, List<ErrorDetail>? detail = null) => new(Unauthenticated, description, detail);
        public static Error UnauthorizedError(string description, List<ErrorDetail>? detail = null) => new(Unauthorized, description, detail);
        public static Error NotFoundError(string description, List<ErrorDetail>? detail = null) => new(NotFound, description, detail);
        public static Error UnhandledError(string description, List<ErrorDetail>? detail = null) => new(Unhandled, description, detail);
    }
}
