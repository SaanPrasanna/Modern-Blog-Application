using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Application.Interfaces {
    public interface IEmailService {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendEmailVerificationAsync(string email, string token);
        Task SendPasswordResetEmailAsync(string email, string token);
    }
}
