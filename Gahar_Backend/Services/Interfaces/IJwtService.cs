using System.Security.Claims;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Services.Interfaces
{
    public interface IJwtService
  {
     string GenerateAccessToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions);
 string GenerateRefreshToken();
   ClaimsPrincipal? ValidateToken(string token);
    }
}
