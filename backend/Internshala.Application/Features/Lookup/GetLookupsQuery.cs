using Internshala.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Lookup;

public sealed record CategoryDto(int Id, string Name, string Slug);
public sealed record LocationDto(int Id, string CityName, string State);
public sealed record SkillDto(int Id, string Name, string Slug);

public sealed record GetCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>;
public sealed record GetLocationsQuery(string? Search) : IRequest<IReadOnlyList<LocationDto>>;
public sealed record GetSkillsQuery(string? Search) : IRequest<IReadOnlyList<SkillDto>>;

public sealed class GetCategoriesQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    public async Task<IReadOnlyList<CategoryDto>> Handle(
        GetCategoriesQuery request, CancellationToken cancellationToken)
        => await db.Categories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto(c.Id, c.Name, c.Slug))
            .ToListAsync(cancellationToken);
}

public sealed class GetLocationsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetLocationsQuery, IReadOnlyList<LocationDto>>
{
    public async Task<IReadOnlyList<LocationDto>> Handle(
        GetLocationsQuery request, CancellationToken cancellationToken)
    {
        var query = db.Locations.AsNoTracking().Where(l => l.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(l => l.CityName.Contains(request.Search));

        return await query
            .OrderBy(l => l.CityName)
            .Select(l => new LocationDto(l.Id, l.CityName, l.State))
            .ToListAsync(cancellationToken);
    }
}

public sealed class GetSkillsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetSkillsQuery, IReadOnlyList<SkillDto>>
{
    public async Task<IReadOnlyList<SkillDto>> Handle(
        GetSkillsQuery request, CancellationToken cancellationToken)
    {
        var query = db.Skills.AsNoTracking().Where(s => s.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(s => s.Name.Contains(request.Search));

        return await query
            .OrderBy(s => s.Name)
            .Select(s => new SkillDto(s.Id, s.Name, s.Slug))
            .Take(50)
            .ToListAsync(cancellationToken);
    }
}
