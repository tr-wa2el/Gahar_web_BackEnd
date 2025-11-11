using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Services.Implementations
{
    public class JwtService : IJwtService
    {
  private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
  {
       _configuration = configuration;
        }

        public string GenerateAccessToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            var claims = new List<Claim>
        {
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
           new Claim(ClaimTypes.Name, user.Username),
       new Claim(ClaimTypes.Email, user.Email),
             new Claim("FirstName", user.FirstName ?? ""),
    new Claim("LastName", user.LastName ?? "")
        };

  // Add roles
 foreach (var role in roles)
            {
        claims.Add(new Claim(ClaimTypes.Role, role));
 }

      // Add permissions
    foreach (var permission in permissions)
       {
            claims.Add(new Claim("Permission", permission));
            }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
       var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
        issuer: _configuration["JwtSettings:Issuer"],
    audience: _configuration["JwtSettings:Audience"],
   claims: claims,
           expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"]!)),
             signingCredentials: credentials
    );

            return new JwtSecurityTokenHandler().WriteToken(token);
     }

        public string GenerateRefreshToken()
        {
        var randomNumber = new byte[64];
    using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

     public ClaimsPrincipal? ValidateToken(string token)
        {
    var tokenHandler = new JwtSecurityTokenHandler();
     var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!);

      try
       {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
     {
          ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(key),
     ValidateIssuer = true,
        ValidIssuer = _configuration["JwtSettings:Issuer"],
     ValidateAudience = true,
   ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
      ClockSkew = TimeSpan.Zero
     }, out var validatedToken);

            return principal;
      }
            catch
       {
           return null;
        }
        }
    }
}
