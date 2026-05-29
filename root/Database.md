# Database.md — Internshala Clone: Database Architecture & Design

## Database Architecture

- **Engine**: SQL Server 2022 (Azure SQL Database in production)
- **ORM**: Entity Framework Core 9 (Code-First, Fluent API)
- **Migration Tool**: EF Core Migrations (automated via CI/CD pipeline)
- **Collation**: `SQL_Latin1_General_CP1_CI_AS` (case-insensitive, accent-sensitive)
- **Schemas**: `dbo` (core), `audit` (change tracking), `config` (app configuration)
- **Compatibility Level**: 160 (SQL Server 2022)

---

## ER Diagram Explanation

### Core Entity Relationships

```
Users (base identity)
  ├── 1:1 ──── Students (profile extension for student role)
  ├── 1:1 ──── Employers (profile extension for employer role)
  └── 1:N ──── RefreshTokens (auth tokens)

Employers
  └── 1:N ──── Internships (posted by employer)
  └── 1:N ──── Jobs (posted by employer)

Internships
  ├── N:1 ──── Categories (industry category)
  ├── N:1 ──── Locations (city)
  ├── N:M ──── Skills (required skills, via InternshipSkills junction)
  └── 1:N ──── Applications (student applications)

Jobs
  ├── N:1 ──── Categories
  ├── N:1 ──── Locations
  ├── N:M ──── Skills (via JobSkills junction)
  └── 1:N ──── Applications

Applications
  ├── N:1 ──── Students (who applied)
  ├── N:1 ──── Internships / Jobs (what was applied to)
  └── 1:N ──── ApplicationStatusHistory (audit trail of status changes)

Students
  ├── 1:1 ──── Resumes (resume builder data)
  ├── N:M ──── Skills (via StudentSkills)
  ├── 1:N ──── Educations (education history)
  └── 1:N ──── Experiences (work/internship experience)

Courses
  ├── N:1 ──── Categories
  ├── 1:N ──── Modules (course content modules)
  └── 1:N ──── Enrollments (student enrollments)

Users
  └── 1:N ──── Notifications (in-app notifications)
```

---

## Table Structures

### Base Audit Columns (Applied to all tables)

```sql
[Id]          INT IDENTITY(1,1) NOT NULL,
[CreatedAt]   DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
[UpdatedAt]   DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
[CreatedBy]   INT NULL,           -- FK to Users.Id
[UpdatedBy]   INT NULL,           -- FK to Users.Id
[IsDeleted]   BIT NOT NULL DEFAULT 0,
[RowVersion]  ROWVERSION NOT NULL, -- Optimistic concurrency
CONSTRAINT [PK_{TableName}] PRIMARY KEY CLUSTERED ([Id] ASC)
```

---

### Users Table

```sql
CREATE TABLE [dbo].[Users] (
    [Id]                INT IDENTITY(1,1) NOT NULL,
    [Email]             NVARCHAR(256) NOT NULL,
    [PasswordHash]      NVARCHAR(512) NOT NULL,
    [Role]              NVARCHAR(20) NOT NULL,    -- 'Student', 'Employer', 'Admin'
    [FirstName]         NVARCHAR(100) NOT NULL,
    [LastName]          NVARCHAR(100) NOT NULL,
    [PhoneNumber]       NVARCHAR(15) NULL,
    [ProfilePictureUrl] NVARCHAR(500) NULL,
    [IsEmailVerified]   BIT NOT NULL DEFAULT 0,
    [IsActive]          BIT NOT NULL DEFAULT 1,
    [LastLoginAt]       DATETIME2(7) NULL,
    [EmailVerifiedAt]   DATETIME2(7) NULL,
    [FailedLoginCount]  INT NOT NULL DEFAULT 0,
    [LockoutUntil]      DATETIME2(7) NULL,
    [CreatedAt]         DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]         DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [IsDeleted]         BIT NOT NULL DEFAULT 0,
    [RowVersion]        ROWVERSION NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Users_Email] UNIQUE NONCLUSTERED ([Email]) WHERE [IsDeleted] = 0,
    CONSTRAINT [CHK_Users_Role] CHECK ([Role] IN ('Student', 'Employer', 'Admin'))
);
```

### Students Table

