# Skills.md — Internshala Clone: Technology & Skill Inventory

## Angular Concepts Used

### Core Framework

| Concept | Usage in Project |
|---|---|
| Standalone Components | All components (no NgModule). Explicit imports per component. |
| Signal-based Reactivity | Component state (`signal()`), computed values (`computed()`), effects (`effect()`). |
| `input()` / `output()` APIs | Replaces `@Input()` / `@Output()` decorators. Signal-based, type-safe. |
| `inject()` function | DI in functional guards, resolvers, and standalone contexts. |
| Lazy Loading (`loadChildren`, `loadComponent`) | All feature routes lazy-loaded. |
| Route Resolvers | Pre-load internship detail data before component activates. |
| Functional Route Guards | Auth guards using `inject(AuthService)` inside function body. |
| `ChangeDetectionStrategy.OnPush` | All leaf components for performance. |
| Angular Material 18 | Forms, tables, dialogs, snackbars, navigation, date pickers. |
| Angular CDK | Virtual scrolling, drag-and-drop (resume builder), overlay, portal, a11y. |
| `NgOptimizedImage` | All `<img>` tags — lazy loading, priority hints, intrinsic sizing. |
| `AsyncPipe` | Consuming observables in templates without manual subscriptions. |
| `HttpClient` + Interceptors | JWT injection, refresh token logic, error normalization. |
| `FormBuilder` + Reactive Forms | All forms (login, registration, application, profile edit). |
| `FormArray` | Dynamic skill/education/experience form entries. |
| `RouterLink`, `RouterLinkActive` | Navigation; active state for sidebar links. |
| `ActivatedRoute` | Reading route params/query params for filter URLs. |
| `Router` | Programmatic navigation after login/logout/apply. |
| `Title` service | Dynamic page titles for SEO. |
| `Meta` service | Meta tags for Open Graph / SEO. |
| `TransferState` | SSR state hydration (when server-side rendering enabled). |
| Angular Animations | Route transition animations, modal open/close, list stagger. |
| `CdkVirtualScrollViewport` | Long internship lists (1000+ items) without DOM bloat. |

### State Management Patterns

| Pattern | When Used |
|---|---|
| Component Signals | Local UI state (toggle, loading, visibility). |
| Service + Signals (Store Pattern) | Feature-shared state (internship filters, application list). |
| `BehaviorSubject` | Auth state (current user), persistent observables. |
| URL as State | Filter state in query params — shareable, bookmarkable URLs. |
| Route Resolver | Pre-fetch data before navigation. |

### Testing Patterns

| Tool | Purpose |
|---|---|
| Jasmine + Karma | Unit tests for services, pipes, guards. |
| `HttpClientTestingModule` | Mock HTTP in service tests. |
| `ComponentFixture` | Component rendering and interaction tests. |
| Playwright | End-to-end: registration flow, apply flow, employer flow. |
| Spectator | Reduced boilerplate for component testing. |

---

## .NET Concepts Used

### ASP.NET Core 9

| Concept | Usage |
|---|---|
| Minimal API / Controller API | Controller-based API with `ApiController` attribute. |
| Middleware Pipeline | Custom: ExceptionHandling, CorrelationId, RequestLogging, SecurityHeaders. |
| Dependency Injection (built-in) | Services, repositories, options — scoped, transient, singleton lifetimes. |
| `IOptions<T>` pattern | Typed configuration (JwtSettings, EmailSettings, StorageSettings). |
| Action Filters | `ValidateModelStateFilter`, `AuditFilter`. |
| `IActionResult` / `ActionResult<T>` | Typed, documented HTTP responses. |
| `CancellationToken` propagation | All async controller actions pass token to services. |
| Output Caching | Public listing endpoints (60s TTL, vary by query). |
| Response Compression | Brotli + GZip for all JSON responses. |
| Health Checks | `/health` endpoint with DB, Redis, storage checks. |
| Rate Limiting | `AspNetCoreRateLimit`: 5 auth attempts/minute/IP. |
| CORS Policy | Explicit allowed origins; wildcard forbidden in production. |
| API Versioning | URL-based (`/api/v1/`), using `Asp.Versioning.Mvc`. |
| OpenAPI / Swagger | Swashbuckle with XML docs, JWT auth support, response examples. |
| SignalR | Real-time notifications hub (`NotificationHub`). |
| Background Services | `IHostedService` / `BackgroundService` for email and event processing. |

