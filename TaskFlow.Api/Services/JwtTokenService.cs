using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskFlow.Api.Dtos;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginResponseDto CreateToken(User user)
        {
            var key = _configuration["Jwt:Key"] ?? 
                            throw new InvalidOperationException("JWT signing key is not configured.");

            var issuer = _configuration["Jwt:Issuer"] ?? 
                            throw new InvalidOperationException("JWT issuer is not configured.");

            var audience = _configuration["Jwt:Audience"] ?? 
                            throw new InvalidOperationException("JWT audience is not configured.");

            var expiresMinutes = int.Parse(_configuration["Jwt:ExpiresMinutes"] ?? "60");

            var expiresAtUtc = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(issuer, audience, claims, null, expiresAtUtc, credentials);

            return new LoginResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAtUtc = expiresAtUtc
            };
        }
    }
}
