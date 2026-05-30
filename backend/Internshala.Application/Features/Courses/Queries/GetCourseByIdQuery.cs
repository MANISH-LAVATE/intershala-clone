using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Courses.DTOs;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Courses.Queries;

public sealed record GetCourseByIdQuery(int CourseId) : IRequest<CourseDetailDto>;

public sealed class GetCourseByIdQueryHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<GetCourseByIdQuery, CourseDetailDto>
{
    public async Task<CourseDetailDto> Handle(
        GetCourseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var course = await db.Courses
            .AsNoTracking()
            .Include(c => c.Category)
            .Include(c => c.Modules.Where(m => !m.IsDeleted).OrderBy(m => m.OrderIndex))
            .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken)
            ?? throw new NotFoundException("Course", request.CourseId);

        bool isEnrolled = false;
        byte progress = 0;

        if (currentUser.IsAuthenticated && currentUser.Role == RoleConstants.Student)
        {
            var student = await db.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == currentUser.UserId, cancellationToken);

            if (student is not null)
            {
                var enrollment = await db.Enrollments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.StudentId == student.Id && e.CourseId == course.Id, cancellationToken);

                isEnrolled = enrollment is not null;
                progress = enrollment?.ProgressPercent ?? 0;
            }
        }

        return new CourseDetailDto(
            course.Id,
            course.Title,
            course.Description,
            course.Instructor,
            course.ThumbnailUrl,
            course.Level,
            course.CategoryId,
            course.Category.Name,
            course.DurationHours,
            course.IsFree,
            course.Price,
            course.EnrolledCount,
            course.Language,
            course.Prerequisites,
            course.WhatYouLearn,
            course.Modules.Select(m => new CourseModuleDto(
                m.Id, m.Title, m.Description, m.VideoUrl,
                m.OrderIndex, m.DurationMinutes, m.IsPreview)).ToList(),
            isEnrolled,
            progress);
    }
}
