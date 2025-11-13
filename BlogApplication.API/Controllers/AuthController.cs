using BlogApplication.Application.Interfaces;
using BlogApplication.Core.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto) {
            try {
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto) {
            try {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email) {
            try {
                var token = await _authService.ForgotPasswordAsync(email);
                return Ok(new { token, message = "Password reset token generated. In production, this would be sent via email." });
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto) {
            try {
                var result = await _authService.ResetPasswordAsync(dto);
                if (result) {
                    return Ok(new { message = "Password has been reset successfully." });
                } else {
                    return BadRequest(new { message = "Failed to reset password. Invalid token or email." });
                }
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
