using Application.Commons.Interfaces.JwtHelper;
using Domain.Constants.UserRoles;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration configuration;

        public JwtHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<UserRole?> ExtractRoleFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult<UserRole?>(null);

            var raw_token = token.StartsWith("Bearer ") ? token.Substring(7) : token;

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadJwtToken(raw_token);

                var role_claim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (role_claim != null && Enum.TryParse<UserRole>(role_claim.Value, out var user_role))
                {
                    return Task.FromResult<UserRole?>(user_role);
                }
            }
            catch
            {
                // Bắt lỗi khi token không hợp lệ hoặc không parse được

            }

            return Task.FromResult<UserRole?>(null);
        }

        public Task<Guid> ExtractUserIdFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult(Guid.Empty);

            var raw_token = token.StartsWith("Bearer ") ? token.Substring(7) : token;

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadJwtToken(raw_token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id");

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Task.FromResult(userId);
                }
            }
            catch
            {
                // Bắt lỗi khi token không hợp lệ hoặc không parse được

            }

            return Task.FromResult(Guid.Empty);
        }
    }
}
