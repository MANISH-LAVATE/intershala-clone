using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Courses.DTOs;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Courses.Queries;

public sealed record GetMyEnrollmentsQuery : IRequest<IReadOnlyList<EnrollmentDto>>;

public sealed class GetMyEnrollmentsQueryHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<GetMyEnrollmentsQuery, IReadOnlyList<EnrollmentDto>>
{
    public async Task<IReadOnlyList<EnrollmentDto>> Handle(
        GetMyEnrollmentsQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser.Role != RoleConstants.Student)
            throw new ForbiddenException("Only students can view enrollments.");

        var userId = currentUser.UserId ?? throw new UnauthorizedException();

        var student = await db.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Student profile", userId);

        return await db.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == student.Id)
            .OrderByDescending(e => e.LastAccessedAt ?? e.CreatedAt)
            .Select(e => new EnrollmentDto(
                e.Id,
                e.CourseId,
                e.Course.Title,
                e.Course.ThumbnailUrl,
                e.ProgressPercent,
                e.CompletedModules,
                e.Course.Modules.Count(m => !m.IsDeleted),
                e.CreatedAt,
                e.CompletedAt,
                e.LastAccessedAt))
            .ToListAsync(cancellationToken);
    }
}
