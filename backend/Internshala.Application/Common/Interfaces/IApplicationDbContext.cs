using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DomainApplication = Internshala.Domain.Entities.Application;

namespace Internshala.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Student> Students { get; }
    DbSet<Employer> Employers { get; }
    DbSet<Internship> Internships { get; }
    DbSet<Job> Jobs { get; }
    DbSet<DomainApplication> Applications { get; }
    DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; }
    DbSet<Category> Categories { get; }
    DbSet<Location> Locations { get; }
    DbSet<Skill> Skills { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<Education> Educations { get; }
    DbSet<Experience> Experiences { get; }
    DbSet<Resume> Resumes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