```sql
CREATE TABLE [dbo].[Students] (
    [Id]                    INT IDENTITY(1,1) NOT NULL,
    [UserId]                INT NOT NULL,
    [DateOfBirth]           DATE NULL,
    [Gender]                NVARCHAR(20) NULL,
    [CurrentInstitution]    NVARCHAR(200) NULL,
    [CourseOfStudy]         NVARCHAR(150) NULL,
    [GraduationYear]        SMALLINT NULL,
    [GPA]                   DECIMAL(4,2) NULL,
    [Bio]                   NVARCHAR(1000) NULL,
    [LinkedInUrl]           NVARCHAR(300) NULL,
    [GitHubUrl]             NVARCHAR(300) NULL,
    [PortfolioUrl]          NVARCHAR(300) NULL,
    [PreferredLocations]    NVARCHAR(500) NULL,   -- JSON: ["Bangalore","Mumbai"]
    [IsProfileComplete]     BIT NOT NULL DEFAULT 0,
    [ProfileCompleteness]   TINYINT NOT NULL DEFAULT 0,  -- 0-100
    [CreatedAt]             DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]             DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [IsDeleted]             BIT NOT NULL DEFAULT 0,
    [RowVersion]            ROWVERSION NOT NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Students_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]),
    CONSTRAINT [UQ_Students_UserId] UNIQUE ([UserId]),
    CONSTRAINT [CHK_Students_GPA] CHECK ([GPA] IS NULL OR ([GPA] >= 0 AND [GPA] <= 10))
);
```

### Employers Table

```sql
CREATE TABLE [dbo].[Employers] (
    [Id]                INT IDENTITY(1,1) NOT NULL,
    [UserId]            INT NOT NULL,
    [CompanyName]       NVARCHAR(200) NOT NULL,
    [CompanySize]       NVARCHAR(30) NULL,        -- '1-10', '11-50', '51-200', '201-500', '500+'
    [IndustryId]        INT NOT NULL,
    [CompanyWebsite]    NVARCHAR(300) NULL,
    [CompanyLogoUrl]    NVARCHAR(500) NULL,
    [Description]       NVARCHAR(2000) NULL,
    [Founded]           SMALLINT NULL,
    [HeadquartersCity]  NVARCHAR(100) NULL,
    [HeadquartersState] NVARCHAR(100) NULL,
    [IsVerified]        BIT NOT NULL DEFAULT 0,
    [VerifiedAt]        DATETIME2(7) NULL,
    [LinkedInUrl]       NVARCHAR(300) NULL,
    [CreatedAt]         DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]         DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [IsDeleted]         BIT NOT NULL DEFAULT 0,
    [RowVersion]        ROWVERSION NOT NULL,
    CONSTRAINT [PK_Employers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employers_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]),
    CONSTRAINT [FK_Employers_Categories] FOREIGN KEY ([IndustryId]) REFERENCES [dbo].[Categories]([Id]),
    CONSTRAINT [UQ_Employers_UserId] UNIQUE ([UserId])
);
```

### Internships Table

```sql
CREATE TABLE [dbo].[Internships] (
    [Id]                    INT IDENTITY(1,1) NOT NULL,
    [EmployerId]            INT NOT NULL,
    [CategoryId]            INT NOT NULL,
    [LocationId]            INT NULL,            -- NULL = Work from Home
    [Title]                 NVARCHAR(200) NOT NULL,
    [Description]           NVARCHAR(MAX) NOT NULL,
    [Responsibilities]      NVARCHAR(MAX) NULL,
    [Requirements]          NVARCHAR(MAX) NULL,
    [InternshipType]        NVARCHAR(30) NOT NULL,  -- 'InOffice', 'Remote', 'Hybrid'
    [StipendMin]            INT NULL,
    [StipendMax]            INT NULL,
    [IsPaid]                BIT NOT NULL DEFAULT 1,
    [DurationMonths]        TINYINT NOT NULL,
    [StartDateType]         NVARCHAR(20) NOT NULL,  -- 'Immediate', 'Specific', 'Flexible'
    [StartDate]             DATE NULL,
    [OpeningsCount]         SMALLINT NOT NULL DEFAULT 1,
    [ApplicationDeadline]   DATE NULL,
    [PerksAndBenefits]      NVARCHAR(500) NULL,     -- JSON array
    [Status]                NVARCHAR(20) NOT NULL DEFAULT 'Draft',
    [IsActive]              BIT NOT NULL DEFAULT 0,
    [IsFeatured]            BIT NOT NULL DEFAULT 0,
    [FeaturedUntil]         DATE NULL,
    [ViewsCount]            INT NOT NULL DEFAULT 0,
    [ApplicationsCount]     INT NOT NULL DEFAULT 0, -- Denormalized counter
    [PublishedAt]           DATETIME2(7) NULL,
    [ExpiresAt]             DATETIME2(7) NULL,
    [CreatedAt]             DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]             DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy]             INT NULL,
    [UpdatedBy]             INT NULL,
    [IsDeleted]             BIT NOT NULL DEFAULT 0,
    [RowVersion]            ROWVERSION NOT NULL,
    CONSTRAINT [PK_Internships] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Internships_Employers] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employers]([Id]),
    CONSTRAINT [FK_Internships_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories]([Id]),
    CONSTRAINT [FK_Internships_Locations] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations]([Id]),
    CONSTRAINT [CHK_Internships_Status] CHECK ([Status] IN ('Draft','Active','Closed','Expired','Paused')),
    CONSTRAINT [CHK_Internships_Stipend] CHECK ([StipendMin] IS NULL OR [StipendMax] IS NULL OR [StipendMin] <= [StipendMax])
);
```

