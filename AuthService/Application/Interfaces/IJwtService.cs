using AuthService.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJwtService 
    {
        Task<string> GenerateToken(object obj);
        Task<string> GenerateRefreshToken();
        Task<ClaimsPrincipal?> ValidateToken(TokenDTO token);
        Task<User> MapClaimsToUser(ClaimsPrincipal principal);
        Task<Guid> ExtractUserIdFromToken(string token); 
    }
}