### Clean Architecture / Domain Design

| Concept | Usage |
|---|---|
| Domain Entities | `User`, `Internship`, `Application`, `Course` — pure C# classes. |
| Value Objects | `Money` (amount + currency), `Address`, `DateRange`. |
| Domain Events | `ApplicationSubmittedEvent`, `StatusChangedEvent` — dispatched via `IMediator`. |
| Repository Pattern | `IInternshipRepository`, `IApplicationRepository` — abstracted data access. |
| Unit of Work | `IUnitOfWork` wraps `SaveChangesAsync()` for multi-repo transactions. |
| Specification Pattern | `ActiveInternshipsSpec`, `StudentApplicationsSpec` — reusable query filters. |
| CQRS (via MediatR) | Commands (write) and Queries (read) separated in Application layer. |
| Pipeline Behaviors (MediatR) | `ValidationBehavior`, `LoggingBehavior`, `PerformanceBehavior`. |
| AutoMapper | Entity → DTO → ViewModel projections. |
| FluentValidation | All command/query validators. Rules: not empty, max length, regex, async uniqueness. |
| Result Pattern | `Result<T>` — success/failure without exceptions for expected failures. |

### Security Concepts

| Concept | Usage |
|---|---|
| JWT Authentication | RS256 signed tokens, 15-minute expiry. |
| Refresh Token Rotation | One-time use, 7-day expiry, stored hashed in DB. |
| BCrypt Password Hashing | Cost factor 12, via `BCrypt.Net-Next`. |
| Role-Based Authorization | `[Authorize(Roles = "Student,Employer")]`. |
| Policy-Based Authorization | `[Authorize(Policy = "CanPostInternship")]`. |
| Resource Authorization | `IAuthorizationService` with `ResourceOwnerRequirement`. |
| Data Protection API | Protect/unprotect tokens, cookies. |
| Anti-forgery (XSRF) | For cookie-based auth endpoints. |
| Secret Management | `ISecretManager` abstraction → Azure Key Vault (prod) / user-secrets (dev). |

---

## SQL Server Concepts Used

### Schema Design

| Concept | Usage |
|---|---|
| Normalized schema (3NF) | Main transactional tables avoid redundancy. |
| Denormalization for reads | Separate read-optimized views/stored procs for listing queries. |
| Schema separation | `dbo` (core), `audit` (audit logs), `config` (settings). |
| Soft delete | `IsDeleted BIT DEFAULT 0` on all user-facing tables. |
| Optimistic concurrency | `RowVersion ROWVERSION` column on `Internships`, `Applications`. |
| Audit columns | `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy` on all tables. |

### Indexing Strategy

| Index Type | Applied To |
|---|---|
| Clustered Index | `Id` primary key (all tables). |
| Non-clustered | FK columns (`StudentId`, `InternshipId`, `EmployerId`). |
| Composite | `(IsDeleted, IsActive, CreatedAt DESC)` on `Internships` (listing query). |
| Filtered | `WHERE IsDeleted = 0` on frequently queried tables. |
| Full-text | `Title`, `Description` on `Internships` and `Jobs` (search). |
| Covering | Include frequently projected columns (`Title`, `Company`, `Location`). |

### Advanced SQL Features

| Feature | Usage |
|---|---|
| CTEs (Common Table Expressions) | Hierarchical category queries, paginated queries. |
| Window Functions | Ranking top internships by application count, row numbering for pagination. |
| Stored Procedures | Complex reporting queries (employer dashboard stats). |
| Views | `vw_ActiveInternships` — pre-filtered, pre-joined view for listings. |
| User-Defined Functions | `fn_IsProfileComplete(@userId)` — reused in multiple queries. |
| Transactions | Multi-table inserts (apply to internship: insert Application + Notification). |
| `FOR JSON PATH` | Generate JSON responses directly from SQL (reporting endpoints). |

---

## Authentication Concepts

