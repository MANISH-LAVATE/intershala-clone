# Architecture.md — Internshala Clone: System Architecture

## High-Level System Architecture

```
┌─────────────────────────────────────────────────────────────────────┐
│                          CLIENT TIER                                 │
│  ┌─────────────────────────────────────────────────────────────┐    │
│  │   Angular 18 SPA (CDN / Vercel / Azure Static Web Apps)     │    │
│  │   PWA-capable, SSR-ready, Mobile-first, WCAG 2.1 AA         │    │
│  └─────────────────────┬───────────────────────────────────────┘    │
└────────────────────────┼────────────────────────────────────────────┘
                         │ HTTPS / HTTP2
┌────────────────────────▼────────────────────────────────────────────┐
│                      API GATEWAY TIER                                │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │   Azure API Management / NGINX Reverse Proxy                 │   │
│  │   Rate Limiting · CORS · SSL Termination · Load Balancing    │   │
│  └──────────────────────┬───────────────────────────────────────┘   │
└─────────────────────────┼───────────────────────────────────────────┘
                          │
┌─────────────────────────▼───────────────────────────────────────────┐
│                      APPLICATION TIER                                │
│  ┌─────────────────┐   ┌─────────────────┐   ┌──────────────────┐  │
│  │  .NET 9 Web API │   │  .NET 9 Web API │   │  Background Jobs │  │
│  │   Instance 1    │   │   Instance 2    │   │  (Hangfire/Worker)│  │
│  └────────┬────────┘   └────────┬────────┘   └────────┬─────────┘  │
└───────────┼──────────────────── ┼ ────────────────────┼────────────┘
            │                     │                     │
┌───────────▼─────────────────────▼─────────────────────▼────────────┐
│                        DATA TIER                                     │
│  ┌──────────────────┐  ┌────────────────┐  ┌──────────────────┐    │
│  │  SQL Server 2022 │  │  Redis Cache   │  │  Azure Blob      │    │
│  │  (Primary)       │  │  (Distributed) │  │  Storage         │    │
│  │  + Read Replica  │  │                │  │  (Files/Media)   │    │
│  └──────────────────┘  └────────────────┘  └──────────────────┘    │
└─────────────────────────────────────────────────────────────────────┘
            │                     │
┌───────────▼─────────────────────▼────────────────────────────────── ┐
│                    OBSERVABILITY TIER                                 │
│   Serilog → Application Insights · Health Checks · Prometheus        │
└──────────────────────────────────────────────────────────────────────┘
```

---

## Frontend Architecture

### Architecture Pattern: Feature-Based Layered Architecture

```
Angular Application
├── Core Layer          — Singleton services, guards, interceptors, app-level config
├── Shared Layer        — Reusable components, pipes, directives, utilities
├── Layout Layer        — Shell: header, footer, sidebar, main-layout
└── Feature Layers      — Self-contained, lazy-loaded feature modules
    ├── Auth Feature
    ├── Internships Feature
    ├── Jobs Feature
    ├── Applications Feature
    ├── Profile Feature
    ├── Employer Feature
    ├── Courses Feature
    └── Admin Feature
```

### Component Architecture

```
Smart Component (Container)
  ├── Injects services directly
  ├── Manages state via signals/stores
  ├── Handles routing/navigation
  └── Passes data to Dumb Components

Dumb Component (Presentational)
  ├── Only input() / output()
  ├── No service injection
  ├── OnPush change detection
  └── Purely reactive to inputs
```

### Data Flow

```
User Action
  → Component event (output() signal)
    → Parent Component or Service
      → HTTP Service (Observable<T>)
        → Angular HttpClient
          → Backend API
            → Response mapped to DTO
              → Signal/Store updated
                → UI re-renders (OnPush)
```

---

## Backend Architecture

### Pattern: Clean Architecture (Onion Architecture)

