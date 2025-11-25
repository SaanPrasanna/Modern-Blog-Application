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
                return Ok(new {
                    result.Token,
                    result.Email,
                    result.FullName,
                    message = "Registration successfully. Please check your email to verify your account"
                });
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

        // GET endpoint
        [HttpGet("verify-email")]
        public async Task<ContentResult> VerifyEmailGet([FromQuery] string email, [FromQuery] string token) {
            try {
                var result = await _authService.VerifyEmailAsync(email, token);
                if (result) {
                    return Content(@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <title>Email Verified</title>
                            <meta charset='utf-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1'>
                            <style>
                                * { margin: 0; padding: 0; box-sizing: border-box; }
                                body {
                                    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Arial, sans-serif;
                                    display: flex;
                                    justify-content: center;
                                    align-items: center;
                                    min-height: 100vh;
                                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                                }
                                .container {
                                    background: white;
                                    padding: 50px 40px;
                                    border-radius: 15px;
                                    box-shadow: 0 20px 60px rgba(0,0,0,0.3);
                                    text-align: center;
                                    max-width: 500px;
                                    animation: slideIn 0.5s ease-out;
                                }
                                @keyframes slideIn {
                                    from { transform: translateY(-30px); opacity: 0; }
                                    to { transform: translateY(0); opacity: 1; }
                                }
                                .success-icon {
                                    width: 100px;
                                    height: 100px;
                                    background: #4CAF50;
                                    border-radius: 50%;
                                    display: flex;
                                    align-items: center;
                                    justify-content: center;
                                    margin: 0 auto 25px;
                                    font-size: 60px;
                                    color: white;
                                    animation: scaleIn 0.5s ease-out 0.2s both;
                                }
                                @keyframes scaleIn {
                                    from { transform: scale(0); }
                                    to { transform: scale(1); }
                                }
                                h1 { 
                                    color: #333; 
                                    margin-bottom: 15px;
                                    font-size: 28px;
                                }
                                p { 
                                    color: #666; 
                                    line-height: 1.6;
                                    font-size: 16px;
                                }
                                .info {
                                    margin-top: 25px;
                                    padding: 15px;
                                    background: #f5f5f5;
                                    border-radius: 8px;
                                    font-size: 14px;
                                    color: #888;
                                }
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <div class='success-icon'>✓</div>
                                <h1>Email Verified Successfully!</h1>
                                <p>Your email has been verified. You can now login to your account and start using Tech Studio Blog.</p>
                                <div class='info'>
                                    You can safely close this window and return to the application.
                                </div>
                            </div>
                        </body>
                        </html>
                    ", "text/html");
                }
                return Content(@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <title>Verification Failed</title>
                        <meta charset='utf-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1'>
                        <style>
                            * { margin: 0; padding: 0; box-sizing: border-box; }
                            body {
                                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Arial, sans-serif;
                                display: flex;
                                justify-content: center;
                                align-items: center;
                                min-height: 100vh;
                                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                            }
                            .container {
                                background: white;
                                padding: 50px 40px;
                                border-radius: 15px;
                                box-shadow: 0 20px 60px rgba(0,0,0,0.3);
                                text-align: center;
                                max-width: 500px;
                            }
                            .error-icon {
                                width: 100px;
                                height: 100px;
                                background: #f44336;
                                border-radius: 50%;
                                display: flex;
                                align-items: center;
                                justify-content: center;
                                margin: 0 auto 25px;
                                font-size: 60px;
                                color: white;
                            }
                            h1 { color: #333; margin-bottom: 15px; font-size: 28px; }
                            p { color: #666; line-height: 1.6; font-size: 16px; }
                            .info {
                                margin-top: 25px;
                                padding: 15px;
                                background: #fff3cd;
                                border-radius: 8px;
                                font-size: 14px;
                                color: #856404;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='error-icon'>✗</div>
                            <h1>Verification Failed</h1>
                            <p>The verification link is invalid or has already been used.</p>
                            <div class='info'>
                                Please request a new verification email or contact support.
                            </div>
                        </div>
                    </body>
                    </html>
                ", "text/html");
            } catch (Exception ex) {
                return Content($@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <title>Error</title>
                        <meta charset='utf-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1'>
                        <style>
                            * {{ margin: 0; padding: 0; box-sizing: border-box; }}
                            body {{
                                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Arial, sans-serif;
                                display: flex;
                                justify-content: center;
                                align-items: center;
                                min-height: 100vh;
                                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                            }}
                            .container {{
                                background: white;
                                padding: 50px 40px;
                                border-radius: 15px;
                                box-shadow: 0 20px 60px rgba(0,0,0,0.3);
                                text-align: center;
                                max-width: 500px;
                            }}
                            .error-icon {{
                                width: 100px;
                                height: 100px;
                                background: #f44336;
                                border-radius: 50%;
                                display: flex;
                                align-items: center;
                                justify-content: center;
                                margin: 0 auto 25px;
                                font-size: 60px;
                                color: white;
                            }}
                            h1 {{ color: #333; margin-bottom: 15px; font-size: 28px; }}
                            p {{ color: #666; line-height: 1.6; font-size: 16px; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='error-icon'>!</div>
                            <h1>An Error Occurred</h1>
                            <p>We encountered an error while verifying your email.</p>
                            <div class='error-details'>
                                {ex.Message}
                            </div>
                        </div>
                    </body>
                    </html>
                ", "text/html");
            }
        }

        // POST endpoint
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto dto) {
            try {
                var result = await _authService.VerifyEmailAsync(dto.Email, dto.Token);
                if (result) {
                    return Ok(new { message = "Email verification successfully. You can now login." });
                } else {
                    return BadRequest(new { message = "Email verification failed." });
                }
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerification([FromBody] string email) {
            try {
                var result = await _authService.ResendVerificationEmailAsync(email);

                if (result) {
                    return Ok(new { message = "Verification email resent successfully." });
                }
                return BadRequest(new { message = "Failed to resend verification email." });
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email) {
            try {
                await _authService.ForgotPasswordAsync(email);
                return Ok(new { message = "Password reset email send successfully." });
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
