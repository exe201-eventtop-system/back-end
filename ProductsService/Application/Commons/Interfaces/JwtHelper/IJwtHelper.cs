using SharedLibrary.Enum;

namespace Application.Commons.Interfaces.JwtHelper
{
    public interface IJwtHelper
    {
        Task<Guid> ExtractUserIdFromToken(string token);

        Task<UserRole?> ExtractRoleFromToken(string token);
    }
}
