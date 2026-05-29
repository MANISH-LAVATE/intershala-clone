# FolderStructure.md — Internshala Clone: Enterprise Folder Structure

## Complete Repository Structure

```
internshala-clone/                          ← Root repository
├── .github/
│   ├── workflows/
│   │   ├── frontend-ci.yml
│   │   ├── backend-ci.yml
│   │   ├── security-scan.yml
│   │   ├── deploy-staging.yml
│   │   └── deploy-production.yml
│   ├── ISSUE_TEMPLATE/
│   │   ├── bug_report.md
│   │   └── feature_request.md
│   └── pull_request_template.md
├── frontend/                               ← Angular application
├── backend/                                ← .NET 9 solution
├── infrastructure/                         ← IaC (Bicep/Terraform)
├── docs/                                   ← Project documentation
├── scripts/                                ← Developer utility scripts
├── .husky/                                 ← Git hooks
├── commitlint.config.js
├── .editorconfig
├── .gitignore
├── docker-compose.yml
├── docker-compose.override.yml             ← Dev overrides
├── CLAUDE.md
├── Architecture.md
├── Design.md
├── Skills.md
├── Database.md
├── Task.md
├── GitWorkflow.md
├── CICD.md
├── FolderStructure.md
└── Setup.md
```

---

## Frontend Folder Structure