```
┌─────────────────────────────────────────────────────┐
│  Presentation (Internshala.API)                      │
│  Controllers · Middlewares · Filters · Program.cs    │
├─────────────────────────────────────────────────────┤
│  Application (Internshala.Application)               │
│  Use Cases · DTOs · Validators · Mappers · Commands  │
│  Queries · Interfaces · Behaviors (Pipeline)         │
├─────────────────────────────────────────────────────┤
│  Domain (Internshala.Domain)                         │
│  Entities · Value Objects · Domain Events            │
│  Domain Services · Aggregates · Specifications       │
├─────────────────────────────────────────────────────┤
│  Infrastructure (Internshala.Infrastructure)         │
│  EF Core · Repositories · External APIs              │
│  Email · File Storage · Cache · Background Jobs      │
├─────────────────────────────────────────────────────┤
│  Shared (Internshala.Shared)                         │
│  Exceptions · Constants · Extensions · Pagination    │
└─────────────────────────────────────────────────────┘
```

### Dependency Rule

```
API → Application → Domain
Infrastructure → Application → Domain
Shared ← (used by all layers)
```

No layer references an outer layer. Domain has zero external dependencies.

### Request Pipeline

```
HTTP Request
  → NGINX Reverse Proxy
    → Rate Limiting Middleware
      → CORS Middleware
        → JWT Authentication Middleware
          → Authorization Middleware
            → Request Logging Middleware (CorrelationId)
              → Input Validation (FluentValidation behavior)
                → Controller Action
                  → Application Service / CQRS Handler
                    → Domain Logic
                      → Repository (EF Core)
                        → SQL Server
                      → Cache Layer (Redis)
                  → Response Mapping (AutoMapper)
                → Response Shaping Middleware
              → Exception Handling Middleware
            → Response Logging Middleware
```

---

## Database Architecture

### Schema Organization

```
[dbo] Schema — Core tables
  ├── Users                — All user accounts (polymorphic base)
  ├── Students             — Student-specific profile data
  ├── Employers            — Employer/company profile data
  ├── Internships          — Internship listings
  ├── Jobs                 — Full-time job listings
  ├── Applications         — Student applications to listings
  ├── Courses              — Training courses
  ├── Enrollments          — Course enrollments
  ├── Skills               — Master skill list
  ├── UserSkills           — Student-skill mappings
  ├── Categories           — Industry categories
  ├── Locations            — City/location master data
  ├── Notifications        — In-app notifications
  ├── Resumes              — Resume builder data
  └── Reviews              — Employer/internship reviews

[audit] Schema — Audit trail tables
  ├── AuditLogs            — Change tracking for sensitive tables
  └── LoginHistory         — User login records

[config] Schema — Configuration tables
  ├── EmailTemplates       — Dynamic email templates
  └── AppSettings          — Dynamic app config (feature flags, etc.)
```

### EF Core Strategy

- Code-First with explicit Fluent API configurations.
- One `IEntityTypeConfiguration<T>` file per entity.
- No data annotations on entities (use Fluent API exclusively).
- Migrations stored in `Internshala.Infrastructure/Migrations/`.
- Seeding via `IDbContextSeeder` implementations per environment.

---

## API Architecture

### Versioning Strategy

- URL-based versioning: `/api/v1/`, `/api/v2/`.
- `v1` → current stable; `v2` → next version (parallel).
- Breaking changes always bump version.
- Deprecated versions supported for 6 months post-deprecation announcement.

### Response Envelope

```csharp
public sealed record ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string Message { get; init; } = string.Empty;
    public IReadOnlyList<string> Errors { get; init; } = [];
    public PaginationMeta? Pagination { get; init; }
    public string CorrelationId { get; init; } = string.Empty;
}

public sealed record PaginationMeta
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public bool HasNextPage { get; init; }
    public bool HasPreviousPage { get; init; }
}
```

### Controller Base Pattern

```csharp
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult OkResponse<T>(T data, string message = "Success")
        => Ok(ApiResponse<T>.Ok(data, message));

    protected IActionResult CreatedResponse<T>(string routeName, object routeValues, T data)
        => CreatedAtRoute(routeName, routeValues, ApiResponse<T>.Ok(data, "Created"));

    protected IActionResult PaginatedResponse<T>(PagedResult<T> result)
        => Ok(ApiResponse<IReadOnlyList<T>>.Paginated(result));
}
```

