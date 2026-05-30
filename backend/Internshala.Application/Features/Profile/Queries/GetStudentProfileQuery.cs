using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Profile.DTOs;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Profile.Queries;

public sealed record GetStudentProfileQuery : IRequest<StudentProfileDto>;

public sealed class GetStudentProfileQueryHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<GetStudentProfileQuery, StudentProfileDto>
{
    public async Task<StudentProfileDto> Handle(
        GetStudentProfileQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var student = await db.Students
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Educations)
            .Include(s => s.Experiences)
            .Include(s => s.Resume)
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Student profile", userId);

        return MapToDto(student);
    }

    private static StudentProfileDto MapToDto(Domain.Entities.Student s) => new(
        s.Id,
        s.UserId,
        s.User.FirstName,
        s.User.LastName,
        s.User.Email,
        s.User.PhoneNumber,
        s.User.ProfilePictureUrl,
        s.DateOfBirth,
        s.Gender,
        s.CurrentInstitution,
        s.CourseOfStudy,
        s.GraduationYear,
        s.Gpa,
        s.Bio,
        s.LinkedInUrl,
        s.GitHubUrl,
        s.PortfolioUrl,
        s.ProfileCompleteness,
        s.IsProfileComplete,
        s.Educations.Select(e => new EducationDto(
            e.Id, e.Institution, e.Degree, e.FieldOfStudy,
            e.StartYear, e.EndYear, e.IsCurrent, e.Grade)).ToList(),
        s.Experiences.Select(e => new ExperienceDto(
            e.Id, e.Title, e.Company, e.Description,
            e.StartDate, e.EndDate, e.IsCurrent)).ToList(),
        s.Resume is null ? null : new ResumeDto(
            s.Resume.Id, s.Resume.FileUrl, s.Resume.FileName,
            s.Resume.FileSizeBytes, s.Resume.UploadedAt));
}