```
frontend/
├── .angular/                               ← Angular CLI cache (gitignored)
├── dist/                                   ← Build output (gitignored)
├── node_modules/                           ← Dependencies (gitignored)
├── src/
│   ├── app/
│   │   ├── core/                           ← Singleton services, loaded once
│   │   │   ├── auth/
│   │   │   │   ├── guards/
│   │   │   │   │   ├── auth.guard.ts
│   │   │   │   │   └── role.guard.ts
│   │   │   │   ├── interceptors/
│   │   │   │   │   ├── jwt.interceptor.ts
│   │   │   │   │   └── token-refresh.interceptor.ts
│   │   │   │   ├── models/
│   │   │   │   │   ├── auth-user.model.ts
│   │   │   │   │   ├── login-request.model.ts
│   │   │   │   │   └── token-response.model.ts
│   │   │   │   └── services/
│   │   │   │       ├── auth.service.ts
│   │   │   │       └── token-storage.service.ts
│   │   │   ├── error/
│   │   │   │   ├── global-error-handler.ts
│   │   │   │   └── http-error.interceptor.ts
│   │   │   ├── logging/
│   │   │   │   └── logger.service.ts
│   │   │   ├── services/
│   │   │   │   ├── api.service.ts
│   │   │   │   ├── notification-hub.service.ts
│   │   │   │   └── performance-monitoring.service.ts
│   │   │   └── constants/
│   │   │       ├── api-endpoints.constant.ts
│   │   │       └── app.constant.ts
│   │   │
│   │   ├── shared/                         ← Reusable, domain-agnostic
│   │   │   ├── components/
│   │   │   │   ├── badge/
│   │   │   │   │   ├── badge.component.ts
│   │   │   │   │   ├── badge.component.html
│   │   │   │   │   └── badge.component.scss
│   │   │   │   ├── button/
│   │   │   │   │   └── button.component.ts
│   │   │   │   ├── card/
│   │   │   │   │   └── card.component.ts
│   │   │   │   ├── confirm-dialog/
│   │   │   │   │   └── confirm-dialog.component.ts
│   │   │   │   ├── data-table/
│   │   │   │   │   ├── data-table.component.ts
│   │   │   │   │   ├── data-table.component.html
│   │   │   │   │   └── data-table.component.scss
│   │   │   │   ├── empty-state/
│   │   │   │   │   └── empty-state.component.ts
│   │   │   │   ├── error-page/
│   │   │   │   │   └── error-page.component.ts
│   │   │   │   ├── loader/
│   │   │   │   │   ├── spinner.component.ts
│   │   │   │   │   └── skeleton-loader.component.ts
│   │   │   │   ├── pagination/
│   │   │   │   │   └── pagination.component.ts
│   │   │   │   └── search-input/
│   │   │   │       └── search-input.component.ts
│   │   │   ├── directives/
│   │   │   │   ├── lazy-image.directive.ts
│   │   │   │   ├── click-outside.directive.ts
│   │   │   │   └── auto-focus.directive.ts
│   │   │   ├── pipes/
│   │   │   │   ├── time-ago.pipe.ts
│   │   │   │   ├── currency-inr.pipe.ts
│   │   │   │   ├── truncate.pipe.ts
│   │   │   │   └── safe-html.pipe.ts
│   │   │   └── models/
│   │   │       ├── api-response.model.ts
│   │   │       ├── pagination.model.ts
│   │   │       └── select-option.model.ts
│   │   │
│   │   ├── layout/                         ← Shell components
│   │   │   ├── main-layout/
│   │   │   │   ├── main-layout.component.ts
│   │   │   │   ├── main-layout.component.html
│   │   │   │   └── main-layout.component.scss
│   │   │   ├── auth-layout/
│   │   │   │   └── auth-layout.component.ts
│   │   │   ├── header/
│   │   │   │   ├── header.component.ts
│   │   │   │   ├── header.component.html
│   │   │   │   ├── header.component.scss
│   │   │   │   └── notification-dropdown/
│   │   │   │       └── notification-dropdown.component.ts
│   │   │   ├── sidebar/
│   │   │   │   ├── sidebar.component.ts
│   │   │   │   ├── sidebar.component.html
│   │   │   │   └── sidebar.component.scss
│   │   │   └── footer/
│   │   │       ├── footer.component.ts
│   │   │       └── footer.component.scss
│   │   │
│   │   └── features/                       ← Lazy-loaded feature modules
│   │       ├── auth/
│   │       │   ├── login/
│   │       │   │   ├── login.component.ts
│   │       │   │   ├── login.component.html
│   │       │   │   └── login.component.scss
│   │       │   ├── register/
│   │       │   │   ├── register.component.ts
│   │       │   │   ├── register.component.html
│   │       │   │   └── student-register/
│   │       │   │       └── student-register.component.ts
│   │       │   │   └── employer-register/
│   │       │   │       └── employer-register.component.ts
│   │       │   ├── forgot-password/
│   │       │   │   └── forgot-password.component.ts
│   │       │   ├── reset-password/
│   │       │   │   └── reset-password.component.ts
│   │       │   └── routes.ts
│   │       │
│   │       ├── internships/
│   │       │   ├── internship-list/
│   │       │   │   ├── internship-list.component.ts
│   │       │   │   ├── internship-list.component.html
│   │       │   │   └── internship-list.component.scss
│   │       │   ├── internship-detail/
│   │       │   │   ├── internship-detail.component.ts
│   │       │   │   ├── internship-detail.component.html
│   │       │   │   └── internship-detail.component.scss
│   │       │   ├── internship-card/
│   │       │   │   └── internship-card.component.ts
│   │       │   ├── internship-filter/
│   │       │   │   └── internship-filter.component.ts
│   │       │   ├── internship-apply/
│   │       │   │   └── internship-apply.component.ts
│   │       │   ├── resolvers/
│   │       │   │   └── internship-detail.resolver.ts
│   │       │   ├── services/
│   │       │   │   └── internship.service.ts
│   │       │   ├── models/
│   │       │   │   ├── internship.model.ts
│   │       │   │   ├── internship-filter.model.ts
│   │       │   │   └── internship-create.model.ts
│   │       │   ├── store/
│   │       │   │   └── internship.store.ts
│   │       │   └── routes.ts
│   │       │
│   │       ├── jobs/
│   │       │   ├── job-list/
│   │       │   ├── job-detail/
│   │       │   ├── job-card/
│   │       │   ├── services/
│   │       │   ├── models/
│   │       │   └── routes.ts
│   │       │
│   │       ├── applications/
│   │       │   ├── my-applications/
│   │       │   │   ├── my-applications.component.ts
│   │       │   │   └── my-applications.component.html
│   │       │   ├── application-detail/
│   │       │   │   └── application-detail.component.ts
│   │       │   ├── application-status-tracker/
│   │       │   │   └── status-tracker.component.ts
│   │       │   ├── services/
│   │       │   │   └── application.service.ts
│   │       │   ├── models/
│   │       │   │   └── application.model.ts
│   │       │   └── routes.ts
│   │       │
│   │       ├── profile/
│   │       │   ├── student-profile/
│   │       │   │   ├── student-profile.component.ts
│   │       │   │   └── sections/
│   │       │   │       ├── personal-info/
│   │       │   │       ├── education/
│   │       │   │       ├── experience/
│   │       │   │       └── skills/
│   │       │   ├── resume-builder/
│   │       │   │   └── resume-builder.component.ts
│   │       │   ├── services/
│   │       │   │   └── profile.service.ts
│   │       │   ├── models/
│   │       │   │   └── profile.model.ts
│   │       │   └── routes.ts
│   │       │
│   │       ├── employer/
│   │       │   ├── employer-dashboard/
│   │       │   │   └── employer-dashboard.component.ts
│   │       │   ├── post-internship/
│   │       │   │   └── post-internship.component.ts
│   │       │   ├── post-job/
│   │       │   │   └── post-job.component.ts
│   │       │   ├── manage-applications/
│   │       │   │   └── manage-applications.component.ts
│   │       │   ├── company-profile/
│   │       │   │   └── company-profile.component.ts
│   │       │   ├── services/
│   │       │   │   └── employer.service.ts
│   │       │   ├── models/
│   │       │   │   └── employer.model.ts
│   │       │   └── routes.ts
│   │       │
│   │       ├── courses/
│   │       │   ├── course-catalog/
│   │       │   ├── course-detail/
│   │       │   ├── my-courses/
│   │       │   ├── course-player/
│   │       │   ├── services/
│   │       │   ├── models/
│   │       │   └── routes.ts
│   │       │
│   │       └── admin/
│   │           ├── admin-dashboard/
│   │           ├── user-management/
│   │           ├── listing-moderation/
│   │           ├── analytics/
│   │           ├── services/
│   │           └── routes.ts
│   │
│   ├── environments/
│   │   ├── environment.ts
│   │   ├── environment.staging.ts
│   │   └── environment.prod.ts
│   ├── styles/
│   │   ├── _variables.scss             ← SCSS tokens
│   │   ├── _typography.scss
│   │   ├── _breakpoints.scss
│   │   ├── _mixins.scss
│   │   ├── _animations.scss
│   │   ├── _utilities.scss
│   │   ├── _reset.scss
│   │   └── custom-theme.scss           ← Angular Material theme
│   ├── assets/
│   │   ├── icons/                      ← SVG icons
│   │   ├── images/
│   │   │   └── illustrations/          ← Empty state / error SVGs
│   │   └── i18n/                       ← Locale files (future)
│   ├── index.html
│   ├── main.ts
│   └── styles.scss                     ← Global styles entry
│
├── tests/
│   ├── e2e/
│   │   ├── auth/
│   │   │   ├── login.spec.ts
│   │   │   └── register.spec.ts
│   │   ├── internships/
│   │   │   ├── browse.spec.ts
│   │   │   └── apply.spec.ts
│   │   ├── employer/
│   │   │   └── post-internship.spec.ts
│   │   └── smoke/
│   │       └── health.spec.ts
│   └── fixtures/
│       └── test-data.ts
│
├── .eslintrc.json
├── .prettierrc
├── .stylelintrc.json
├── angular.json
├── karma.conf.js
├── playwright.config.ts
├── package.json
├── tsconfig.json
├── tsconfig.app.json
├── tsconfig.spec.json
├── Dockerfile
└── nginx.conf
```

