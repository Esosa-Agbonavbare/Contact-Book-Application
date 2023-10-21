using ContactBookApp.Core.Services.Interfaces;
using ContactBookApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactBookApp.Core.Services.Implementations
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenGeneratorService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(User user, string roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, roles)
            };

            if (double.TryParse(jwtSettings["AccessTokenExpiration"], out double accessTokenExpirationMinutes))
            {
                accessTokenExpirationMinutes = 30;
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTSettings:Issuer"],
                audience: _configuration["JWTSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes),
                signingCredentials: credential
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
