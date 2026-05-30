using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Internships.DTOs;
using Internshala.Domain.Enums;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Internships.Queries;

public sealed record GetInternshipsQuery(InternshipFilterParams Filters) : IRequest<PagedResult<InternshipListDto>>;

public sealed class GetInternshipsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetInternshipsQuery, PagedResult<InternshipListDto>>
{
    public async Task<PagedResult<InternshipListDto>> Handle(
        GetInternshipsQuery request,
        CancellationToken cancellationToken)
    {
        var f = request.Filters;

        var query = db.Internships
            .AsNoTracking()
            .Where(i => i.IsActive && i.Status == InternshipStatus.Active);

        if (!string.IsNullOrWhiteSpace(f.Search))
            query = query.Where(i => i.Title.Contains(f.Search) || i.Employer.CompanyName.Contains(f.Search));

        if (f.CategoryId.HasValue)
            query = query.Where(i => i.CategoryId == f.CategoryId);

        if (f.IsRemote == true)
            query = query.Where(i => i.InternshipType == InternshipType.Remote || i.LocationId == null);
        else if (f.LocationId.HasValue)
            query = query.Where(i => i.LocationId == f.LocationId);

        if (f.StipendMin.HasValue)
            query = query.Where(i => i.StipendMin >= f.StipendMin || !i.IsPaid);

        if (!string.IsNullOrWhiteSpace(f.InternshipType) &&
            Enum.TryParse<InternshipType>(f.InternshipType, out var type))
            query = query.Where(i => i.InternshipType == type);

        if (f.DurationMonths.HasValue)
            query = query.Where(i => i.DurationMonths <= f.DurationMonths);

        var totalCount = await query.CountAsync(cancellationToken);

        query = f.SortBy.ToLowerInvariant() switch
        {
            "stipend" => f.SortOrder == "asc"
                ? query.OrderBy(i => i.StipendMin)
                : query.OrderByDescending(i => i.StipendMin),
            "applications" => query.OrderByDescending(i => i.ApplicationsCount),
            _ => query.OrderByDescending(i => i.IsFeatured).ThenByDescending(i => i.CreatedAt)
        };

        var items = await query
            .Skip((f.Page - 1) * f.PageSize)
            .Take(f.PageSize)
            .Select(i => new InternshipListDto(
                i.Id,
                i.Title,
                i.Employer.CompanyName,
                i.Employer.CompanyLogoUrl,
                i.Employer.IsVerified,
                i.Location != null ? i.Location.CityName : null,
                i.InternshipType,
                i.StipendMin,
                i.StipendMax,
                i.IsPaid,
                i.DurationMonths,
                i.ApplicationDeadline,
                i.ApplicationsCount,
                i.IsFeatured,
                i.CreatedAt,
                i.Skills.Select(s => s.Name).ToList()))
            .ToListAsync(cancellationToken);

        return PagedResult<InternshipListDto>.Create(items, f.Page, f.PageSize, totalCount);
    }
}
