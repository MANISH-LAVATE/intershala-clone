using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Profile.DTOs;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Profile.Commands;

public sealed record UpdateStudentProfileCommand(UpdateStudentProfileRequest Request) : IRequest;

public sealed class UpdateStudentProfileCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<UpdateStudentProfileCommand>
{
    public async Task Handle(UpdateStudentProfileCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var student = await db.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Student profile", userId);

        var req = command.Request;

        student.User.FirstName = req.FirstName;
        student.User.LastName = req.LastName;
        student.User.PhoneNumber = req.PhoneNumber;
        student.DateOfBirth = req.DateOfBirth;
        student.Gender = req.Gender;
        student.CurrentInstitution = req.CurrentInstitution;
        student.CourseOfStudy = req.CourseOfStudy;
        student.GraduationYear = req.GraduationYear;
        student.Gpa = req.Gpa;
        student.Bio = req.Bio;
        student.LinkedInUrl = req.LinkedInUrl;
        student.GitHubUrl = req.GitHubUrl;
        student.PortfolioUrl = req.PortfolioUrl;

        student.ProfileCompleteness = ComputeCompleteness(student);
        student.IsProfileComplete = student.ProfileCompleteness >= 80;

        await db.SaveChangesAsync(cancellationToken);
    }

    private static byte ComputeCompleteness(Domain.Entities.Student s)
    {
        var filled = 0;
        var total = 10;

        if (!string.IsNullOrWhiteSpace(s.User.FirstName)) filled++;
        if (!string.IsNullOrWhiteSpace(s.User.LastName)) filled++;
        if (s.DateOfBirth.HasValue) filled++;
        if (!string.IsNullOrWhiteSpace(s.CurrentInstitution)) filled++;
        if (!string.IsNullOrWhiteSpace(s.CourseOfStudy)) filled++;
        if (s.GraduationYear.HasValue) filled++;
        if (!string.IsNullOrWhiteSpace(s.Bio)) filled++;
        if (!string.IsNullOrWhiteSpace(s.User.PhoneNumber)) filled++;
        if (!string.IsNullOrWhiteSpace(s.LinkedInUrl)) filled++;
        if (!string.IsNullOrWhiteSpace(s.GitHubUrl) || !string.IsNullOrWhiteSpace(s.PortfolioUrl)) filled++;

        return (byte)(filled * 100 / total);
    }
}