---

## Authentication Flow

### Registration Flow

```
POST /api/v1/auth/register
  → Validate input (FluentValidation)
  → Check email uniqueness
  → Hash password (BCrypt, cost=12)
  → Create User record
  → Create role-specific profile (Student/Employer)
  → Send verification email (background job)
  → Return 201 with user summary
```

### Login Flow

```
POST /api/v1/auth/login
  → Validate input
  → Load user by email
  → Verify password hash
  → Check account status (active, verified)
  → Generate JWT (15min expiry, RS256)
  → Generate Refresh Token (GUID, 7-day expiry, hashed in DB)
  → Store refresh token record (userId, tokenHash, expiry, userAgent, IP)
  → Return: { accessToken, refreshToken, expiresAt, user }
  → Set refresh token in HttpOnly SameSite=Strict cookie
```

### Token Refresh Flow

```
POST /api/v1/auth/refresh
  → Extract refresh token (from cookie or body)
  → Hash token → lookup in DB
  → Validate: not expired, not revoked, matches userId
  → Revoke old refresh token (one-time use)
  → Generate new JWT
  → Generate new Refresh Token (rotation)
  → Return new token pair
```

### JWT Structure

```json
{
  "header": { "alg": "RS256", "typ": "JWT" },
  "payload": {
    "sub": "123",
    "email": "student@example.com",
    "role": "Student",
    "jti": "unique-jwt-id",
    "iat": 1705000000,
    "exp": 1705000900,
    "iss": "internshala-api",
    "aud": "internshala-web"
  }
}
```

---

## Authorization Flow

### Role-Based Access Control (RBAC)

| Role | Access |
|---|---|
| `Guest` | Public listings, course catalog |
| `Student` | Apply to internships/jobs, manage profile, enroll courses |
| `Employer` | Post internships/jobs, manage applications, company profile |
| `Admin` | Full access, user management, content moderation |

### Policy-Based Authorization

```csharp
// Registration in Program.cs
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanPostInternship", policy =>
        policy.RequireRole("Employer", "Admin")
              .RequireClaim("email_verified", "true"));

    options.AddPolicy("CanApplyToInternship", policy =>
        policy.RequireRole("Student")
              .RequireClaim("profile_complete", "true"));
});
```

### Resource-Based Authorization

Employers can only edit/delete their own listings. Students can only manage their own applications. Enforced via `IAuthorizationService` with custom requirements.

---

## Folder Structure

### Angular (`/frontend/src/app/`)

```
app/
├── core/
│   ├── auth/
│   │   ├── services/            auth.service.ts, token.service.ts
│   │   ├── guards/              auth.guard.ts, role.guard.ts
│   │   ├── interceptors/        jwt.interceptor.ts, refresh.interceptor.ts
│   │   └── models/              auth.model.ts
│   ├── services/                api.service.ts, logger.service.ts
│   ├── error-handler/           global-error-handler.ts
│   └── constants/               api-endpoints.constant.ts
│
├── shared/
│   ├── components/
│   │   ├── button/
│   │   ├── badge/
│   │   ├── card/
│   │   ├── data-table/
│   │   ├── empty-state/
│   │   ├── loader/
│   │   ├── confirm-dialog/
│   │   ├── pagination/
│   │   └── search-input/
│   ├── pipes/                   time-ago.pipe.ts, currency-inr.pipe.ts
│   ├── directives/              lazy-image.directive.ts
│   └── models/                  api-response.model.ts, pagination.model.ts
│
├── layout/
│   ├── header/
│   ├── footer/
│   ├── sidebar/
│   ├── main-layout/
│   └── auth-layout/
│
└── features/
    ├── auth/
    │   ├── login/
    │   ├── register/
    │   ├── forgot-password/
    │   └── routes.ts
    ├── internships/
    │   ├── internship-list/
    │   ├── internship-detail/
    │   ├── internship-apply/
    │   ├── services/
    │   ├── models/
    │   └── routes.ts
    ├── jobs/
    ├── applications/
    ├── profile/
    ├── employer/
    │   ├── dashboard/
    │   ├── post-internship/
    │   ├── post-job/
    │   ├── manage-applications/
    │   └── routes.ts
    ├── courses/
    └── admin/
```

