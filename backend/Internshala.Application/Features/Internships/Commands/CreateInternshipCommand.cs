using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Internships.DTOs;
using Internshala.Domain.Entities;
using Internshala.Domain.Enums;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Internships.Commands;

public sealed record CreateInternshipCommand(
    CreateInternshipRequest Request) : IRequest<int>;

public sealed class CreateInternshipCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<CreateInternshipCommand, int>
{
    public async Task<int> Handle(CreateInternshipCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException("Authentication is required.");

        var employer = await db.Employers
            .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken)
            ?? throw new ForbiddenException("Only employers can post internships. Complete your employer profile first.");

        var req = command.Request;

        if (!Enum.TryParse<InternshipType>(req.InternshipType, out var internshipType))
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["InternshipType"] = [$"'{req.InternshipType}' is not a valid internship type."]
            });

        var skills = req.SkillIds.Count > 0
            ? await db.Skills.Where(s => req.SkillIds.Contains(s.Id)).ToListAsync(cancellationToken)
            : [];

        var internship = new Internship
        {
            EmployerId = employer.Id,
            CategoryId = req.CategoryId,
            LocationId = req.LocationId,
            Title = req.Title,
            Description = req.Description,
            Responsibilities = req.Responsibilities,
            Requirements = req.Requirements,
            InternshipType = internshipType,
            StipendMin = req.StipendMin,
            StipendMax = req.StipendMax,
            IsPaid = req.IsPaid,
            DurationMonths = req.DurationMonths,
            StartDateType = req.StartDateType,
            StartDate = req.StartDate,
            OpeningsCount = req.OpeningsCount,
            ApplicationDeadline = req.ApplicationDeadline,
            Status = InternshipStatus.Active,
            IsActive = true,
            PublishedAt = DateTime.UtcNow,
            Skills = skills
        };

        db.Internships.Add(internship);
        await db.SaveChangesAsync(cancellationToken);

        return internship.Id;
    }
}