---

## Backend Folder Structure

```
backend/
├── Internshala.sln                         ← Solution file
│
├── Internshala.API/                        ← Presentation layer
│   ├── Controllers/
│   │   └── v1/
│   │       ├── AuthController.cs
│   │       ├── InternshipsController.cs
│   │       ├── JobsController.cs
│   │       ├── ApplicationsController.cs
│   │       ├── ProfileController.cs
│   │       ├── EmployerController.cs
│   │       ├── CoursesController.cs
│   │       ├── NotificationsController.cs
│   │       ├── SearchController.cs
│   │       └── AdminController.cs
│   ├── Middleware/
│   │   ├── ExceptionHandlingMiddleware.cs
│   │   ├── CorrelationIdMiddleware.cs
│   │   ├── RequestLoggingMiddleware.cs
│   │   └── SecurityHeadersMiddleware.cs
│   ├── Filters/
│   │   └── ValidateModelFilter.cs
│   ├── Extensions/
│   │   ├── ServiceCollectionExtensions.cs  ← Register all services
│   │   └── WebApplicationExtensions.cs     ← Configure middleware
│   ├── Hubs/
│   │   └── NotificationHub.cs              ← SignalR hub
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── appsettings.Staging.json
│   ├── appsettings.Production.json
│   ├── Program.cs
│   └── Internshala.API.csproj
│
├── Internshala.Application/                ← Use cases layer
│   ├── Common/
│   │   ├── Behaviors/
│   │   │   ├── ValidationBehavior.cs       ← MediatR: auto-validate commands
│   │   │   ├── LoggingBehavior.cs          ← MediatR: log all requests
│   │   │   └── PerformanceBehavior.cs      ← MediatR: warn on slow queries
│   │   ├── Interfaces/
│   │   │   ├── IApplicationDbContext.cs
│   │   │   ├── ICurrentUserService.cs
│   │   │   ├── IEmailService.cs
│   │   │   ├── IFileStorageService.cs
│   │   │   ├── ICacheService.cs
│   │   │   └── INotificationService.cs
│   │   ├── Mappings/
│   │   │   └── MappingProfile.cs           ← AutoMapper profile
│   │   └── Models/
│   │       └── ApiResponse.cs
│   ├── Features/
│   │   ├── Auth/
│   │   │   ├── Commands/
│   │   │   │   ├── RegisterStudentCommand.cs
│   │   │   │   ├── RegisterStudentCommandHandler.cs
│   │   │   │   ├── RegisterEmployerCommand.cs
│   │   │   │   ├── RegisterEmployerCommandHandler.cs
│   │   │   │   ├── LoginCommand.cs
│   │   │   │   ├── LoginCommandHandler.cs
│   │   │   │   ├── RefreshTokenCommand.cs
│   │   │   │   ├── RefreshTokenCommandHandler.cs
│   │   │   │   └── RevokeTokenCommand.cs
│   │   │   ├── Queries/
│   │   │   │   └── GetCurrentUserQuery.cs
│   │   │   └── Validators/
│   │   │       ├── RegisterStudentCommandValidator.cs
│   │   │       ├── RegisterEmployerCommandValidator.cs
│   │   │       └── LoginCommandValidator.cs
│   │   ├── Internships/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateInternshipCommand.cs
│   │   │   │   ├── CreateInternshipCommandHandler.cs
│   │   │   │   ├── UpdateInternshipCommand.cs
│   │   │   │   ├── UpdateInternshipCommandHandler.cs
│   │   │   │   ├── PublishInternshipCommand.cs
│   │   │   │   └── DeleteInternshipCommand.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetInternshipsQuery.cs
│   │   │   │   ├── GetInternshipsQueryHandler.cs
│   │   │   │   ├── GetInternshipByIdQuery.cs
│   │   │   │   └── GetInternshipByIdQueryHandler.cs
│   │   │   ├── Validators/
│   │   │   │   └── CreateInternshipCommandValidator.cs
│   │   │   └── DTOs/
│   │   │       ├── InternshipDto.cs
│   │   │       ├── InternshipListDto.cs
│   │   │       ├── CreateInternshipRequest.cs
│   │   │       └── InternshipFilterParams.cs
│   │   ├── Jobs/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   ├── Validators/
│   │   │   └── DTOs/
│   │   ├── Applications/
│   │   │   ├── Commands/
│   │   │   │   ├── SubmitApplicationCommand.cs
│   │   │   │   ├── UpdateApplicationStatusCommand.cs
│   │   │   │   └── WithdrawApplicationCommand.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetStudentApplicationsQuery.cs
│   │   │   │   └── GetInternshipApplicationsQuery.cs
│   │   │   └── DTOs/
│   │   │       └── ApplicationDto.cs
│   │   ├── Profile/
│   │   │   ├── Commands/
│   │   │   │   ├── UpdateStudentProfileCommand.cs
│   │   │   │   ├── AddEducationCommand.cs
│   │   │   │   └── AddExperienceCommand.cs
│   │   │   ├── Queries/
│   │   │   │   └── GetStudentProfileQuery.cs
│   │   │   └── DTOs/
│   │   │       └── StudentProfileDto.cs
│   │   ├── Courses/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   └── DTOs/
│   │   └── Notifications/
│   │       ├── Queries/
│   │       │   └── GetUserNotificationsQuery.cs
│   │       └── Commands/
│   │           └── MarkNotificationReadCommand.cs
│   └── Internshala.Application.csproj
│
├── Internshala.Domain/                     ← Domain layer (zero dependencies)
│   ├── Common/
│   │   ├── BaseEntity.cs                   ← Id, CreatedAt, UpdatedAt, IsDeleted
│   │   ├── AuditableEntity.cs              ← + CreatedBy, UpdatedBy, RowVersion
│   │   └── DomainEvent.cs
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Student.cs
│   │   ├── Employer.cs
│   │   ├── Internship.cs
│   │   ├── Job.cs
│   │   ├── Application.cs
│   │   ├── ApplicationStatusHistory.cs
│   │   ├── Course.cs
│   │   ├── CourseModule.cs
│   │   ├── Enrollment.cs
│   │   ├── Notification.cs
│   │   ├── RefreshToken.cs
│   │   ├── Skill.cs
│   │   ├── Category.cs
│   │   ├── Location.cs
│   │   ├── Education.cs
│   │   └── Experience.cs
│   ├── Enums/
│   │   ├── UserRole.cs
│   │   ├── ApplicationStatus.cs
│   │   ├── InternshipType.cs
│   │   ├── InternshipStatus.cs
│   │   ├── NotificationType.cs
│   │   └── DifficultyLevel.cs
│   ├── ValueObjects/
│   │   ├── Money.cs
│   │   ├── Address.cs
│   │   └── DateRange.cs
│   ├── Events/
│   │   ├── ApplicationSubmittedEvent.cs
│   │   ├── ApplicationStatusChangedEvent.cs
│   │   └── InternshipPublishedEvent.cs
│   └── Internshala.Domain.csproj
│
├── Internshala.Infrastructure/             ← Infrastructure layer
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs
│   │   ├── ApplicationDbContextFactory.cs  ← For EF migrations in CLI
│   │   ├── Configurations/                 ← IEntityTypeConfiguration per entity
│   │   │   ├── UserConfiguration.cs
│   │   │   ├── StudentConfiguration.cs
│   │   │   ├── EmployerConfiguration.cs
│   │   │   ├── InternshipConfiguration.cs
│   │   │   ├── JobConfiguration.cs
│   │   │   ├── ApplicationConfiguration.cs
│   │   │   ├── CourseConfiguration.cs
│   │   │   └── NotificationConfiguration.cs
│   │   ├── Interceptors/
│   │   │   ├── AuditSaveChangesInterceptor.cs
│   │   │   └── SoftDeleteInterceptor.cs
│   │   ├── Migrations/
│   │   │   ├── 20240115120000_InitialCreate.cs
│   │   │   └── ...
│   │   ├── Repositories/
│   │   │   ├── BaseRepository.cs
│   │   │   ├── InternshipRepository.cs
│   │   │   ├── JobRepository.cs
│   │   │   ├── ApplicationRepository.cs
│   │   │   ├── StudentRepository.cs
│   │   │   ├── EmployerRepository.cs
│   │   │   ├── CourseRepository.cs
│   │   │   └── UserRepository.cs
│   │   ├── Seeders/
│   │   │   ├── IDbContextSeeder.cs
│   │   │   ├── CategorySeeder.cs
│   │   │   ├── LocationSeeder.cs
│   │   │   └── SkillSeeder.cs
│   │   └── UnitOfWork.cs
│   ├── Services/
│   │   ├── JwtService.cs
│   │   ├── EmailService.cs
│   │   ├── FileStorageService.cs
│   │   ├── CacheService.cs
│   │   └── NotificationService.cs
│   ├── BackgroundJobs/
│   │   ├── EmailDispatchWorker.cs
│   │   └── ExpiredInternshipCleanupWorker.cs
│   ├── DependencyInjection.cs              ← Register all infrastructure services
│   └── Internshala.Infrastructure.csproj
│
├── Internshala.Shared/                     ← Cross-cutting utilities
│   ├── Exceptions/
│   │   ├── NotFoundException.cs
│   │   ├── ConflictException.cs
│   │   ├── ForbiddenException.cs
│   │   ├── UnauthorizedException.cs
│   │   └── ValidationException.cs
│   ├── Extensions/
│   │   ├── StringExtensions.cs
│   │   ├── QueryableExtensions.cs
│   │   └── ClaimsPrincipalExtensions.cs
│   ├── Pagination/
│   │   ├── PagedResult.cs
│   │   └── PaginationParams.cs
│   ├── Result/
│   │   └── Result.cs                       ← Result<T> pattern
│   ├── Constants/
│   │   ├── RoleConstants.cs
│   │   ├── PolicyConstants.cs
│   │   └── CacheKeys.cs
│   └── Internshala.Shared.csproj
│
├── tests/
│   ├── Internshala.Domain.Tests/
│   │   ├── Entities/
│   │   │   ├── InternshipTests.cs
│   │   │   └── ApplicationTests.cs
│   │   └── Internshala.Domain.Tests.csproj
│   ├── Internshala.Application.Tests/
│   │   ├── Features/
│   │   │   ├── Auth/
│   │   │   │   └── LoginCommandHandlerTests.cs
│   │   │   ├── Internships/
│   │   │   │   └── CreateInternshipCommandHandlerTests.cs
│   │   │   └── Applications/
│   │   │       └── SubmitApplicationCommandHandlerTests.cs
│   │   └── Internshala.Application.Tests.csproj
│   └── Internshala.IntegrationTests/
│       ├── Controllers/
│       │   ├── AuthControllerTests.cs
│       │   ├── InternshipsControllerTests.cs
│       │   └── ApplicationsControllerTests.cs
│       ├── TestBase.cs
│       ├── TestWebApplicationFactory.cs
│       └── Internshala.IntegrationTests.csproj
│
└── Dockerfile
```

