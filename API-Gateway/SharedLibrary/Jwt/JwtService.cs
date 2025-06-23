using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.DTOs.Token;
using SharedLibrary.DTOs.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Jwt
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<UserToHashPassword> _passwordHasher;

        public JwtService(IConfiguration configuration, IPasswordHasher<UserToHashPassword> passwordHasher)
        {
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public Task<string> GenerateToken(object obj)
        {
            var claims = ExtractClaimsFromObject(obj);
            return Task.FromResult(GenerateJwtToken(claims));
        }


        private IEnumerable<Claim> ExtractClaimsFromObject(object obj)
        {
            var claims = new List<Claim>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj);
                if (value == null) continue;

                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    claims.Add(new Claim(prop.Name, value.ToString()));
                }
                else
                {
                    claims.Add(new Claim(prop.Name, value.ToString()));
                }
            }
            return claims;
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<ClaimsPrincipal?> ValidateToken(TokenDTO token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing"));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token.AccessToken, validationParameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult<ClaimsPrincipal?>(null);
                }

                return Task.FromResult<ClaimsPrincipal?>(principal);
            }
            catch (SecurityTokenExpiredException)
            {
                // Token đã hết hạn
                return Task.FromResult<ClaimsPrincipal?>(null);
            }
            catch (SecurityTokenException)
            {
                // Token không hợp lệ
                return Task.FromResult<ClaimsPrincipal?>(null);
            }
            catch
            {
                // Lỗi không xác định
                return Task.FromResult<ClaimsPrincipal?>(null);
            }
        }



        public Task<Guid> ExtractUserIdFromToken(string rawToken)
        {
            if (string.IsNullOrWhiteSpace(rawToken))
                return Task.FromResult(Guid.Empty);

            var token = rawToken.StartsWith("Bearer ") ? rawToken.Substring(7) : rawToken;

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadJwtToken(token);

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