### .NET (`/backend/`)

```
backend/
├── Internshala.API/
│   ├── Controllers/
│   │   ├── v1/
│   │   │   ├── AuthController.cs
│   │   │   ├── InternshipsController.cs
│   │   │   ├── JobsController.cs
│   │   │   ├── ApplicationsController.cs
│   │   │   ├── ProfileController.cs
│   │   │   ├── EmployerController.cs
│   │   │   └── CoursesController.cs
│   ├── Middleware/
│   │   ├── ExceptionHandlingMiddleware.cs
│   │   ├── CorrelationIdMiddleware.cs
│   │   └── RequestLoggingMiddleware.cs
│   ├── Filters/
│   │   └── ValidationFilter.cs
│   └── Program.cs
│
├── Internshala.Application/
│   ├── Features/
│   │   ├── Auth/
│   │   │   ├── Commands/         RegisterCommand.cs, LoginCommand.cs
│   │   │   ├── Queries/          GetCurrentUserQuery.cs
│   │   │   └── Validators/       RegisterCommandValidator.cs
│   │   ├── Internships/
│   │   │   ├── Commands/         CreateInternshipCommand.cs
│   │   │   ├── Queries/          GetInternshipsQuery.cs
│   │   │   └── Validators/
│   │   ├── Jobs/
│   │   ├── Applications/
│   │   └── Courses/
│   ├── Common/
│   │   ├── Behaviors/            ValidationBehavior.cs, LoggingBehavior.cs
│   │   ├── Interfaces/           IApplicationDbContext.cs, ICurrentUser.cs
│   │   └── Mappings/             MappingProfile.cs
│   └── DTOs/
│
├── Internshala.Domain/
│   ├── Entities/
│   │   ├── User.cs, Student.cs, Employer.cs
│   │   ├── Internship.cs, Job.cs
│   │   ├── Application.cs
│   │   └── Course.cs
│   ├── Enums/
│   │   ├── ApplicationStatus.cs
│   │   ├── InternshipType.cs
│   │   └── UserRole.cs
│   ├── ValueObjects/             Money.cs, Address.cs
│   └── Events/                   ApplicationSubmittedEvent.cs
│
├── Internshala.Infrastructure/
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs
│   │   ├── Configurations/       InternshipConfiguration.cs
│   │   ├── Migrations/
│   │   ├── Repositories/         InternshipRepository.cs
│   │   └── Seeders/              CategorySeeder.cs
│   ├── Services/
│   │   ├── JwtService.cs
│   │   ├── EmailService.cs
│   │   └── FileStorageService.cs
│   └── DependencyInjection.cs
│
└── Internshala.Shared/
    ├── Exceptions/               NotFoundException.cs, ConflictException.cs
    ├── Extensions/               StringExtensions.cs, QueryableExtensions.cs
    ├── Pagination/               PagedResult.cs, PaginationParams.cs
    └── Constants/                RoleConstants.cs, PolicyConstants.cs
```

---

## Shared Module Strategy

Angular has no NgModules in this project (standalone-only). Sharing is achieved via:

1. **`src/app/shared/`** — reusable, domain-agnostic components, pipes, directives.
2. **`src/app/core/`** — singleton services provided at root level.
3. **`src/app/layout/`** — shell components imported by the app routes.
4. **Direct imports** — standalone components explicitly import what they need.

---

## Lazy Loading Strategy

