# Task.md — Internshala Clone: 5-Day Execution Plan

## Overview

This plan covers the complete build of the Internshala clone MVP from project scaffolding to a deployable, production-ready application. The timeline assumes 10-12 hours/day of focused work with a senior engineer.

**Definition of Done (per task)**: Code compiles, passes lint, has passing unit tests, is PR-reviewed, and is merged to `develop`.

---

## Day 1: Project Foundation & Authentication

### Objectives

1. Initialize both Angular and .NET projects with production-grade configuration.
2. Configure databases, migrations, and seed data.
3. Implement complete JWT + Refresh Token authentication (register, login, refresh, logout).
4. Deploy initial CI pipeline.

### Deliverables

| # | Deliverable | Estimated Hours |
|---|---|---|
| 1.1 | Angular project scaffold (standalone, routing, Material, ESLint, Prettier, Husky) | 1.5h |
| 1.2 | .NET 9 Clean Architecture solution (5 projects, DI wiring, Swagger, Serilog) | 2h |
| 1.3 | SQL Server database setup, EF Core context, base entity with audit columns | 1h |
| 1.4 | Users, Students, Employers, RefreshTokens tables + EF configurations + initial migration | 1.5h |
| 1.5 | Categories, Locations, Skills seed data (100+ realistic Indian cities, 30 categories) | 0.5h |
| 1.6 | Auth module: Register (Student/Employer), Login, JWT generation, Refresh Token | 3h |
| 1.7 | Angular Auth feature: Login/Register forms, JWT interceptor, token service, auth guard | 2h |
| 1.8 | GitHub Actions CI: lint + test + build for both Angular and .NET | 1h |

**Total Estimated Hours**: ~12.5h

### Module Dependencies

- No external module dependencies on Day 1.
- All subsequent days depend on Auth being complete.

### Risk Analysis

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| EF Core migration conflicts | Low | Medium | Keep migrations atomic, review generated SQL |
| JWT RS256 key setup complexity | Medium | High | Use `dotnet user-secrets` for dev keys; document key generation steps |
| Angular Material theming conflicts | Low | Low | Set up theme early, validate with component catalog page |
| Refresh token race condition | Medium | High | Implement token family tracking; rotate on use |

### Testing Checklist

- [ ] `POST /api/v1/auth/register` returns 201 with user data.
- [ ] Duplicate email returns 409 Conflict.
- [ ] `POST /api/v1/auth/login` returns JWT + refresh token.
- [ ] `POST /api/v1/auth/refresh` rotates tokens correctly.
- [ ] `POST /api/v1/auth/logout` revokes refresh token.
- [ ] Angular login form shows validation errors on invalid input.
- [ ] Angular JWT interceptor attaches Bearer token to API requests.
- [ ] Route guard redirects unauthenticated users to login.
- [ ] TypeScript: `ng build` zero errors.
- [ ] .NET: `dotnet build` zero warnings.

### Git Strategy

```
Branch: feature/day1-foundation
Commits:
  feat(backend): initialize clean architecture solution
  feat(database): add initial EF Core context and base entity
  feat(database): add auth tables migration and seed data
  feat(auth): implement JWT + refresh token authentication
  feat(frontend): scaffold angular with material and auth feature
  ci: add github actions pipeline for angular and dotnet
PR: feature/day1-foundation → develop
```

### Build Verification

```powershell
# Backend
cd backend && dotnet build -c Release --no-incremental
dotnet test --no-build --logger "console;verbosity=minimal"

# Frontend
cd frontend && ng build --configuration production
ng lint --max-warnings=0
```

### QA Checklist

- [ ] Register as Student → profile created in DB.
- [ ] Register as Employer → employer profile created in DB.
- [ ] Login with wrong password → 401 response.
- [ ] Access protected route when logged out → redirect to login.
- [ ] Token refreshed automatically when expired.
- [ ] Database has correct indexes (verify with SSMS Index Viewer).

---

## Day 2: Internship & Job Listings

### Objectives

1. Build the full internship listing system (CRUD, search, filter, pagination).
2. Build the job listings system.
3. Build Angular listing pages with real-time filter, skeleton loaders, and pagination.
4. Implement employer internship/job posting forms.

### Deliverables

| # | Deliverable | Estimated Hours |
|---|---|---|
| 2.1 | Internships and Jobs DB tables + EF configs + migration | 1.5h |
| 2.2 | InternshipSkills, JobSkills junction tables | 0.5h |
| 2.3 | `InternshipRepository` with filtering, pagination, full-text search | 2h |
| 2.4 | `InternshipsController` — GET list, GET by id, POST, PUT, DELETE | 1.5h |
| 2.5 | FluentValidation for CreateInternship/UpdateInternship commands | 0.5h |
| 2.6 | `JobsController` + repository (parallel to internships) | 1.5h |
| 2.7 | Angular Internships feature: list page, filter sidebar, internship card, detail page | 3h |
| 2.8 | Angular Employer feature: post internship form, post job form | 2h |

