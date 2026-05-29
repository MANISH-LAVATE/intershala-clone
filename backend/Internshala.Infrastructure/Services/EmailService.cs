using Internshala.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Internshala.Infrastructure.Services;

public sealed class EmailService(ILogger<EmailService> logger) : IEmailService
{
    public Task SendEmailVerificationAsync(string email, string firstName, string token, CancellationToken ct = default)
    {
        logger.LogInformation("Sending email verification to {Email}", email);
        return Task.CompletedTask;
    }

    public Task SendPasswordResetAsync(string email, string firstName, string token, CancellationToken ct = default)
    {
        logger.LogInformation("Sending password reset to {Email}", email);
        return Task.CompletedTask;
    }

    public Task SendApplicationStatusUpdateAsync(string email, string firstName, string internshipTitle, string newStatus, CancellationToken ct = default)
    {
        logger.LogInformation("Sending application status update to {Email}: {Status}", email, newStatus);
        return Task.CompletedTask;
    }
}