| Concept | Implementation |
|---|---|
| JWT (JSON Web Tokens) | `System.IdentityModel.Tokens.Jwt` — RS256 asymmetric keys. |
| Refresh Token Strategy | UUID generated, SHA-256 hashed before storage. Rotated on use. |
| Token Blacklist | Redis set of revoked `jti` values — checked in auth middleware. |
| PKCE (future) | Planned for OAuth2 social login (Google, LinkedIn). |
| Email Verification | Time-limited signed token (HMAC-SHA256, 24h expiry). |
| Password Reset | One-time token (GUID, 1h expiry, single-use flag in DB). |
| Account Lockout | 5 failed attempts → 15-minute lockout (tracked in Redis). |
| MFA (future) | TOTP via Google Authenticator planned for employer accounts. |
| OAuth2 Social Login (future) | Google, LinkedIn via `AspNet.Security.OAuth.Providers`. |

---

## Performance Optimization Techniques

### Frontend

| Technique | Applied Where |
|---|---|
| `OnPush` change detection | All components |
| Signal-based reactivity | Eliminates zone.js re-renders |
| Virtual scrolling | Internship list, application list (1000+ items) |
| `NgOptimizedImage` | All images — lazy loading + priority hints |
| Route-level lazy loading | All feature routes |
| Preloading strategy (`PreloadAllModules`) | After initial load — pre-fetch next likely routes |
| HTTP caching (ETag / Cache-Control) | GET requests with `CachingInterceptor` |
| Debounced search (300ms) | All search/filter inputs |
| Bundle optimization | `esbuild` (Angular 17+), tree-shaking, code splitting |
| `trackBy` on `*ngFor` | All list renders |
| `defer` blocks (Angular 17+) | Below-the-fold content on listing pages |

### Backend

| Technique | Applied Where |
|---|---|
| `AsNoTracking()` | All read-only EF Core queries |
| `Select()` projections | Never fetch full entity for DTO queries |
| Eager loading with `.Include()` | Avoid N+1 — only include what's needed |
| Output caching | Public GET listing endpoints (60s) |
| In-memory cache | Category, location, skill master data |
| Compiled queries | Frequent EF Core queries compiled once |
| Bulk operations (EFCore.BulkExtensions) | Bulk status updates for applications |
| Async all the way | All I/O: DB, HTTP, file, email |
| Connection pooling | SQL Server connection pool (min=5, max=100) |
| Response compression | Brotli > GZip on all JSON responses |

---

## Security Implementation Techniques

| Technique | Implementation Detail |
|---|---|
| Input validation | FluentValidation on all commands; Angular Validators on all forms |
| SQL injection prevention | EF Core parameterized queries; no raw SQL string concatenation |
| XSS prevention | Angular `DomSanitizer`, CSP headers, `ng-content` security by default |
| CSRF prevention | `SameSite=Strict` on refresh token cookie; Angular XSRF interceptor |
| Rate limiting | `AspNetCoreRateLimit` — 5 auth/min/IP, 100 API/min/user |
| Security headers | `NWebSec` or custom middleware — HSTS, X-Frame-Options, CSP |
| Secret management | Azure Key Vault in prod; `dotnet user-secrets` in dev |
| File upload security | MIME type validation, file size limits, virus scan (ClamAV or Defender) |
| HTTPS enforcement | `UseHttpsRedirection()`, HSTS max-age=31536000 |
| Password policy | Min 8 chars, 1 uppercase, 1 number, 1 special char (FluentValidation) |
| OWASP Top 10 coverage | Reviewed and mitigated: injection, broken auth, XSS, IDOR, SSRF |

---

## DevOps Skills

| Skill | Implementation |
|---|---|
| Containerization | Docker: multi-stage builds for Angular (Nginx) and .NET API |
| Container orchestration | Docker Compose (dev), Azure App Service (prod) |
| Infrastructure as Code | Azure Bicep / Terraform for Azure resource provisioning |
| Environment management | `appsettings.{env}.json`, `.env.{env}` for Angular, Azure App Config |
| Secret rotation | Azure Key Vault rotation policy + app restart hooks |
| Database migration strategy | EF Core migrations run on startup (dev) / CI pipeline step (prod) |
| Health monitoring | Azure Application Insights, custom `/health` endpoint |
| Alerting | Azure Monitor alerts on error rate > 1%, p95 latency > 500ms |
| Log aggregation | Serilog → Azure Application Insights (structured telemetry) |
| Backup strategy | Azure SQL automated backups (PITR 35 days), geo-redundant |

---

## CI/CD Concepts

