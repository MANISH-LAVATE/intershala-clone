using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Internshala.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailVerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: false),
                    LockoutUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanySize = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IndustryId = table.Column<int>(type: "int", nullable: false),
                    CompanyWebsite = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CompanyLogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Founded = table.Column<short>(type: "smallint", nullable: true),
                    HeadquartersCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employers_Categories_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ActionUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CurrentInstitution = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CourseOfStudy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    GraduationYear = table.Column<short>(type: "smallint", nullable: true),
                    Gpa = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    GitHubUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PortfolioUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsProfileComplete = table.Column<bool>(type: "bit", nullable: false),
                    ProfileCompleteness = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Internships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployerId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Responsibilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Requirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternshipType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    StipendMin = table.Column<int>(type: "int", nullable: true),
                    StipendMax = table.Column<int>(type: "int", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    DurationMonths = table.Column<byte>(type: "tinyint", nullable: false),
                    StartDateType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    OpeningsCount = table.Column<short>(type: "smallint", nullable: false),
                    ApplicationDeadline = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    FeaturedUntil = table.Column<DateOnly>(type: "date", nullable: true),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    ApplicationsCount = table.Column<int>(type: "int", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Internships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Internships_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Internships_Employers_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Employers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Internships_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployerId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SalaryMin = table.Column<int>(type: "int", nullable: true),
                    SalaryMax = table.Column<int>(type: "int", nullable: true),
                    ExperienceYearsMin = table.Column<int>(type: "int", nullable: true),
                    ApplicationDeadline = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    ApplicationsCount = table.Column<int>(type: "int", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Employers_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Employers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Institution = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StartYear = table.Column<short>(type: "smallint", nullable: false),
                    EndYear = table.Column<short>(type: "smallint", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    Grade = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiences_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resumes_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternshipSkills",
                columns: table => new
                {
                    InternshipsId = table.Column<int>(type: "int", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipSkills", x => new { x.InternshipsId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_InternshipSkills_Internships_InternshipsId",
                        column: x => x.InternshipsId,
                        principalTable: "Internships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InternshipSkills_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ListingType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    InternshipId = table.Column<int>(type: "int", nullable: true),
                    JobId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CoverLetter = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ResumeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AvailabilityDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpectedStipend = table.Column<int>(type: "int", nullable: true),
                    EmployerNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WithdrawnAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Internships_InternshipId",
                        column: x => x.InternshipId,
                        principalTable: "Internships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobSkills",
                columns: table => new
                {
                    JobsId = table.Column<int>(type: "int", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkills", x => new { x.JobsId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_JobSkills_Jobs_JobsId",
                        column: x => x.JobsId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobSkills_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationStatusHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    FromStatus = table.Column<int>(type: "int", nullable: false),
                    ToStatus = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationStatusHistories_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "IsActive", "IsDeleted", "Name", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3197), true, false, "Engineering & Technology", "engineering-technology", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3206) },
                    { 2, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3215), true, false, "Marketing", "marketing", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3216) },
                    { 3, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3217), true, false, "Business Development", "business-development", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3218) },
                    { 4, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3219), true, false, "Finance & Accounting", "finance-accounting", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3219) },
                    { 5, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3221), true, false, "Human Resources", "human-resources", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3221) },
                    { 6, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3222), true, false, "Design & UX", "design-ux", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3223) },
                    { 7, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3224), true, false, "Content & Journalism", "content-journalism", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3224) },
                    { 8, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3226), true, false, "Data Science & Analytics", "data-science-analytics", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3226) },
                    { 9, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3227), true, false, "Operations", "operations", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3228) },
                    { 10, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3229), true, false, "Sales", "sales", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3229) },
                    { 11, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3230), true, false, "Legal", "legal", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3231) },
                    { 12, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3232), true, false, "Education & Teaching", "education-teaching", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3232) },
                    { 13, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3234), true, false, "Healthcare & Medicine", "healthcare-medicine", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3234) },
                    { 14, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3235), true, false, "Social Media", "social-media", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3236) },
                    { 15, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3239), true, false, "Research", "research", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3250) },
                    { 16, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3251), true, false, "Architecture", "architecture", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3252) },
                    { 17, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3253), true, false, "Event Management", "event-management", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3253) },
                    { 18, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3255), true, false, "Supply Chain", "supply-chain", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3255) },
                    { 19, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3256), true, false, "Media & Entertainment", "media-entertainment", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3256) },
                    { 20, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3258), true, false, "NGO / Social Work", "ngo-social-work", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3258) }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "CityName", "CreatedAt", "IsActive", "IsDeleted", "State", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Bangalore", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3909), true, false, "Karnataka", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3909) },
                    { 2, "Mumbai", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3914), true, false, "Maharashtra", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3914) },
                    { 3, "Delhi", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3916), true, false, "Delhi", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3916) },
                    { 4, "Hyderabad", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3918), true, false, "Telangana", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3918) },
                    { 5, "Chennai", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3919), true, false, "Tamil Nadu", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3920) },
                    { 6, "Pune", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3921), true, false, "Maharashtra", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3921) },
                    { 7, "Kolkata", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3923), true, false, "West Bengal", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3923) },
                    { 8, "Ahmedabad", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3926), true, false, "Gujarat", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3927) },
                    { 9, "Jaipur", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3928), true, false, "Rajasthan", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3928) },
                    { 10, "Lucknow", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3929), true, false, "Uttar Pradesh", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3930) },
                    { 11, "Noida", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3931), true, false, "Uttar Pradesh", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3931) },
                    { 12, "Gurgaon", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3933), true, false, "Haryana", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3933) },
                    { 13, "Kochi", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3934), true, false, "Kerala", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3935) },
                    { 14, "Bhubaneswar", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3936), true, false, "Odisha", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3936) },
                    { 15, "Coimbatore", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3937), true, false, "Tamil Nadu", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3938) },
                    { 16, "Indore", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3939), true, false, "Madhya Pradesh", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3939) },
                    { 17, "Chandigarh", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3941), true, false, "Punjab", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3941) },
                    { 18, "Nagpur", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3942), true, false, "Maharashtra", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3942) },
                    { 19, "Visakhapatnam", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3944), true, false, "Andhra Pradesh", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3944) },
                    { 20, "Surat", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3945), true, false, "Gujarat", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3945) },
                    { 21, "Mysore", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4009), true, false, "Karnataka", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4010) },
                    { 22, "Thiruvananthapuram", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4011), true, false, "Kerala", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4012) },
                    { 23, "Patna", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4013), true, false, "Bihar", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4014) },
                    { 24, "Bhopal", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4015), true, false, "Madhya Pradesh", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4015) },
                    { 25, "Vadodara", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4017), true, false, "Gujarat", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4017) }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "CreatedAt", "IsActive", "IsDeleted", "Name", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4149), true, false, "Python", "python", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4149) },
                    { 2, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4154), true, false, "JavaScript", "javascript", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4154) },
                    { 3, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4156), true, false, "React", "react", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4157) },
                    { 4, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4158), true, false, "Angular", "angular", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4159) },
                    { 5, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4160), true, false, "Node.js", "nodejs", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4161) },
                    { 6, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4162), true, false, "Java", "java", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4163) },
                    { 7, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4164), true, false, "C#", "csharp", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4165) },
                    { 8, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4166), true, false, "SQL", "sql", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4167) },
                    { 9, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4168), true, false, "Machine Learning", "machine-learning", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4169) },
                    { 10, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4170), true, false, "Data Analysis", "data-analysis", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4171) },
                    { 11, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4172), true, false, "Digital Marketing", "digital-marketing", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4173) },
                    { 12, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4174), true, false, "SEO", "seo", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4175) },
                    { 13, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4176), true, false, "Content Writing", "content-writing", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4177) },
                    { 14, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4178), true, false, "Graphic Design", "graphic-design", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4179) },
                    { 15, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4180), true, false, "Figma", "figma", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4181) },
                    { 16, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4182), true, false, "Excel", "excel", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4183) },
                    { 17, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4185), true, false, "Communication", "communication", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4185) },
                    { 18, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4186), true, false, "Leadership", "leadership", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4187) },
                    { 19, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4188), true, false, "Canva", "canva", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4189) },
                    { 20, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4190), true, false, "AWS", "aws", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4191) },
                    { 21, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4192), true, false, "Docker", "docker", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4193) },
                    { 22, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4194), true, false, "Git", "git", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4195) },
                    { 23, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4196), true, false, "Flutter", "flutter", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4197) },
                    { 24, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4198), true, false, "React Native", "react-native", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4198) },
                    { 25, new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4200), true, false, "PowerBI", "powerbi", new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4200) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_InternshipId_IsDeleted",
                table: "Applications",
                columns: new[] { "InternshipId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobId",
                table: "Applications",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_StudentId_InternshipId",
                table: "Applications",
                columns: new[] { "StudentId", "InternshipId" },
                unique: true,
                filter: "[InternshipId] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_StudentId_IsDeleted",
                table: "Applications",
                columns: new[] { "StudentId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStatusHistories_ApplicationId",
                table: "ApplicationStatusHistories",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Educations_StudentId",
                table: "Educations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employers_IndustryId",
                table: "Employers",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_Employers_UserId",
                table: "Employers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_StudentId",
                table: "Experiences",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_CategoryId",
                table: "Internships",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_EmployerId",
                table: "Internships",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_IsDeleted_IsActive_Status_CategoryId_LocationId",
                table: "Internships",
                columns: new[] { "IsDeleted", "IsActive", "Status", "CategoryId", "LocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Internships_LocationId",
                table: "Internships",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipSkills_SkillsId",
                table: "InternshipSkills",
                column: "SkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CategoryId",
                table: "Jobs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_EmployerId",
                table: "Jobs",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IsDeleted_IsActive_Status_CategoryId",
                table: "Jobs",
                columns: new[] { "IsDeleted", "IsActive", "Status", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_LocationId",
                table: "Jobs",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_SkillsId",
                table: "JobSkills",
                column: "SkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CityName",
                table: "Locations",
                column: "CityName");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_RevokedAt",
                table: "RefreshTokens",
                columns: new[] { "UserId", "RevokedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_StudentId",
                table: "Resumes",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Slug",
                table: "Skills",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationStatusHistories");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropTable(
                name: "InternshipSkills");

            migrationBuilder.DropTable(
                name: "JobSkills");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Resumes");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Internships");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Employers");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