### Applications Table

```sql
CREATE TABLE [dbo].[Applications] (
    [Id]                INT IDENTITY(1,1) NOT NULL,
    [StudentId]         INT NOT NULL,
    [ListingType]       NVARCHAR(10) NOT NULL,   -- 'Internship', 'Job'
    [InternshipId]      INT NULL,
    [JobId]             INT NULL,
    [Status]            NVARCHAR(30) NOT NULL DEFAULT 'Applied',
    [CoverLetter]       NVARCHAR(2000) NULL,
    [ResumeUrl]         NVARCHAR(500) NOT NULL,
    [ResumeSnapshotId]  INT NULL,               -- FK to ResumeSnapshots
    [AvailabilityDate]  DATE NULL,
    [ExpectedStipend]   INT NULL,
    [EmployerNote]      NVARCHAR(1000) NULL,    -- Internal employer note
    [RejectionReason]   NVARCHAR(500) NULL,
    [WithdrawnAt]       DATETIME2(7) NULL,
    [CreatedAt]         DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]         DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [IsDeleted]         BIT NOT NULL DEFAULT 0,
    [RowVersion]        ROWVERSION NOT NULL,
    CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Applications_Students] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([Id]),
    CONSTRAINT [FK_Applications_Internships] FOREIGN KEY ([InternshipId]) REFERENCES [dbo].[Internships]([Id]),
    CONSTRAINT [CHK_Applications_Status] CHECK ([Status] IN ('Applied','UnderReview','Shortlisted','InterviewScheduled','Selected','Rejected','Withdrawn')),
    CONSTRAINT [CHK_Applications_ListingRef] CHECK (
        ([ListingType] = 'Internship' AND [InternshipId] IS NOT NULL AND [JobId] IS NULL) OR
        ([ListingType] = 'Job' AND [JobId] IS NOT NULL AND [InternshipId] IS NULL)
    )
);
```

### Additional Core Tables (Abbreviated)

```sql
-- Courses
CREATE TABLE [dbo].[Courses] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Title] NVARCHAR(200) NOT NULL,
    [CategoryId] INT NOT NULL,
    [InstructorId] INT NOT NULL,   -- FK to Users
    [ShortDescription] NVARCHAR(500) NULL,
    [Description] NVARCHAR(MAX) NULL,
    [ThumbnailUrl] NVARCHAR(500) NULL,
    [PriceInr] DECIMAL(10,2) NOT NULL DEFAULT 0,
    [IsFree] BIT NOT NULL DEFAULT 1,
    [DurationHours] SMALLINT NULL,
    [DifficultyLevel] NVARCHAR(20) NOT NULL DEFAULT 'Beginner',
    [CertificateProvided] BIT NOT NULL DEFAULT 1,
    [IsPublished] BIT NOT NULL DEFAULT 0,
    [EnrollmentsCount] INT NOT NULL DEFAULT 0,
    [Rating] DECIMAL(3,2) NULL,
    [ReviewsCount] INT NOT NULL DEFAULT 0,
    -- ... audit columns
);

-- Notifications
CREATE TABLE [dbo].[Notifications] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [Type] NVARCHAR(50) NOT NULL,
    [Title] NVARCHAR(200) NOT NULL,
    [Message] NVARCHAR(1000) NOT NULL,
    [ActionUrl] NVARCHAR(500) NULL,
    [IsRead] BIT NOT NULL DEFAULT 0,
    [ReadAt] DATETIME2(7) NULL,
    [CreatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Notifications_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
);

-- Skills (master table)
CREATE TABLE [dbo].[Skills] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Slug] NVARCHAR(100) NOT NULL,
    [CategoryId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Skills] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Skills_Slug] UNIQUE ([Slug])
);

-- RefreshTokens
CREATE TABLE [dbo].[RefreshTokens] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [TokenHash] NVARCHAR(64) NOT NULL,   -- SHA-256 hash of the token
    [ExpiresAt] DATETIME2(7) NOT NULL,
    [CreatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [RevokedAt] DATETIME2(7) NULL,
    [ReplacedByToken] NVARCHAR(64) NULL,
    [UserAgent] NVARCHAR(300) NULL,
    [IpAddress] NVARCHAR(45) NULL,
    [IsRevoked] AS (CASE WHEN [RevokedAt] IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) PERSISTED,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
);
```

