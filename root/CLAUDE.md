# CLAUDE.md — Internshala Clone: AI Coding Rules & Development Standards

## Project Overview

**Internshala Clone** is a full-stack, production-grade internship and job portal built with Angular (frontend), .NET 9 Web API (backend), and SQL Server (database). It replicates and extends Internshala's core features: internship/job listings, student profiles, employer dashboards, application tracking, resume builder, and learning courses.

- **Frontend**: Angular 18+ (Standalone Components, Signals, SSR-ready)
- **Backend**: .NET 9 Web API (Clean Architecture, CQRS-ready)
- **Database**: SQL Server 2022 (EF Core 9, Code-First Migrations)
- **Auth**: JWT + Refresh Tokens + Role-Based Access Control
- **UI**: Angular Material 18 + Custom SCSS Design System
- **CI/CD**: GitHub Actions (lint → test → build → deploy)

---

## AI Coding Rules

These rules govern all AI-assisted code generation in this project.

### Non-Negotiable Rules

1. **Never generate mock data** — all data flows through real services and real database queries.
2. **Never use `any` in TypeScript** — use explicit types, generics, or `unknown` with type guards.
3. **Never skip error handling** — every HTTP call, DB query, and async operation must handle failure.
4. **Never hardcode secrets** — use environment variables, `appsettings.{env}.json`, or Secret Manager.
5. **Never generate TODO comments** — implement it now or create a tracked GitHub Issue.
6. **Never create barrel files that re-export everything** — export only what consumers actually need.
7. **Never use `console.log` in production code** — use the `LoggerService` (Angular) or `ILogger<T>` (.NET).
8. **Never bypass FluentValidation** — every API endpoint input must be validated.
9. **Never use `SELECT *`** — always specify columns in SQL queries.
10. **Never commit directly to `main` or `develop`** — all changes go through Pull Requests.

### Code Quality Rules

- All generated code must compile with zero TypeScript errors (`tsc --noEmit`).
- All generated code must pass ESLint with zero warnings (`ng lint`).
- All generated .NET code must compile with zero errors and zero warnings (`dotnet build -warnaserror`).
- All public APIs must have XML documentation comments.
- All Angular components must be standalone unless explicitly inside a legacy NgModule.
- All Angular services must be `providedIn: 'root'` unless feature-scoped.
- All HTTP interceptors must be functional interceptors (not class-based).

---

## Development Standards

### General Principles

- **SOLID** — Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion.
- **DRY** — Don't Repeat Yourself; extract shared logic into services, utilities, or shared components.
- **YAGNI** — You Aren't Gonna Need It; do not build features speculatively.
- **Fail Fast** — validate inputs at system boundaries; reject invalid data early.
- **Separation of Concerns** — UI, business logic, data access, and infrastructure are strictly separated.

### Code Style

