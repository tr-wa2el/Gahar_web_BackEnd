using Gahar_Backend.Models.DTOs.Auth;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations
{
    public class AuthService : IAuthService
 {
  private readonly IUserRepository _userRepository;
     private readonly IJwtService _jwtService;
   private readonly IAuditLogService _auditLogService;

        public AuthService(
       IUserRepository userRepository,
     IJwtService jwtService,
    IAuditLogService auditLogService)
{
     _userRepository = userRepository;
  _jwtService = jwtService;
     _auditLogService = auditLogService;
     }

   public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
     {
          var user = await _userRepository.GetByEmailAsync(loginDto.Email);

   if (user == null)
  throw new UnauthorizedException("Invalid email or password");

    // Check if account is locked
  if (user.LockedUntil.HasValue && user.LockedUntil > DateTime.UtcNow)
    throw new UnauthorizedException($"Account is locked until {user.LockedUntil}");

     // Verify password
       if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
     {
     // Increment failed attempts
    user.FailedLoginAttempts++;
     
     if (user.FailedLoginAttempts >= 5)
    {
   user.LockedUntil = DateTime.UtcNow.AddMinutes(30);
 }

          await _userRepository.UpdateAsync(user);

      throw new UnauthorizedException("Invalid email or password");
      }

   // Reset failed attempts
    user.FailedLoginAttempts = 0;
  user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

    // Get roles and permissions
  var roles = await _userRepository.GetUserRolesAsync(user.Id);
   var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);

    // Generate tokens
      var accessToken = _jwtService.GenerateAccessToken(user, roles, permissions);
       var refreshToken = _jwtService.GenerateRefreshToken();

   // Log audit
       await _auditLogService.LogAsync(user.Id, "Login", "User", user.Id, "User logged in successfully");

          return new AuthResponseDto
{
        AccessToken = accessToken,
    RefreshToken = refreshToken,
             ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
       {
     Id = user.Id,
     Username = user.Username,
  Email = user.Email,
    FirstName = user.FirstName,
      LastName = user.LastName,
     Roles = roles.ToList()
       }
        };
        }

 public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
   // Check if user exists
     if (await _userRepository.ExistsByEmailAsync(registerDto.Email))
       throw new BadRequestException("Email already exists");

   // Hash password
    var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

       var user = new User
      {
      Username = registerDto.Username,
       Email = registerDto.Email,
         PasswordHash = passwordHash,
    FirstName = registerDto.FirstName,
     LastName = registerDto.LastName,
     PhoneNumber = registerDto.PhoneNumber
   };

         await _userRepository.CreateAsync(user);

  // Assign default role (e.g., Viewer)
       await _userRepository.AssignRoleAsync(user.Id, "Viewer");

 return true;
  }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
 // TODO: Implement refresh token logic with RefreshToken entity
  throw new NotImplementedException();
     }

   public async Task<bool> LogoutAsync(int userId)
   {
      // TODO: Implement logout logic (invalidate refresh token)
    return await Task.FromResult(true);
     }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
 {
      var user = await _userRepository.GetByIdAsync(userId);
  if (user == null)
    throw new NotFoundException("User not found");

    // Verify current password
       if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
         throw new BadRequestException("Current password is incorrect");

        // Hash new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
       await _userRepository.UpdateAsync(user);

    await _auditLogService.LogAsync(userId, "ChangePassword", "User", userId, "Password changed successfully");

      return true;
     }

     public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
{
  // TODO: Implement reset password logic with token validation
  throw new NotImplementedException();
}
    }
}