---

## Relationships

| Parent Table | Child Table | Cardinality | FK Column | Cascade |
|---|---|---|---|---|
| Users | Students | 1:1 | Students.UserId | Restrict |
| Users | Employers | 1:1 | Employers.UserId | Restrict |
| Users | RefreshTokens | 1:N | RefreshTokens.UserId | Delete |
| Users | Notifications | 1:N | Notifications.UserId | Delete |
| Employers | Internships | 1:N | Internships.EmployerId | Restrict |
| Employers | Jobs | 1:N | Jobs.EmployerId | Restrict |
| Categories | Internships | 1:N | Internships.CategoryId | Restrict |
| Locations | Internships | 1:N | Internships.LocationId | SetNull |
| Students | Applications | 1:N | Applications.StudentId | Restrict |
| Internships | Applications | 1:N | Applications.InternshipId | Restrict |
| Internships | InternshipSkills | 1:N | InternshipSkills.InternshipId | Cascade |
| Skills | InternshipSkills | 1:N | InternshipSkills.SkillId | Restrict |
| Courses | Enrollments | 1:N | Enrollments.CourseId | Restrict |
| Students | Enrollments | 1:N | Enrollments.StudentId | Restrict |

---

## Constraints

### Business Rule Constraints

```sql
-- Prevent applying to the same internship twice
CREATE UNIQUE INDEX [UQ_Applications_Student_Internship]
ON [dbo].[Applications] ([StudentId], [InternshipId])
WHERE [InternshipId] IS NOT NULL AND [IsDeleted] = 0 AND [Status] <> 'Withdrawn';

-- Prevent enrolling in the same course twice
CREATE UNIQUE INDEX [UQ_Enrollments_Student_Course]
ON [dbo].[Enrollments] ([StudentId], [CourseId])
WHERE [IsDeleted] = 0;

-- Email uniqueness for active users
CREATE UNIQUE INDEX [UQ_Users_Email_Active]
ON [dbo].[Users] ([Email])
WHERE [IsDeleted] = 0;

-- Slug uniqueness for internship profiles (URL)
CREATE UNIQUE INDEX [UQ_Internships_Slug]
ON [dbo].[Internships] ([Slug])
WHERE [IsDeleted] = 0;
```

---

## Indexing Strategy

```sql
-- Users
CREATE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users] ([Email]) WHERE [IsDeleted] = 0;
CREATE NONCLUSTERED INDEX [IX_Users_Role] ON [dbo].[Users] ([Role]) WHERE [IsDeleted] = 0;

-- Internships — Main listing query index
CREATE NONCLUSTERED INDEX [IX_Internships_Listing]
ON [dbo].[Internships] ([IsDeleted], [IsActive], [Status], [CategoryId], [LocationId])
INCLUDE ([Title], [StipendMin], [StipendMax], [DurationMonths], [InternshipType], [ApplicationDeadline], [CreatedAt], [EmployerId])
WHERE [IsDeleted] = 0 AND [IsActive] = 1;

-- Internships — Full-text search
CREATE FULLTEXT INDEX ON [dbo].[Internships] ([Title], [Description])
KEY INDEX [PK_Internships] ON [ft_catalog];

-- Applications — Student view
CREATE NONCLUSTERED INDEX [IX_Applications_StudentId]
ON [dbo].[Applications] ([StudentId], [IsDeleted])
INCLUDE ([Status], [CreatedAt], [InternshipId], [JobId]);

-- Applications — Employer view
CREATE NONCLUSTERED INDEX [IX_Applications_InternshipId]
ON [dbo].[Applications] ([InternshipId], [IsDeleted])
INCLUDE ([StudentId], [Status], [CreatedAt]);

-- Notifications — User inbox
CREATE NONCLUSTERED INDEX [IX_Notifications_User_Unread]
ON [dbo].[Notifications] ([UserId], [IsRead])
INCLUDE ([Type], [Title], [CreatedAt], [ActionUrl])
WHERE [IsDeleted] = 0;

-- RefreshTokens — Token lookup
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_TokenHash]
ON [dbo].[RefreshTokens] ([TokenHash])
WHERE [IsRevoked] = 0;
```

