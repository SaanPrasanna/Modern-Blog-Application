using BlogApplication.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace BlogApplication.Application.Services {
    public class EmailService : IEmailService {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body) {
            var smtpHost = _configuration["Email:SmtpHost"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"]!);
            var smtpUser = _configuration["Email:SmtpUser"];
            var smtpPass = _configuration["Email:SmtpPass"];
            var fromEmail = _configuration["Email:FromEmail"];
            var fromName = _configuration["Email:FromName"];

            using var smtpClient = new SmtpClient(smtpHost, smtpPort) {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage {
                From = new MailAddress(fromEmail!, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendEmailVerificationAsync(string email, string token) {
            var verificationUrl = $"{_configuration["AppUrl"]}/api/Auth/verify-email?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}";

            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #333;'>Welcome to Tech Studio Blog!</h2>
                        <p>Thank you for registering. Please verify your email address by clicking the button below:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{verificationUrl}' 
                               style='background-color: #4CAF50; color: white; padding: 12px 24px; 
                                      text-decoration: none; border-radius: 4px; display: inline-block;'>
                                Verify Email
                            </a>
                        </div>
                        <p style='color: #666; font-size: 12px;'>
                            If the button doesn't work, copy and paste this link into your browser:<br>
                            <a href='{verificationUrl}'>{verificationUrl}</a>
                        </p>
                        <p style='color: #666; font-size: 12px;'>
                            If you didn't create an account, please ignore this email.
                        </p>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(email, "Verify Your Email - Tech Studio Blog", body);
        }

        public async Task SendPasswordResetEmailAsync(string email, string token) {
            var resetUrl = $"{_configuration["AppUrl"]}/api/Auth/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}";

            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #333;'>Password Reset Request</h2>
                        <p>We received a request to reset your password. Click the button below to reset it:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{resetUrl}' 
                               style='background-color: #2196F3; color: white; padding: 12px 24px; 
                                      text-decoration: none; border-radius: 4px; display: inline-block;'>
                                Reset Password
                            </a>
                        </div>
                        <p style='color: #666; font-size: 12px;'>
                            This link will expire in 24 hours for security reasons.
                        </p>
                        <p style='color: #666; font-size: 12px;'>
                            If you didn't request a password reset, please ignore this email.
                        </p>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(email, "Reset Your Password - Tech Studio Blog", body);
        }
    }
}