- Indentation: 2 spaces (TypeScript/HTML/SCSS), 4 spaces (C#).
- Max line length: 120 characters.
- Single quotes in TypeScript; double quotes in C# strings.
- Trailing commas in TypeScript arrays and objects (where valid).
- Semicolons required in TypeScript.
- No trailing whitespace.
- UTF-8 encoding for all files.
- LF line endings (enforced by `.editorconfig`).

---

## Angular Coding Standards

### Component Rules

```typescript
// CORRECT: Standalone component with explicit imports
@Component({
  selector: 'app-internship-card',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule],
  templateUrl: './internship-card.component.html',
  styleUrl: './internship-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InternshipCardComponent {
  // Use signals for reactive state
  readonly internship = input.required<InternshipDto>();
  readonly applyClicked = output<number>();
}
```

- Always use `ChangeDetectionStrategy.OnPush`.
- Always use `input()` and `output()` signal-based APIs (Angular 17+).
- Never use `ngOnChanges` for simple input reactions — use `effect()` or computed signals.
- Component files must not exceed 200 lines; split into child components if needed.
- Template expressions must be pure (no side effects).
- Use `trackBy` on all `*ngFor` directives.
- Use `async` pipe for observable subscriptions in templates.

### Service Rules

- Services handle all HTTP calls, state, and business logic.
- Use `HttpClient` with typed generics: `this.http.get<ApiResponse<InternshipDto[]>>(url)`.
- Every service method that calls an API must return `Observable<T>`, not `Promise<T>`.
- Use `catchError` and re-throw as domain-specific errors.
- Use `EMPTY` or `of(defaultValue)` for safe fallbacks, never suppress errors silently.

### Routing Rules

- All feature modules use lazy loading via `loadComponent` or `loadChildren`.
- Route guards use functional guard pattern (`inject()` inside guard functions).
- Route resolvers pre-load data before component activation.
- All routes have `title` property for SEO and browser tab accuracy.

### State Management Rules

- Use Angular Signals for component-local state.
- Use a service with `BehaviorSubject` / `signal` for shared state.
- Use `NgRx` only if state complexity justifies it (cross-feature, time-travel debug needs).
- State mutations happen only inside services, never in components.

### SCSS Rules

- Use BEM naming: `.internship-card__title`, `.internship-card--featured`.
- Use CSS custom properties for theming (defined in `_variables.scss`).
- Never use inline styles in templates.
- Never use `!important` except in utility classes.
- Mobile-first: all media queries use `min-width`.
- All colors must reference SCSS variables, never raw hex in component files.

### File Naming Conventions (Angular)

| Type | Convention | Example |
|---|---|---|
| Component | `kebab-case.component.ts` | `internship-card.component.ts` |
| Service | `kebab-case.service.ts` | `internship.service.ts` |
| Guard | `kebab-case.guard.ts` | `auth.guard.ts` |
| Interceptor | `kebab-case.interceptor.ts` | `jwt.interceptor.ts` |
| Pipe | `kebab-case.pipe.ts` | `time-ago.pipe.ts` |
| Directive | `kebab-case.directive.ts` | `lazy-image.directive.ts` |
| Model/DTO | `kebab-case.model.ts` | `internship.model.ts` |
| Enum | `kebab-case.enum.ts` | `application-status.enum.ts` |
| Constant | `kebab-case.constant.ts` | `api-endpoints.constant.ts` |
| Store | `kebab-case.store.ts` | `internship.store.ts` |

---

## .NET Coding Standards

### Naming Conventions (C#)

| Element | Convention | Example |
|---|---|---|
| Class | PascalCase | `InternshipService` |
| Interface | I + PascalCase | `IInternshipRepository` |
| Method | PascalCase | `GetActiveInternshipsAsync` |
| Parameter | camelCase | `internshipId` |
| Private field | `_camelCase` | `_repository` |
| Constant | PascalCase | `MaxApplicationsPerDay` |
| Enum | PascalCase | `ApplicationStatus` |
| Enum member | PascalCase | `UnderReview` |
| DTO | PascalCase + `Dto` | `InternshipDto` |
| Command | PascalCase + `Command` | `CreateInternshipCommand` |
| Query | PascalCase + `Query` | `GetInternshipsQuery` |
| Controller | PascalCase + `Controller` | `InternshipsController` |
| Repository | PascalCase + `Repository` | `InternshipRepository` |

### Controller Rules

- Controllers are thin — no business logic, only routing and response shaping.
- All action methods are `async` and return `Task<IActionResult>` or `Task<ActionResult<T>>`.
- Use `[ProducesResponseType]` attributes on all action methods.
- Use `[Authorize]` with explicit roles where required.
- Use `[ApiController]` attribute on all controllers.
- Route prefix: `api/v1/[controller]` (versioned from day one).

### Service/Repository Rules

- All repository methods are async with `CancellationToken` parameter.
- Services use constructor injection only (no property injection).
- Business logic lives in domain services, not repositories.
- Repositories handle only CRUD operations; no business logic.
- Use `IUnitOfWork` for multi-repository transactions.

### Error Handling Rules

- Use a global exception middleware — never try/catch at the controller level for generic exceptions.
- Define custom domain exceptions: `NotFoundException`, `ConflictException`, `ForbiddenException`.
- Return RFC 7807 `ProblemDetails` for all error responses.
- Log exceptions with correlation IDs at the `Error` level.

### Async Rules

- All I/O-bound methods must be async with `Async` suffix.
- Always pass and use `CancellationToken`.
- Never use `.Result` or `.Wait()` — always `await`.
- Use `ConfigureAwait(false)` in library code; omit in ASP.NET Core (SynchronizationContext is null).

---

## Database Standards

- Table names: PascalCase, plural (e.g., `Internships`, `Applications`).
- Column names: PascalCase (e.g., `CreatedAt`, `IsActive`).
- Primary keys: `Id` (int or Guid — prefer `int` for FK-heavy tables, `Guid` for distributed scenarios).
- Foreign keys: `{EntityName}Id` (e.g., `InternshipId`, `StudentId`).
- All tables must have: `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy`, `IsDeleted` (soft delete), `RowVersion` (optimistic concurrency).
- All string columns have `MaxLength` attributes.
- All date columns use `datetime2` type.
- Use database indexes on all FK columns and frequently-queried columns.
- Never use EF Core lazy loading — always explicit `.Include()`.
- All migrations are named descriptively: `AddIndexToApplicationsTable`, not `Migration1`.

---

## Naming Conventions

### API Endpoints

```
GET    /api/v1/internships              — list/search
GET    /api/v1/internships/{id}         — get by id
POST   /api/v1/internships              — create
PUT    /api/v1/internships/{id}         — full update
PATCH  /api/v1/internships/{id}         — partial update
DELETE /api/v1/internships/{id}         — soft delete
GET    /api/v1/internships/{id}/applications — nested resource
POST   /api/v1/internships/{id}/apply   — action
```

### Angular Feature Folders

Each feature follows: `features/{feature-name}/{component|service|store|model|routes}`.

### .NET Project Naming

- `Internshala.API` — presentation layer
- `Internshala.Application` — application services, DTOs, CQRS
- `Internshala.Domain` — entities, value objects, domain services
- `Internshala.Infrastructure` — EF Core, repositories, external services
- `Internshala.Shared` — cross-cutting: exceptions, constants, utilities

---

## Folder Structure Rules

- Angular: `src/app/features/{feature}/` for feature-specific code.
- Angular: `src/app/core/` for singleton services, guards, interceptors.
- Angular: `src/app/shared/` for reusable components, pipes, directives.
- Angular: `src/app/layout/` for shell components (header, sidebar, footer).
- .NET: One project per architectural layer (Domain, Application, Infrastructure, API).
- .NET: One folder per feature within Application layer (e.g., `Features/Internships/`).
- Never mix features in the same folder.
- Never put infrastructure code in Domain layer.
- Never import Application from Infrastructure (direction: API → Application → Domain ← Infrastructure).

---

## Reusable Component Strategy

All shared UI components live in `src/app/shared/components/` and follow these rules:

- They have no knowledge of routing or application state.
- They receive data via `@Input()` / `input()` and emit events via `@Output()` / `output()`.
- They are documented with JSDoc on inputs/outputs.
- They include Storybook stories (when Storybook is configured).
- Examples: `ButtonComponent`, `BadgeComponent`, `EmptyStateComponent`, `DataTableComponent`, `ConfirmDialogComponent`.

---

## Error Handling Rules

### Frontend

1. HTTP errors are caught in interceptors and mapped to user-friendly messages.
2. Unhandled errors are caught by a global `ErrorHandler` and reported to logging service.
3. Forms show inline validation messages — never `alert()` or `console.error()`.
4. Network failures show retry UI, not blank screens.
5. Route resolution failures show error page component.

### Backend

1. All controller actions are wrapped by global exception middleware.
2. Validation errors return `400 Bad Request` with field-level detail.
3. Authentication errors return `401 Unauthorized`.
4. Authorization errors return `403 Forbidden`.
5. Not found errors return `404 Not Found`.
6. Conflict errors return `409 Conflict`.
7. Server errors return `500 Internal Server Error` with correlation ID (no stack traces in production).

---

## Logging Standards

### Frontend (Angular)

- `LoggerService` wraps console methods and sends errors to a backend logging endpoint.
- Log levels: `debug` (dev only), `info`, `warn`, `error`.
- Include user ID, route, and timestamp in log context.
- Never log PII (passwords, tokens, national IDs).

### Backend (.NET)

- Use `Microsoft.Extensions.Logging.ILogger<T>`.
- Use structured logging with Serilog → Seq (dev) / Application Insights (prod).
- Log levels: `Trace` (dev), `Debug`, `Information`, `Warning`, `Error`, `Critical`.
- Every request logs: `CorrelationId`, `UserId`, `Endpoint`, `DurationMs`, `StatusCode`.
- Sensitive fields are masked in logs: passwords, tokens, card numbers.

---

## Security Standards

- JWT tokens expire in 15 minutes; refresh tokens expire in 7 days.
- Refresh tokens are rotated on every use (one-time use).
- Passwords hashed with BCrypt (cost factor 12).
- HTTPS enforced in all environments.
- CORS restricted to known origins.
- CSRF protection via `SameSite=Strict` cookies for refresh tokens.
- Rate limiting on auth endpoints: 5 attempts/minute per IP.
- All user inputs sanitized against XSS.
- SQL injection prevented by EF Core parameterized queries (never string concatenation).
- Sensitive config values stored in Azure Key Vault / GitHub Secrets.
- API keys rotated every 90 days.
- Security headers: `X-Content-Type-Options`, `X-Frame-Options`, `Content-Security-Policy`, `Strict-Transport-Security`.

---

## API Standards

- Version prefix: `/api/v1/`.
- All responses use unified envelope:
  ```json
  {
    "success": true,
    "data": { ... },
    "message": "Success",
    "errors": [],
    "pagination": { "page": 1, "pageSize": 20, "total": 450 }
  }
  ```
- Pagination: cursor-based for feeds, offset-based for admin tables.
- Field names: camelCase in JSON.
- Dates: ISO 8601 UTC (`2024-01-15T10:30:00Z`).
- IDs in URLs: numeric int (not GUID) for simplicity.
- Filtering via query parameters: `?category=engineering&location=bangalore&stipend=5000`.
- Sorting: `?sortBy=stipend&sortOrder=desc`.
- OpenAPI/Swagger auto-generated with XML docs.

---

## Commit Message Rules

Follow **Conventional Commits** specification:

```
<type>(<scope>): <short description>

[optional body]

[optional footer]
```

**Types**: `feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`, `build`, `ci`, `chore`, `revert`

**Examples**:
```
feat(internships): add filter by stipend range
fix(auth): refresh token rotation race condition
perf(listings): add database index on CategoryId
test(applications): add unit tests for ApplicationService
docs(api): update OpenAPI spec for internships endpoint
```

- Subject line max 72 characters.
- Use imperative mood: "add" not "added" or "adds".
- Reference issues: `Closes #42`, `Fixes #87`.

---

## Performance Rules

### Frontend

- Lazy-load all feature modules.
- Use `OnPush` change detection everywhere.
- Use `trackBy` on all lists.
- Use virtual scrolling for lists > 50 items (`CdkVirtualScrollViewport`).
- Debounce search inputs by 300ms.
- Use `NgOptimizedImage` for all `<img>` tags.
- Bundle size limit: `< 250KB` initial chunk, `< 100KB` per lazy chunk.
- Core Web Vitals targets: LCP < 2.5s, FID < 100ms, CLS < 0.1.

### Backend

- Use async/await for all I/O.
- Use output caching for public listing endpoints (60s TTL).
- Use database indexes on all FK and filter columns.
- Use `AsNoTracking()` for read-only EF Core queries.
- Use `Select()` projections — never fetch entire entities for DTOs.
- Connection pool: min 5, max 100.
- Response compression: Brotli > GZip.
- Use `IMemoryCache` for hot reference data (categories, locations).

---

## Testing Rules

### Angular

- Unit tests: Jasmine + Karma (or Vitest).
- E2E tests: Playwright.
- Minimum coverage: 80% for services, 60% for components.
- All HTTP calls must be tested with `HttpClientTestingModule`.
- All guards and interceptors must have unit tests.
- Use `spectator` library for component testing to reduce boilerplate.

### .NET

- Unit tests: xUnit + Moq + FluentAssertions.
- Integration tests: `WebApplicationFactory<Program>` with test database.
- Minimum coverage: 85% for Application layer, 70% for API layer.
- All repository methods must have integration tests against a real SQL Server (TestContainers).
- All validators must have unit tests for valid and invalid inputs.
- Use `AutoFixture` for test data generation.

---

## Code Review Checklist

Before merging any PR, verify:

- [ ] Zero TypeScript errors and ESLint warnings.
- [ ] Zero .NET build errors and warnings.
- [ ] All new code has corresponding tests.
- [ ] No hardcoded secrets, URLs, or magic numbers.
- [ ] All API endpoints are documented in Swagger.
- [ ] Database migrations are included and reversible.
- [ ] No `any` types in TypeScript.
- [ ] No synchronous I/O in .NET.
- [ ] Error handling present for all async operations.
- [ ] No unused imports or variables.
- [ ] Accessibility attributes on interactive elements.
- [ ] Mobile-responsive UI verified at 360px, 768px, 1024px, 1440px.
- [ ] PR description references the issue/ticket.
- [ ] Commit messages follow Conventional Commits.
- [ ] No TODO comments left in code.
- [ ] Performance: no N+1 queries introduced.

---

## Accessibility Rules

- All images have descriptive `alt` attributes.
- All form inputs have associated `<label>` elements.
- All buttons have accessible names (text or `aria-label`).
- Color is never the only way to convey information.
- Focus indicator is always visible (never `outline: none` without replacement).
- Keyboard navigation works for all interactive elements.
- WCAG 2.1 AA compliance minimum.
- Use Angular CDK A11y utilities (`FocusTrap`, `LiveAnnouncer`).
- Screen reader tested with NVDA (Windows) or VoiceOver (Mac).
- Minimum contrast ratio: 4.5:1 for normal text, 3:1 for large text.
