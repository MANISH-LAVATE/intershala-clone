using Internshala.Application.Common.Interfaces;
using Internshala.Domain.Entities;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Courses.Commands;

public sealed record EnrollCommand(int CourseId) : IRequest;

public sealed class EnrollCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<EnrollCommand>
{
    public async Task Handle(EnrollCommand command, CancellationToken cancellationToken)
    {
        if (currentUser.Role != RoleConstants.Student)
            throw new ForbiddenException("Only students can enroll in courses.");

        var userId = currentUser.UserId ?? throw new UnauthorizedException();

        var student = await db.Students
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Student profile", userId);

        var course = await db.Courses
            .FirstOrDefaultAsync(c => c.Id == command.CourseId && c.IsPublished, cancellationToken)
            ?? throw new NotFoundException("Course", command.CourseId);

        var alreadyEnrolled = await db.Enrollments.AnyAsync(
            e => e.StudentId == student.Id && e.CourseId == course.Id,
            cancellationToken);

        if (alreadyEnrolled)
            throw new ConflictException("You are already enrolled in this course.");

        db.Enrollments.Add(new Enrollment
        {
            StudentId = student.Id,
            CourseId = course.Id,
            ProgressPercent = 0,
            CompletedModules = 0,
            LastAccessedAt = DateTime.UtcNow
        });

        course.EnrolledCount++;
        await db.SaveChangesAsync(cancellationToken);
    }
}
