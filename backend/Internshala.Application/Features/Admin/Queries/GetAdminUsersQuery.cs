using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Admin.DTOs;
using Internshala.Domain.Enums;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Admin.Queries;

public sealed record GetAdminUsersQuery(AdminUserFilterParams Filters) : IRequest<PagedResult<AdminUserDto>>;

public sealed class GetAdminUsersQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetAdminUsersQuery, PagedResult<AdminUserDto>>
{
    public async Task<PagedResult<AdminUserDto>> Handle(
        GetAdminUsersQuery request,
        CancellationToken cancellationToken)
    {
        var f = request.Filters;

        var query = db.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(f.Search))
            query = query.Where(u => u.FirstName.Contains(f.Search)
                || u.LastName.Contains(f.Search)
                || u.Email.Contains(f.Search));

        if (!string.IsNullOrWhiteSpace(f.Role) && Enum.TryParse<UserRole>(f.Role, out var role))
            query = query.Where(u => u.Role == role);

        if (f.IsActive.HasValue)
            query = query.Where(u => u.IsActive == f.IsActive);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((f.Page - 1) * f.PageSize)
            .Take(f.PageSize)
            .Select(u => new AdminUserDto(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Role,
                u.IsActive,
                u.IsEmailVerified,
                u.CreatedAt,
                u.LastLoginAt))
            .ToListAsync(cancellationToken);

        return PagedResult<AdminUserDto>.Create(items, f.Page, f.PageSize, totalCount);
    }
}