```typescript
// app.routes.ts
export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', redirectTo: 'internships', pathMatch: 'full' },
      {
        path: 'internships',
        loadChildren: () => import('./features/internships/routes')
          .then(m => m.INTERNSHIP_ROUTES),
        title: 'Internships — Internshala'
      },
      {
        path: 'jobs',
        loadChildren: () => import('./features/jobs/routes')
          .then(m => m.JOB_ROUTES),
        title: 'Jobs — Internshala'
      },
      {
        path: 'courses',
        loadChildren: () => import('./features/courses/routes')
          .then(m => m.COURSE_ROUTES)
      },
      {
        path: 'employer',
        loadChildren: () => import('./features/employer/routes')
          .then(m => m.EMPLOYER_ROUTES),
        canActivate: [() => inject(AuthGuard).canActivate('Employer')]
      },
      {
        path: 'admin',
        loadChildren: () => import('./features/admin/routes')
          .then(m => m.ADMIN_ROUTES),
        canActivate: [() => inject(AuthGuard).canActivate('Admin')]
      }
    ]
  },
  {
    path: 'auth',
    component: AuthLayoutComponent,
    loadChildren: () => import('./features/auth/routes')
      .then(m => m.AUTH_ROUTES)
  }
];
```

---

## State Management Strategy

### Tier 1 — Component-Local State (Angular Signals)

For UI state that doesn't leave the component: loading flags, form state, toggle state.

```typescript
readonly isLoading = signal(false);
readonly showFilters = signal(false);
```

### Tier 2 — Feature-Shared State (Service + Signals)

For state shared between sibling components in a feature:

```typescript
@Injectable({ providedIn: 'root' })
export class InternshipStore {
  private readonly _internships = signal<InternshipDto[]>([]);
  private readonly _loading = signal(false);
  private readonly _filters = signal<InternshipFilters>(defaultFilters);

  readonly internships = this._internships.asReadonly();
  readonly loading = this._loading.asReadonly();
  readonly filters = this._filters.asReadonly();
  readonly filteredInternships = computed(() => this.applyFilters());
}
```

### Tier 3 — Global State (NgRx — if needed)

Reserved for complex cross-feature state (e.g., notification count, user session across features). Evaluate before adopting.

---

## Caching Strategy

### Frontend (Angular)

- **HTTP cache**: Use `HttpClient` with cache interceptor for GET requests (ETag / Last-Modified).
- **Route data cache**: Resolvers cache data in service for 60s before re-fetching.
- **Static data**: Categories, locations fetched once and stored in `BehaviorSubject`.

### Backend (.NET)

- **In-Memory Cache** (`IMemoryCache`): Hot reference data — categories, locations, skills. TTL: 1 hour.
- **Output Cache** (ASP.NET Core): Public GET endpoints for listings. TTL: 60s. Vary by query params.
- **Distributed Cache** (Redis): Session data, token blacklist, rate limiting counters.

```csharp
// Output caching for public listing endpoints
[OutputCache(Duration = 60, VaryByQueryKeys = ["page", "category", "location", "stipend"])]
[HttpGet]
public async Task<IActionResult> GetInternships(...)
```

---

## Logging Strategy

```
Request → CorrelationId assigned (X-Correlation-Id header)
        → RequestLoggingMiddleware: logs method, path, userId
          → Application logs structured events via ILogger<T>
            → Serilog enrichers add: environment, version, machine
              → Serilog sinks:
                  Dev:  Console (colored) + Seq (http://localhost:5341)
                  Staging: App Insights (structured)
                  Prod: App Insights + Azure Log Analytics
        → Response logged with status code and duration
```

### Log Levels by Environment

| Level | Development | Staging | Production |
|---|---|---|---|
| Trace | ✓ | ✗ | ✗ |
| Debug | ✓ | ✗ | ✗ |
| Information | ✓ | ✓ | ✓ |
| Warning | ✓ | ✓ | ✓ |
| Error | ✓ | ✓ | ✓ |
| Critical | ✓ | ✓ | ✓ |

---

## Error Handling Strategy

### Frontend Error Hierarchy

```
GlobalErrorHandler (app-level)
  ├── HttpErrorInterceptor (HTTP errors)
  │   ├── 401 → Redirect to login, clear tokens
  │   ├── 403 → Show forbidden page
  │   ├── 404 → Show not found page
  │   ├── 422 → Map validation errors to form
  │   └── 5xx → Show error toast + log to backend
  └── RouteErrorComponent (router navigation errors)
```

### Backend Error Hierarchy