| Concept | Implementation |
|---|---|
| Continuous Integration | GitHub Actions — run on every push/PR to any branch |
| Branch protection | `main`/`develop` require passing CI + 1 code review |
| Parallel jobs | Lint, test, build run in parallel in CI |
| Build caching | GitHub Actions cache for `node_modules`, NuGet packages |
| Artifact storage | Angular dist + .NET publish uploaded as workflow artifacts |
| Docker builds | `docker buildx` with multi-platform support |
| Image tagging | `{sha}`, `{branch}`, `{semver}` — never `latest` in production |
| Deployment slots | Azure App Service staging slot → production swap (zero-downtime) |
| Smoke tests | Post-deployment Playwright smoke test against staging URL |
| Rollback | Azure slot swap reversal (< 30 seconds to rollback) |
| Secrets management | GitHub Encrypted Secrets → environment variables in Actions |

---

## Testing Concepts

### Testing Pyramid

```
              /\
             /  \  E2E (Playwright) — 20 critical flows
            /────\
           /      \  Integration — 60% API endpoints, DB repositories
          /────────\
         /          \  Unit — 80%+ Services, Validators, Domain logic
        /────────────\
```

| Test Type | Tools | Coverage Target |
|---|---|---|
| Angular Unit | Jasmine, Karma, Spectator | 60% components, 80% services |
| .NET Unit | xUnit, Moq, FluentAssertions | 85% Application, 70% Domain |
| .NET Integration | WebApplicationFactory, TestContainers | All API endpoints |
| E2E | Playwright | 20 critical user journeys |
| Performance | k6 (load testing) | 500 concurrent users, < 200ms p95 |
| Security | OWASP ZAP (DAST), Trivy (SAST) | Zero high-severity findings |

---

## Architecture Principles

| Principle | How Applied |
|---|---|
| SOLID | Each class has one reason to change; open for extension (strategy pattern); interfaces everywhere |
| DRY | Shared components, base controllers, extension methods, utility functions |
| YAGNI | No speculative features; no unused parameters or abstractions |
| Separation of Concerns | UI / Application / Domain / Infrastructure strictly separated |
| Dependency Inversion | All dependencies on abstractions (interfaces), never on concrete implementations |
| Fail Fast | FluentValidation at API boundary; Angular validators at form boundary |
| Command-Query Separation | MediatR CQRS — commands mutate, queries read — never mixed |
| Repository Pattern | Data access abstracted behind interfaces; business logic never touches EF Core directly |
| DTO Pattern | Entities never exposed in API responses; always mapped through DTOs |
| Specification Pattern | Reusable, composable query logic without leaking to repositories |

---

## Scalability Techniques

| Technique | Applied To |
|---|---|
| Horizontal scaling | Stateless API; Redis for distributed cache and SignalR backplane |
| Database read replicas | Report/analytics queries routed to replica |
| Output caching | Public endpoints cached at API layer |
| CDN for static assets | Angular bundles served from Azure CDN globally |
| Lazy loading (Angular) | Reduces initial bundle size; faster TTI |
| Pagination (all lists) | No endpoint returns unbounded result sets |
| Background jobs (Hangfire/Worker) | Email, notifications, heavy processing off the request thread |
| Message queue (Azure Service Bus) | Decouples event producers from consumers for high-traffic scenarios |
| Table partitioning | `Applications` table partitioned by year for older data archival |

---

## Design Patterns Used

| Pattern | Where Used |
|---|---|
| Repository | `IInternshipRepository`, `IApplicationRepository` |
| Unit of Work | `IUnitOfWork` for multi-repo transactions |
| CQRS | Commands (create/update/delete) and Queries (read) via MediatR |
| Mediator | MediatR decouples controllers from application services |
| Strategy | Multiple `IEmailProvider` implementations; multiple `IStorageProvider` implementations |
| Factory | `INotificationFactory` creates correct notification type from event |
| Observer | Domain events dispatched after aggregate state changes |
| Decorator | MediatR pipeline behaviors (validation, logging, caching, performance) |
| Builder | `InternshipQueryBuilder` for complex dynamic EF Core queries |
| Specification | `ISpecification<T>` for composable query filters |
| Adapter | `IExternalJobBoardAdapter` — wraps third-party job board APIs |
| Singleton | Configuration, `IMemoryCache`, `ILogger` factories |
| Template Method | `BaseEmailTemplate` with abstract `BuildBody()` and `GetSubject()` |
| State | `ApplicationStateMachine` — manages allowed status transitions |
| Null Object | `NullEmailService` for test environments (no real email sent) |