**Total Estimated Hours**: ~12.5h

### Module Dependencies

- Requires Day 1 complete (Auth, Users, Categories, Locations seed data).

### Risk Analysis

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Complex filter query performance | Medium | High | Use covering index; test with `SET STATISTICS IO ON` |
| Full-text search setup (SQL Server) | Medium | Medium | Script FT catalog creation in migration; test with CONTAINS() |
| Angular filter state management | Medium | Medium | Use URL query params as state source of truth |
| N+1 queries in listing | High | Medium | Always use `.Include()` + `.Select()` projection |

### Testing Checklist

- [ ] `GET /api/v1/internships` returns paginated list with 20 default page size.
- [ ] Filter by category, location, stipend, work type returns correct results.
- [ ] Full-text search returns results containing search term in title/description.
- [ ] `POST /api/v1/internships` (Employer role) creates internship correctly.
- [ ] `PUT /api/v1/internships/{id}` (own listing only) updates correctly.
- [ ] Non-employer cannot create internship (403 response).
- [ ] Angular filter sidebar updates URL query params.
- [ ] Filter state preserved on browser back navigation.
- [ ] Skeleton loaders show while data loads.
- [ ] Pagination works correctly (next/prev/jump to page).
- [ ] Internship detail page loads all information.
- [ ] Employer can post a new internship via the form.

### Git Strategy

```
Branch: feature/day2-listings
Commits:
  feat(database): add internships and jobs tables migration
  feat(internships): implement internship repository with filtering
  feat(internships): add internships controller and validators
  feat(jobs): add jobs feature parallel to internships
  feat(frontend/internships): implement listing page with filters
  feat(frontend/employer): add post internship and post job forms
PR: feature/day2-listings → develop
```

### QA Checklist

- [ ] Listing page loads in < 2 seconds.
- [ ] 1000 internships in DB — listing query < 50ms (verify with SQL Profiler).
- [ ] Mobile view: filter sidebar becomes bottom sheet.
- [ ] Featured internships appear first.
- [ ] Employer can see their own listings in employer dashboard.
- [ ] Inactive/deleted internships do not appear in public listing.

---

## Day 3: Applications, Student Profile & Resume Builder

### Objectives

1. Implement the complete application flow (apply, track, withdraw).
2. Build employer application management (view, shortlist, select, reject).
3. Build student profile (edit profile, education, experience, skills).
4. Build basic resume builder (structured resume sections).

### Deliverables

| # | Deliverable | Estimated Hours |
|---|---|---|
| 3.1 | Applications, ApplicationStatusHistory tables + migration | 1h |
| 3.2 | `ApplicationsController` — apply, get my applications, employer view, status update | 2h |
| 3.3 | Application state machine (allowed transitions, guard logic) | 1h |
| 3.4 | Student profile tables: Educations, Experiences, StudentSkills, Resumes | 1h |
| 3.5 | `ProfileController` — get/update profile, education CRUD, experience CRUD | 2h |
| 3.6 | Angular Applications feature: my applications list, application detail, status tracker | 2h |
| 3.7 | Angular Profile feature: edit profile, add education, add experience, skill picker | 2h |
| 3.8 | Employer dashboard: view applications, change status, add notes | 1.5h |

**Total Estimated Hours**: ~12.5h

### Module Dependencies

- Requires Day 2 complete (Internships, Jobs tables, Employers seeded).

### Risk Analysis

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Duplicate application prevention | Medium | High | DB unique filtered index + service-layer check + 409 response |
| Application status transition bugs | Medium | High | State machine class with explicit allowed transitions |
| File upload for resume (optional) | Medium | Medium | Start with URL link; file upload is enhancement |
| Profile completeness calculation | Low | Low | Computed property in Student entity |

### Testing Checklist

- [ ] Student can apply to internship — application created in DB.
- [ ] Cannot apply to same internship twice (409 response).
- [ ] Employer can view applications for their internship.
- [ ] Employer can change status: Applied → Shortlisted → Selected.
- [ ] Invalid status transition rejected (e.g., Applied → Selected directly).
- [ ] Student can withdraw application.
- [ ] `GET /api/v1/applications/my` returns student's applications.
- [ ] Student profile update persists correctly.
- [ ] Education/Experience CRUD operations work.
- [ ] Skills can be added/removed from student profile.
- [ ] Profile completeness percentage calculated correctly.

### Git Strategy

