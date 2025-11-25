using BlogApplication.Application.Interfaces;
using BlogApplication.Core.DTOs.Auth;
using BlogApplication.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApplication.Application.Services {
    public class AuthService : IAuthService {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<User> userManager, IConfiguration configuration, IEmailService emailService) {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto) {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null) {
                throw new Exception("User with this email already exists");
            }

            var user = new User {
                Email = dto.Email,
                UserName = dto.Email,
                FullName = dto.FullName,
                EmailConfirmed = false,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _emailService.SendEmailVerificationAsync(user.Email!, emailToken);

            var token = GenerateJwtToken(user);

            return new AuthResponseDto {
                Token = token,
                Email = user.Email!,
                FullName = user.FullName ?? string.Empty,
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto) {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null) {
                throw new Exception("Invalid email or password");
            }

            if (!user.EmailConfirmed) {
                throw new Exception("Please verify your email before logging in");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isPasswordValid) {
                throw new Exception("Invalid email or password");
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDto {
                Token = token,
                Email = user.Email!,
                FullName = user.FullName ?? string.Empty,
            };
        }

        public async Task<bool> VerifyEmailAsync(string email, string token) {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) {
                throw new Exception("User not found");
            }

            if (user.EmailConfirmed) {
                throw new Exception("Email is already verified");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded) {
                throw new Exception("Invalid or expired verification token");
            }

            return true;
        }

        public async Task<bool> ResendVerificationEmailAsync(String email) {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) {
                throw new Exception("User not found");
            }

            if (user.EmailConfirmed) {
                throw new Exception("Email is already verified");
            }

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendEmailVerificationAsync(user.Email!, emailToken);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto) {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) {
                throw new Exception("User not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

            if (!result.Succeeded) {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result.Succeeded;
        }

        public async Task<string> ForgotPasswordAsync(string Email) {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null) {
                throw new Exception("User not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        private string GenerateJwtToken(User user) {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
