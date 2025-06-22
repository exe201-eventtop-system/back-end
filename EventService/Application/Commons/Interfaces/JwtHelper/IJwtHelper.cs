using Domain.Constants.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Interfaces.JwtHelper
{
    public interface IJwtHelper
    {
        Task<Guid> ExtractUserIdFromToken(string token);

        Task<UserRole?> ExtractRoleFromToken(string token);
    }
}