---

## Soft Delete Strategy

All tables use `IsDeleted BIT NOT NULL DEFAULT 0`. Rules:

1. **EF Core Global Filter**: Applied in `OnModelCreating` for each entity:
   ```csharp
   modelBuilder.Entity<Internship>().HasQueryFilter(i => !i.IsDeleted);
   ```
2. **Cascading soft delete**: Deleting an employer soft-deletes their internships (via trigger or service layer).
3. **Unique constraint consideration**: Use filtered unique indexes (`WHERE IsDeleted = 0`) to allow re-registration with deleted email.
4. **Hard delete**: Only for GDPR right-to-erasure requests; implemented in `DataErasureService`.
5. **Audit**: Soft delete sets `UpdatedAt`, `UpdatedBy`, and `IsDeleted = 1`.

---

## Audit Table Strategy

### audit.AuditLogs Table

```sql
CREATE TABLE [audit].[AuditLogs] (
    [Id]            BIGINT IDENTITY(1,1) NOT NULL,
    [TableName]     NVARCHAR(100) NOT NULL,
    [RecordId]      INT NOT NULL,
    [Action]        NVARCHAR(10) NOT NULL,   -- 'INSERT', 'UPDATE', 'DELETE'
    [OldValues]     NVARCHAR(MAX) NULL,      -- JSON: old column values
    [NewValues]     NVARCHAR(MAX) NULL,      -- JSON: new column values
    [ChangedBy]     INT NULL,               -- FK to Users.Id
    [ChangedAt]     DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [IpAddress]     NVARCHAR(45) NULL,
    [UserAgent]     NVARCHAR(300) NULL,
    [CorrelationId] NVARCHAR(36) NULL,
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- Index for looking up history of a specific record
CREATE NONCLUSTERED INDEX [IX_AuditLogs_Table_Record]
ON [audit].[AuditLogs] ([TableName], [RecordId], [ChangedAt] DESC);
```

Audit logging implemented in EF Core via `SaveChangesInterceptor`:

```csharp
public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(...)
    {
        // Capture ChangeTracker entries before save
        var auditEntries = BuildAuditEntries(context.ChangeTracker);
        // After save, write audit records
    }
}
```

---

## Migration Strategy

### Naming Convention

```
{timestamp}_{DescriptiveName}.cs
Example: 20240115120000_InitialCreate.cs
         20240116090000_AddIndexToApplicationsTable.cs
         20240118140000_AddFeaturedColumnToInternships.cs
```

### Migration Workflow

```
1. Developer makes entity changes
2. dotnet ef migrations add {DescriptiveName} --project Internshala.Infrastructure
3. Review generated migration SQL (dotnet ef migrations script)
4. Write Down() migration to make it reversible
5. Commit migration files with the entity changes
6. CI pipeline runs: dotnet ef database update on staging DB
7. Production: migration runs in pre-deployment step (zero-downtime compatible changes only)
```

### Zero-Downtime Migration Rules

- **Safe**: Add nullable column, add index (online), add table.
- **Unsafe (requires maintenance window)**: Rename column, change data type, drop column.
- For unsafe changes: use expand-contract pattern (add new column → backfill → use new column → drop old column across 3 deploys).

---

## Stored Procedures

