using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Models
{
    public sealed record Error(string Code, string? Description = null, Dictionary<string, object>? Information = null)
    {
        public const string NotFound = "NotFoundError";
        public const string Unhandled = "InternalError";
        public const string Validation = "ValidationError";
        public const string Existed = "EntityExistedError";
        public const string Unauthorized = "InvalidPermissionError";

        public static readonly Error None = new(string.Empty);

        public static Error NotFoundError(string description) => new(NotFound, description);
        public static Error UnhandledException(string description) => new(Unhandled, description);
        public static Error ValidationFailed(string description) => new(Validation, description);
        public static Error ExistedError(string description) => new(Existed, description);
        public static Error UnauthorizedError(string description) => new(Unauthorized, description);
    }
}
