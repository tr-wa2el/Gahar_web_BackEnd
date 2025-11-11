using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Auth
{
    public class LoginDto
    {
      [Required]
  [EmailAddress]
public string Email { get; set; } = string.Empty;

        [Required]
    public string Password { get; set; } = string.Empty;
    }

  public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
    public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

 public string? FirstName { get; set; }
     public string? LastName { get; set; }
   public string? PhoneNumber { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
      public string Token { get; set; } = string.Empty;

        [Required]
      [MinLength(8)]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class AuthResponseDto
 {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; } = null!;
    }

    public class UserDto
    {
      public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
      public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
      public string? LastName { get; set; }
 public List<string> Roles { get; set; } = new List<string>();
    }
}