---

## Shared Library Structure

```
Internshala.Shared/
├── Exceptions/
│   ├── AppException.cs          ← Base class with StatusCode, Message
│   ├── NotFoundException.cs     ← HTTP 404
│   ├── ConflictException.cs     ← HTTP 409
│   ├── ForbiddenException.cs    ← HTTP 403
│   ├── UnauthorizedException.cs ← HTTP 401
│   └── ValidationException.cs  ← HTTP 422 with field errors
├── Result/
│   └── Result.cs
│   ── Result.Generic.cs         ← Result<T>
│   └── Error.cs
├── Pagination/
│   ├── PaginationParams.cs
│   └── PagedResult.cs
├── Extensions/
│   ├── StringExtensions.cs
│   ├── DateTimeExtensions.cs
│   ├── QueryableExtensions.cs   ← .ApplyPagination(), .ApplySorting()
│   └── ClaimsPrincipalExtensions.cs
└── Constants/
    ├── RoleConstants.cs
    ├── PolicyConstants.cs
    ├── CacheKeys.cs
    └── RegexPatterns.cs
```

---

## API Module Structure (Per Feature)

Every feature in `Internshala.Application/Features/{Feature}/` follows this structure:

```
Features/{Feature}/
├── Commands/
│   ├── Create{Feature}Command.cs          ← IRequest<{Feature}Dto>
│   ├── Create{Feature}CommandHandler.cs   ← IRequestHandler<>
│   ├── Update{Feature}Command.cs
│   ├── Update{Feature}CommandHandler.cs
│   └── Delete{Feature}Command.cs
├── Queries/
│   ├── Get{Feature}sQuery.cs              ← Paginated list
│   ├── Get{Feature}sQueryHandler.cs
│   ├── Get{Feature}ByIdQuery.cs
│   └── Get{Feature}ByIdQueryHandler.cs
├── Validators/
│   ├── Create{Feature}CommandValidator.cs ← FluentValidation
│   └── Update{Feature}CommandValidator.cs
└── DTOs/
    ├── {Feature}Dto.cs                    ← Response DTO
    ├── {Feature}ListDto.cs                ← Lightweight list DTO
    ├── Create{Feature}Request.cs          ← Request DTO
    └── Update{Feature}Request.cs
```

