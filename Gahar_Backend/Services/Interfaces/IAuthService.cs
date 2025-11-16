using Gahar_Backend.Models.DTOs.Auth;

namespace Gahar_Backend.Services.Interfaces
{
    public interface IAuthService
    {
  Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
   Task<bool> LogoutAsync(int userId);
      Task<bool> RegisterAsync(RegisterDto registerDto);
  Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
 Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