```
Branch: feature/day3-applications-profile
Commits:
  feat(database): add applications and profile tables migration
  feat(applications): implement application controller and state machine
  feat(profile): implement student profile controller
  feat(frontend/applications): add my applications page and status tracker
  feat(frontend/profile): add profile edit, education, experience features
  feat(frontend/employer): add application management dashboard
PR: feature/day3-applications-profile → develop
```

### QA Checklist

- [ ] Full apply flow: browse internship → apply → see in My Applications.
- [ ] Employer flow: post internship → receive application → shortlist → select.
- [ ] Student notified in-app when application status changes.
- [ ] Profile edit: all sections save correctly and display updated data.
- [ ] Profile completeness updates as sections are filled in.
- [ ] Mobile: application form usable on 360px viewport.

---

## Day 4: Courses, Notifications & Search

### Objectives

1. Implement course catalog (list, enroll, progress tracking).
2. Build real-time notification system (SignalR + in-app notifications).
3. Implement global search across internships, jobs, and courses.
4. Build admin panel (user management, listing moderation, analytics).

### Deliverables

| # | Deliverable | Estimated Hours |
|---|---|---|
| 4.1 | Courses, Modules, Enrollments tables + migration | 1h |
| 4.2 | `CoursesController` — list, detail, enroll, progress update | 2h |
| 4.3 | Notifications table + `NotificationsController` + SignalR `NotificationHub` | 2h |
| 4.4 | Angular Courses feature: course catalog, course detail, my courses | 2h |
| 4.5 | Angular notification dropdown + SignalR connection service | 1.5h |
| 4.6 | Global search API endpoint (multi-entity) + Angular search component | 1.5h |
| 4.7 | Admin panel: user list, listing moderation, basic analytics | 2h |
| 4.8 | Email service: email verification, application status emails | 1h |

**Total Estimated Hours**: ~13h

### Module Dependencies

- Requires Days 1-3 complete.
- Notifications require Applications to exist (status change events).

### Risk Analysis

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| SignalR connection management | Medium | Medium | Handle reconnect in Angular service; use connection state observable |
| Multi-entity search performance | Medium | Medium | Use SQL Full-Text with ranked results; consider Elasticsearch for scale |
| Email delivery in dev | Low | Low | Use Mailhog (Docker) in dev; SMTP credentials in user-secrets |
| Admin panel scope creep | High | Medium | Keep admin MVP minimal: user list, listing approval/reject |

### Testing Checklist

- [ ] `GET /api/v1/courses` returns paginated course list.
- [ ] Student can enroll in a course.
- [ ] Cannot enroll in the same course twice.
- [ ] `GET /api/v1/search?q=software` returns results from internships, jobs, courses.
- [ ] Notification created when application status changes.
- [ ] Angular notification dropdown shows unread count badge.
- [ ] SignalR: notification appears in real-time without page refresh.
- [ ] Mark notification as read updates badge count.
- [ ] Admin can approve/reject a pending internship listing.
- [ ] Email verification email sent on registration (visible in Mailhog).

### Git Strategy

```
Branch: feature/day4-courses-notifications-search
Commits:
  feat(database): add courses, notifications tables migration
  feat(courses): implement courses controller and enrollment
  feat(notifications): add signalr hub and notification service
  feat(search): implement global search endpoint
  feat(admin): add admin panel with user and listing management
  feat(frontend/courses): add course catalog and enrollment UI
  feat(frontend/notifications): add real-time notification dropdown
PR: feature/day4-courses-notifications-search → develop
```

### QA Checklist

- [ ] Course enrollment flow: browse → enroll → see in My Courses.
- [ ] Apply to internship → employer gets real-time notification.
- [ ] Search: "machine learning" returns relevant results from all entities.
- [ ] Search results ranked by relevance.
- [ ] Admin can view all users and change account status.
- [ ] Email verification flow: register → email → click link → verified.

---

## Day 5: Polish, Testing, Performance & Deployment

### Objectives

1. Complete E2E test suite for critical flows.
2. Performance optimization pass (frontend and backend).
3. Security hardening (headers, rate limiting, input sanitization review).
4. Configure production deployment (Docker, Azure App Service).
5. Set up monitoring and alerting.
6. Final QA pass and bug fixes.

### Deliverables

| # | Deliverable | Estimated Hours |
|---|---|---|
| 5.1 | Playwright E2E tests: register, login, apply, employer post/manage | 2h |
| 5.2 | .NET integration tests: all API endpoints via WebApplicationFactory | 2h |
| 5.3 | Performance: add missing indexes, output caching, `AsNoTracking` audit | 1h |
| 5.4 | Security hardening: headers middleware, rate limiting, CORS lockdown | 1h |
| 5.5 | Docker multi-stage builds (Angular Nginx, .NET API) | 1h |
| 5.6 | Docker Compose for local dev (API + DB + Redis + Mailhog + Seq) | 0.5h |
| 5.7 | GitHub Actions CD: build → push to ACR → deploy to Azure (staging) | 1.5h |
| 5.8 | Production environment config: Azure Key Vault, App Insights, health checks | 1h |
| 5.9 | README.md: setup instructions, architecture diagram, API docs link | 0.5h |
| 5.10 | Final QA pass: test all user flows on staging environment | 1.5h |

