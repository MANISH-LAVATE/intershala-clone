using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Internships.DTOs;
using Internshala.Domain.Enums;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Internships.Commands;

public sealed record UpdateInternshipCommand(int Id, UpdateInternshipRequest Request) : IRequest;

public sealed class UpdateInternshipCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<UpdateInternshipCommand>
{
    public async Task Handle(UpdateInternshipCommand command, CancellationToken cancellationToken)
    {
        var internship = await db.Internships
            .Include(i => i.Employer)
            .Include(i => i.Skills)
            .FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException("Internship", command.Id);

        if (internship.Employer.UserId != currentUser.UserId && currentUser.Role != RoleConstants.Admin)
            throw new ForbiddenException("You can only update your own internship listings.");

        var req = command.Request;

        if (!Enum.TryParse<InternshipType>(req.InternshipType, out var internshipType))
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["InternshipType"] = [$"'{req.InternshipType}' is not a valid internship type."]
            });

        var skills = req.SkillIds.Count > 0
            ? await db.Skills.Where(s => req.SkillIds.Contains(s.Id)).ToListAsync(cancellationToken)
            : new List<Domain.Entities.Skill>();

        internship.Title = req.Title;
        internship.Description = req.Description;
        internship.Responsibilities = req.Responsibilities;
        internship.Requirements = req.Requirements;
        internship.CategoryId = req.CategoryId;
        internship.LocationId = req.LocationId;
        internship.InternshipType = internshipType;
        internship.StipendMin = req.StipendMin;
        internship.StipendMax = req.StipendMax;
        internship.IsPaid = req.IsPaid;
        internship.DurationMonths = req.DurationMonths;
        internship.StartDateType = req.StartDateType;
        internship.StartDate = req.StartDate;
        internship.OpeningsCount = req.OpeningsCount;
        internship.ApplicationDeadline = req.ApplicationDeadline;
        internship.Skills = skills;

        await db.SaveChangesAsync(cancellationToken);
    }
}