```sql
-- Employer Dashboard: Get statistics for employer's listings
CREATE OR ALTER PROCEDURE [dbo].[sp_GetEmployerDashboardStats]
    @EmployerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(DISTINCT i.Id)                                    AS TotalInternships,
        COUNT(DISTINCT CASE WHEN i.IsActive = 1 THEN i.Id END) AS ActiveInternships,
        COUNT(DISTINCT a.Id)                                    AS TotalApplications,
        COUNT(DISTINCT CASE WHEN a.Status = 'Shortlisted' THEN a.Id END) AS ShortlistedCount,
        COUNT(DISTINCT CASE WHEN a.Status = 'Selected' THEN a.Id END)    AS SelectedCount,
        SUM(i.ViewsCount)                                       AS TotalViews
    FROM [dbo].[Internships] i
    LEFT JOIN [dbo].[Applications] a ON a.InternshipId = i.Id AND a.IsDeleted = 0
    WHERE i.EmployerId = @EmployerId AND i.IsDeleted = 0;
END;

-- Student: Get application status summary
CREATE OR ALTER PROCEDURE [dbo].[sp_GetStudentApplicationSummary]
    @StudentId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Status,
        COUNT(*) AS Count
    FROM [dbo].[Applications]
    WHERE StudentId = @StudentId AND IsDeleted = 0
    GROUP BY Status;
END;
```

---

## SQL Optimization

### Query Patterns

```sql
-- Listing query: uses covering index IX_Internships_Listing
SELECT
    i.Id, i.Title, i.StipendMin, i.StipendMax, i.DurationMonths,
    i.InternshipType, i.ApplicationDeadline, i.ApplicationsCount,
    e.CompanyName, e.CompanyLogoUrl,
    l.CityName, c.Name AS CategoryName
FROM [dbo].[Internships] i
INNER JOIN [dbo].[Employers] e ON e.Id = i.EmployerId AND e.IsDeleted = 0
LEFT JOIN [dbo].[Locations] l ON l.Id = i.LocationId
INNER JOIN [dbo].[Categories] c ON c.Id = i.CategoryId
WHERE i.IsDeleted = 0
  AND i.IsActive = 1
  AND i.Status = 'Active'
  AND (@CategoryId IS NULL OR i.CategoryId = @CategoryId)
  AND (@LocationId IS NULL OR i.LocationId = @LocationId)
  AND (@StipendMin IS NULL OR i.StipendMin >= @StipendMin)
  AND (@IsRemote IS NULL OR (@IsRemote = 1 AND i.LocationId IS NULL))
ORDER BY i.IsFeatured DESC, i.CreatedAt DESC
OFFSET (@Page - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;

-- Count query for pagination (separate from data query)
SELECT COUNT(*)
FROM [dbo].[Internships] i
WHERE i.IsDeleted = 0 AND i.IsActive = 1 AND i.Status = 'Active'
  AND (@CategoryId IS NULL OR i.CategoryId = @CategoryId);
```

### EF Core Optimization

```csharp
// Query with projection — never fetch full entities for reads
var internships = await _context.Internships
    .AsNoTracking()
    .Where(i => !i.IsDeleted && i.IsActive)
    .Include(i => i.Employer)
    .Include(i => i.Location)
    .Include(i => i.Category)
    .Select(i => new InternshipListDto
    {
        Id = i.Id,
        Title = i.Title,
        CompanyName = i.Employer.CompanyName,
        // ... only what's needed
    })
    .ToListAsync(cancellationToken);
```

---

## Backup Strategy

| Backup Type | Frequency | Retention | Location |
|---|---|---|---|
| Full backup | Daily (2 AM UTC) | 35 days | Azure Blob Storage (RA-GRS) |
| Differential | Every 12 hours | 7 days | Azure Blob Storage |
| Transaction log | Every 15 minutes | 35 days | Azure Blob Storage |
| Point-in-time restore | Any point in 35 days | — | Azure SQL built-in |
| Geo-redundant copy | Continuous | 7 days | Paired Azure region |

### Backup Verification

- Monthly automated restore test to isolated environment.
- RTO (Recovery Time Objective): < 4 hours.
- RPO (Recovery Point Objective): < 15 minutes (transaction log backup interval).

---

## Security Strategy

```sql
-- Principle of least privilege: application uses a restricted user
CREATE LOGIN [internshala_app] WITH PASSWORD = '$(AppPassword)';
CREATE USER [internshala_app] FOR LOGIN [internshala_app];

-- Grant only required permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::[dbo] TO [internshala_app];
GRANT EXECUTE ON [dbo].[sp_GetEmployerDashboardStats] TO [internshala_app];

-- Read-only user for reporting/analytics
CREATE LOGIN [internshala_readonly] WITH PASSWORD = '$(ReadPassword)';
CREATE USER [internshala_readonly] FOR LOGIN [internshala_readonly];
GRANT SELECT ON SCHEMA::[dbo] TO [internshala_readonly];

-- No direct table access for app user to audit schema
DENY SELECT, INSERT, UPDATE, DELETE ON SCHEMA::[audit] TO [internshala_app];
GRANT EXECUTE ON [audit].[sp_WriteAuditLog] TO [internshala_app];
```