**Total Estimated Hours**: ~12h

### Module Dependencies

- Requires all Days 1-4 complete.

### Risk Analysis

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Azure deployment first-time issues | High | Medium | Test Docker locally first; have rollback plan ready |
| Performance regressions under load | Medium | High | k6 load test before production release |
| SSL/TLS certificate issues | Low | High | Use Azure Managed Certificate (auto-renewed) |
| Environment config errors | Medium | High | Config validation in `Program.cs` at startup (fail fast) |
| Test flakiness in E2E | Medium | Low | Mark flaky tests with `test.fixme` tag; don't block CI |

### Testing Checklist

- [ ] E2E: Student registration → email verify → login → apply to internship.
- [ ] E2E: Employer registration → post internship → shortlist applicant → select.
- [ ] E2E: Browse courses → enroll → complete module.
- [ ] E2E: Admin login → approve internship → verify appears in listing.
- [ ] Load test: 500 concurrent users, listing page p95 < 200ms.
- [ ] Security: OWASP ZAP scan — zero high-severity findings.
- [ ] All API endpoints return correct HTTP status codes.
- [ ] Health check endpoint returns 200 with all services healthy.
- [ ] CSP headers present and correct (no unsafe-inline without nonce).
- [ ] Production build: Angular bundle < 250KB initial chunk.

### Deployment Checklist

- [ ] Docker images build successfully: `docker build -t internshala-api:latest ./backend`.
- [ ] Docker Compose local stack starts: `docker-compose up -d`.
- [ ] Azure resources provisioned (Bicep template applied).
- [ ] GitHub Secrets configured (DB connection string, JWT keys, Azure credentials).
- [ ] GitHub Actions CD pipeline triggers on `develop` merge.
- [ ] Staging deployment successful and smoke tests pass.
- [ ] Production deployment via staging slot swap.
- [ ] Post-deployment health check: `GET /health` returns 200.
- [ ] Application Insights showing telemetry.
- [ ] Azure Monitor alert rules configured (error rate, latency).

### Git Strategy

```
Branch: feature/day5-polish-deployment
Commits:
  test(e2e): add playwright tests for critical user flows
  test(integration): add api integration tests
  perf: add database indexes and output caching
  security: add security headers middleware and rate limiting
  build: add docker multi-stage builds
  ci: add continuous deployment to azure staging
  docs: add readme with setup and architecture guide
PR: feature/day5-polish-deployment → develop
Tag: v1.0.0-rc.1
```

### Final QA Checklist

- [ ] App loads in < 3s on 3G throttled connection (Chrome DevTools).
- [ ] All forms show validation errors correctly.
- [ ] All 404 pages display custom error page.
- [ ] All API errors show user-friendly messages (no stack traces).
- [ ] App works on Chrome, Firefox, Edge, Safari (latest versions).
- [ ] Mobile: test on 360px, 375px, 414px viewports.
- [ ] Tablet: test on 768px, 1024px viewports.
- [ ] Keyboard-only navigation works for all features.
- [ ] Screen reader (NVDA) can navigate and use all core features.
- [ ] GDPR: delete account removes PII correctly.

---

## Cross-Day Dependencies Summary

```
Day 1 (Foundation/Auth)
    ↓
Day 2 (Listings) — depends on: Users, Categories, Locations, Auth
    ↓
Day 3 (Applications/Profile) — depends on: Internships, Jobs, Students, Employers
    ↓
Day 4 (Courses/Notifications/Search) — depends on: Applications, all prior entities
    ↓
Day 5 (Polish/Deploy) — depends on: all features complete
```

---

## Risk Register (Overall)

| ID | Risk | Probability | Impact | Owner | Mitigation |
|---|---|---|---|---|---|
| R1 | Scope creep | High | High | Tech Lead | Strictly follow Task.md; log extras as issues |
| R2 | DB performance at scale | Medium | High | Backend Dev | Benchmark queries with 10K records in dev |
| R3 | JWT key rotation | Low | High | Security | Document rotation procedure; test before prod |
| R4 | SignalR scale (multi-instance) | Medium | Medium | Backend Dev | Redis backplane configured from day one |
| R5 | Third-party API changes | Low | Low | Backend Dev | Wrap all external calls in adapters |
| R6 | Data loss during migration | Low | Critical | DBA | Always backup before running migrations in prod |
| R7 | CI/CD pipeline downtime | Low | Medium | DevOps | Manual deployment procedure documented as fallback |