---

## Database Project Structure

```
database/
├── migrations/                 ← Manual SQL scripts (for reference)
│   ├── v1.0.0/
│   │   ├── 001_create_schema.sql
│   │   ├── 002_create_users_table.sql
│   │   └── ...
│   └── v1.1.0/
│       └── 001_add_featured_column.sql
├── seed/
│   ├── categories.sql
│   ├── locations.sql
│   └── skills.sql
├── procedures/
│   ├── sp_GetEmployerDashboardStats.sql
│   └── sp_GetStudentApplicationSummary.sql
├── views/
│   └── vw_ActiveInternships.sql
├── indexes/
│   └── create_indexes.sql
└── schema.dbml                 ← DB diagram source (dbdiagram.io)
```

---

## CI/CD Structure

```
.github/
├── workflows/
│   ├── frontend-ci.yml         ← PR: lint, test, build Angular
│   ├── backend-ci.yml          ← PR: build, test .NET, run migrations
│   ├── security-scan.yml       ← PR: CodeQL, Trivy, OWASP
│   ├── deploy-staging.yml      ← develop push: Docker → ACR → Azure staging
│   └── deploy-production.yml   ← main push + approval: slot swap
├── ISSUE_TEMPLATE/
│   ├── bug_report.md
│   └── feature_request.md
└── pull_request_template.md
```

---

## Deployment Structure

```
infrastructure/
├── azure/
│   ├── main.bicep              ← Entry point for all Azure resources
│   ├── modules/
│   │   ├── app-service.bicep
│   │   ├── sql-database.bicep
│   │   ├── redis.bicep
│   │   ├── blob-storage.bicep
│   │   ├── key-vault.bicep
│   │   ├── app-insights.bicep
│   │   └── cdn.bicep
│   └── parameters/
│       ├── staging.bicepparam
│       └── production.bicepparam
├── docker/
│   ├── docker-compose.yml          ← Dev full stack
│   ├── docker-compose.test.yml     ← Integration test stack
│   └── docker-compose.prod.yml     ← Production reference
└── scripts/
    ├── provision-azure.sh       ← Run Bicep deployment
    ├── setup-keyvault.sh        ← Populate Key Vault secrets
    └── rotate-jwt-keys.sh       ← Generate new RSA key pair
```