### Column-Level Encryption

```sql
-- Sensitive PII encrypted at rest (SQL Server Always Encrypted)
ALTER TABLE [dbo].[Students]
ALTER COLUMN [DateOfBirth] DATE
    ENCRYPTED WITH (
        COLUMN_ENCRYPTION_KEY = [CEK_Students],
        ENCRYPTION_TYPE = Deterministic,
        ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256'
    ) NOT NULL;
```

---

## Transaction Handling

### EF Core Transactions

```csharp
// Multi-step operation: Apply to internship + create notification + increment counter
public async Task<ApplicationDto> ApplyToInternshipAsync(
    CreateApplicationCommand command, CancellationToken ct)
{
    await using var transaction = await _context.Database.BeginTransactionAsync(ct);
    try
    {
        // 1. Create application
        var application = Application.Create(command.StudentId, command.InternshipId, ...);
        _context.Applications.Add(application);

        // 2. Increment applications count (optimistic concurrency safe)
        await _context.Internships
            .Where(i => i.Id == command.InternshipId)
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.ApplicationsCount, i => i.ApplicationsCount + 1), ct);

        // 3. Create notification for employer
        var notification = Notification.Create(employerUserId, NotificationType.NewApplication, ...);
        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return _mapper.Map<ApplicationDto>(application);
    }
    catch
    {
        await transaction.RollbackAsync(ct);
        throw;
    }
}
```

### Isolation Levels

- Default: `READ COMMITTED` (SQL Server default) — prevents dirty reads.
- Report queries: `READ UNCOMMITTED` (WITH NOLOCK) acceptable for approximate counts.
- Financial/critical operations: `SERIALIZABLE` isolation.
- Optimistic concurrency: `RowVersion` column for entity-level conflict detection.

---

## Query Optimization

### Index Maintenance

```sql
-- Weekly: Rebuild heavily fragmented indexes (> 30%)
ALTER INDEX [IX_Internships_Listing] ON [dbo].[Internships] REBUILD
WITH (ONLINE = ON, SORT_IN_TEMPDB = ON);

-- Daily: Reorganize lightly fragmented indexes (5-30%)
ALTER INDEX [IX_Applications_StudentId] ON [dbo].[Applications] REORGANIZE;

-- Update statistics after rebuild
UPDATE STATISTICS [dbo].[Internships] WITH FULLSCAN;
```

### Query Plans

- All complex stored procedures have their execution plans reviewed and pinned using plan guides when necessary.
- Use `SET STATISTICS IO ON` during development to verify index usage.
- Avoid `NOLOCK` on transactional tables — accept `READ COMMITTED SNAPSHOT ISOLATION (RCSI)` instead (enabled at database level).

```sql
-- Enable RCSI to prevent readers from blocking writers
ALTER DATABASE [InternshalaDb] SET READ_COMMITTED_SNAPSHOT ON;
```

### Views for Performance

```sql
-- Pre-joined, filtered view for listing queries
CREATE OR ALTER VIEW [dbo].[vw_ActiveInternships]
WITH SCHEMABINDING
AS
SELECT
    i.Id, i.Title, i.InternshipType, i.StipendMin, i.StipendMax,
    i.DurationMonths, i.ApplicationDeadline, i.ApplicationsCount,
    i.IsFeatured, i.CreatedAt,
    e.CompanyName, e.CompanyLogoUrl, e.IsVerified AS IsCompanyVerified,
    l.CityName AS Location,
    c.Name AS Category, c.Slug AS CategorySlug
FROM [dbo].[Internships] i
INNER JOIN [dbo].[Employers] e ON e.Id = i.EmployerId AND e.IsDeleted = 0
LEFT JOIN [dbo].[Locations] l ON l.Id = i.LocationId AND l.IsActive = 1
INNER JOIN [dbo].[Categories] c ON c.Id = i.CategoryId AND c.IsActive = 1
WHERE i.IsDeleted = 0 AND i.IsActive = 1 AND i.Status = 'Active';
```
