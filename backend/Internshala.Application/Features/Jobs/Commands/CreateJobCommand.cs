using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Jobs.DTOs;
using Internshala.Domain.Entities;
using Internshala.Domain.Enums;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Jobs.Commands;

public sealed record CreateJobCommand(CreateJobRequest Request) : IRequest<int>;

public sealed class CreateJobCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<CreateJobCommand, int>
{
    public async Task<int> Handle(CreateJobCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException("Authentication is required.");

        var employer = await db.Employers
            .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken)
            ?? throw new ForbiddenException("Only employers can post jobs. Complete your employer profile first.");

        var req = command.Request;

        if (!Enum.TryParse<InternshipType>(req.JobType, out var jobType))
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["JobType"] = [$"'{req.JobType}' is not a valid job type."]
            });

        var skills = req.SkillIds.Count > 0
            ? await db.Skills.Where(s => req.SkillIds.Contains(s.Id)).ToListAsync(cancellationToken)
            : [];

        var job = new Job
        {
            EmployerId = employer.Id,
            CategoryId = req.CategoryId,
            LocationId = req.LocationId,
            Title = req.Title,
            Description = req.Description,
            Requirements = req.Requirements,
            JobType = jobType,
            SalaryMin = req.SalaryMin,
            SalaryMax = req.SalaryMax,
            ExperienceYearsMin = req.ExperienceYearsMin,
            ApplicationDeadline = req.ApplicationDeadline,
            Status = InternshipStatus.Active,
            IsActive = true,
            PublishedAt = DateTime.UtcNow,
            Skills = skills
        };

        db.Jobs.Add(job);
        await db.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}
