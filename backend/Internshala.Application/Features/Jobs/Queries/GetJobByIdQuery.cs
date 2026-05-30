using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Jobs.DTOs;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Jobs.Queries;

public sealed record GetJobByIdQuery(int Id) : IRequest<JobDetailDto>;

public sealed class GetJobByIdQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetJobByIdQuery, JobDetailDto>
{
    public async Task<JobDetailDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await db.Jobs
            .AsNoTracking()
            .Include(j => j.Employer)
            .Include(j => j.Category)
            .Include(j => j.Location)
            .Include(j => j.Skills)
            .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Job", request.Id);

        return new JobDetailDto(
            job.Id, job.Title, job.Description, job.Requirements,
            job.EmployerId, job.Employer.CompanyName, job.Employer.CompanyLogoUrl, job.Employer.IsVerified,
            job.CategoryId, job.Category.Name,
            job.Location?.CityName, job.JobType, job.SalaryMin, job.SalaryMax, job.ExperienceYearsMin,
            job.ApplicationDeadline, job.ApplicationsCount, job.PublishedAt ?? job.CreatedAt,
            job.Skills.Select(s => s.Name).ToList());
    }
}