```
ExceptionHandlingMiddleware
  ├── ValidationException → 400 Bad Request + field errors
  ├── NotFoundException → 404 Not Found
  ├── ConflictException → 409 Conflict
  ├── UnauthorizedException → 401 Unauthorized
  ├── ForbiddenException → 403 Forbidden
  └── Exception (unhandled) → 500 + CorrelationId (no stack trace in prod)
```

---

## Notification Architecture

### In-App Notifications

- Stored in `Notifications` table with `UserId`, `Type`, `Message`, `IsRead`, `CreatedAt`.
- Delivered via SignalR WebSocket connection (authenticated).
- Frontend: `NotificationService` maintains live WebSocket hub connection.
- Badge count updated in real-time; mark-as-read via `PATCH /api/v1/notifications/{id}`.

### Email Notifications

- Triggered by domain events (e.g., `ApplicationSubmittedEvent`, `ApplicationStatusChangedEvent`).
- Background worker processes events from an in-memory queue (or Azure Service Bus in production).
- Email templates stored in database (`config.EmailTemplates`), rendered with Scriban templating.
- Provider: SendGrid (production), Mailhog (development).

---

## Deployment Architecture

```
Developer Machine
  └── git push origin feature/xyz
        ↓
GitHub Repository
  └── Pull Request Created
        ↓
GitHub Actions CI Pipeline
  ├── lint-and-test (Angular)
  ├── build-and-test (.NET)
  └── security-scan (CodeQL)
        ↓ (on merge to develop)
GitHub Actions CD Pipeline — Staging
  ├── Build Docker images
  ├── Push to Azure Container Registry
  └── Deploy to Azure App Service (staging slot)
        ↓ (manual approval gate)
GitHub Actions CD Pipeline — Production
  ├── Swap staging ↔ production slots
  └── Post-deployment health checks
```

### Infrastructure (Azure)

```
Azure Resource Group: rg-internshala-prod
├── Azure App Service Plan (P2v3)
│   ├── internshala-api (App Service)
│   │   ├── Production slot
│   │   └── Staging slot
│   └── internshala-jobs (Worker Service)
├── Azure SQL Database (General Purpose, 4 vCores)
│   └── Geo-redundant backup
├── Azure Cache for Redis (C1 Standard)
├── Azure Blob Storage (ZRS)
├── Azure CDN (for static Angular assets)
├── Azure Container Registry
├── Azure Key Vault (secrets)
├── Azure Application Insights
└── Azure API Management (optional)
```

---

## Scalability Strategy

### Horizontal Scaling

- API tier: stateless; scale out via App Service autoscale rules.
- JWT validation is stateless — no session affinity needed.
- Refresh tokens in database — shared across instances.
- SignalR with Redis backplane for multi-instance support.

### Database Scaling

- Read replica for reporting and analytics queries.
- Table partitioning on `Applications` (by `CreatedAt` year).
- Archival: move applications > 2 years old to `archive` schema.
- Connection pooling: managed by Azure SQL + EF Core.

### CDN Strategy

- All Angular static assets (JS bundles, CSS, images) served from Azure CDN.
- Cache-Control headers: `immutable` for hashed assets, `no-cache` for `index.html`.

---

## Security Architecture

### Defense in Depth

```
Layer 1 (Network): HTTPS everywhere, HSTS, WAF (Azure Front Door)
Layer 2 (API Gateway): Rate limiting, IP allowlisting for admin APIs
Layer 3 (Authentication): JWT RS256, refresh token rotation, MFA (future)
Layer 4 (Authorization): RBAC + resource-based authorization
Layer 5 (Input Validation): FluentValidation, parameterized queries
Layer 6 (Output Encoding): Angular DomSanitizer, Content-Security-Policy
Layer 7 (Secrets): Azure Key Vault, no secrets in code/config files
Layer 8 (Audit): Immutable audit log for sensitive operations
```

### Security Headers (via Middleware)

```
Strict-Transport-Security: max-age=31536000; includeSubDomains
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Content-Security-Policy: default-src 'self'; script-src 'self'
Referrer-Policy: strict-origin-when-cross-origin
Permissions-Policy: camera=(), microphone=(), geolocation=()
```
