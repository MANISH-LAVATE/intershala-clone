namespace Internshala.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailVerificationAsync(string email, string firstName, string token, CancellationToken ct = default);
    Task SendPasswordResetAsync(string email, string firstName, string token, CancellationToken ct = default);
    Task SendApplicationStatusUpdateAsync(string email, string firstName, string internshipTitle, string newStatus, CancellationToken ct = default);
}
