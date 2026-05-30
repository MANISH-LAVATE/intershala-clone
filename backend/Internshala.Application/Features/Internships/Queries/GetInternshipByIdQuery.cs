using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Internships.DTOs;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Internships.Queries;

public sealed record GetInternshipByIdQuery(int Id) : IRequest<InternshipDetailDto>;

public sealed class GetInternshipByIdQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetInternshipByIdQuery, InternshipDetailDto>
{
    public async Task<InternshipDetailDto> Handle(
        GetInternshipByIdQuery request,
        CancellationToken cancellationToken)
    {
        var internship = await db.Internships
            .AsNoTracking()
            .Include(i => i.Employer)
            .Include(i => i.Category)
            .Include(i => i.Location)
            .Include(i => i.Skills)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Internship", request.Id);

        return new InternshipDetailDto(
            internship.Id,
            internship.Title,
            internship.Description,
            internship.Responsibilities,
            internship.Requirements,
            internship.EmployerId,
            internship.Employer.CompanyName,
            internship.Employer.CompanyLogoUrl,
            internship.Employer.CompanyWebsite,
            internship.Employer.IsVerified,
            internship.Employer.Description,
            internship.CategoryId,
            internship.Category.Name,
            internship.Location?.CityName,
            internship.InternshipType,
            internship.StipendMin,
            internship.StipendMax,
            internship.IsPaid,
            internship.DurationMonths,
            internship.StartDateType,
            internship.StartDate,
            internship.OpeningsCount,
            internship.ApplicationDeadline,
            internship.ApplicationsCount,
            internship.ViewsCount,
            internship.IsFeatured,
            internship.PublishedAt ?? internship.CreatedAt,
            internship.Skills.Select(s => s.Name).ToList());
    }
}
