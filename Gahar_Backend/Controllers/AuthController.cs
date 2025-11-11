using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Gahar_Backend.Models.DTOs.Auth;
using Gahar_Backend.Services.Interfaces;
using System.Security.Claims;

namespace Gahar_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
 private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
  _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// ????? ??????
        /// </summary>
    [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);
            var response = await _authService.LoginAsync(loginDto);
    return Ok(response);
        }

        /// <summary>
        /// ????? ?????? ????
        /// </summary>
   [HttpPost("register")]
        [AllowAnonymous]
     [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            _logger.LogInformation("Registration attempt for email: {Email}", registerDto.Email);
     var result = await _authService.RegisterAsync(registerDto);
return Ok(new { message = "Registration successful" });
   }

      /// <summary>
    /// ????? ??????
        /// </summary>
    [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
   {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
     await _authService.LogoutAsync(userId);
         return Ok(new { message = "Logout successful" });
      }

        /// <summary>
   /// ????? ???? ??????
        /// </summary>
        [HttpPost("change-password")]
     [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
  var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);
   return Ok(new { message = "Password changed successfully" });
        }

   /// <summary>
        /// ????? ????? ???? ??????
        /// </summary>
   [HttpPost("reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
 [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
   var result = await _authService.ResetPasswordAsync(resetPasswordDto);
            return Ok(new { message = "Password reset successful" });
        }

        /// <summary>
      /// ????? Access Token ???????? Refresh Token
/// </summary>
   [HttpPost("refresh-token")]
    [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
      var response = await _authService.RefreshTokenAsync(refreshToken);
 return Ok(response);
        }

        /// <summary>
        /// ?????? ??? ??????? ???????? ??????
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
     public IActionResult GetCurrentUser()
{
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var username = User.FindFirst(ClaimTypes.Name)?.Value;
 var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            var userDto = new UserDto
        {
          Id = int.Parse(userId ?? "0"),
   Username = username ?? "",
        Email = email ?? "",
                Roles = roles
            };

            return Ok(userDto);
        }
    }
}
