# GitWorkflow.md — Internshala Clone: Git Strategy & Workflow

## Git Branching Strategy

This project follows **GitFlow** adapted for continuous deployment with added protection for production.

### Branch Hierarchy

```
main          — Production. Always deployable. Tagged with semantic versions.
  └── develop — Integration. Staging deploys on every merge. Must always build.
       ├── feature/*    — New features; branched from develop, merged back to develop.
       ├── fix/*        — Bug fixes; branched from develop, merged back to develop.
       ├── hotfix/*     — Critical production fixes; branched from main, merged to main AND develop.
       ├── release/*    — Release candidates; branched from develop, merged to main AND develop.
       └── chore/*      — Maintenance (deps, config, docs); branched from develop.
```

### Branch Rules

| Branch | Protected | Direct Push | Require PR | Required Reviews | Status Checks |
|---|---|---|---|---|---|
| `main` | Yes | No | Yes | 2 | All CI + smoke tests |
| `develop` | Yes | No | Yes | 1 | All CI |
| `release/*` | Yes | No | Yes | 1 | All CI |
| `feature/*` | No | Yes | — | — | — |
| `hotfix/*` | No | Yes | — | — | — |

---

## Branch Naming Conventions

### Format

```
<type>/<ticket-id>-<short-description>
```

### Types

| Type | Usage |
|---|---|
| `feature` | New functionality |
| `fix` | Bug fix in development |
| `hotfix` | Critical bug fix in production |
| `release` | Release candidate |
| `chore` | Maintenance, dependencies, docs, config |
| `test` | Adding or fixing tests only |
| `perf` | Performance improvements |
| `refactor` | Code refactoring without feature change |

### Examples

```bash
feature/INT-42-internship-filter-sidebar
feature/INT-87-employer-dashboard-stats
fix/INT-101-refresh-token-expiry-bug
fix/INT-115-internship-list-n-plus-one
hotfix/INT-200-critical-auth-bypass
release/v1.2.0
chore/INT-55-upgrade-angular-18-2
perf/INT-90-add-covering-index-applications
refactor/INT-78-extract-internship-query-builder
test/INT-93-add-application-service-unit-tests
```

---

## Commit Naming Conventions

### Conventional Commits Specification

```
<type>(<scope>): <description>

[optional body]

[optional footer(s)]
```

### Commit Types

| Type | When to Use |
|---|---|
| `feat` | New feature or significant enhancement |
| `fix` | Bug fix |
| `docs` | Documentation changes only |
| `style` | Formatting, missing semicolons (no code change) |
| `refactor` | Code change that neither fixes a bug nor adds a feature |
| `perf` | Performance improvement |
| `test` | Adding or correcting tests |
| `build` | Changes to build system, dependencies (npm, NuGet) |
| `ci` | Changes to CI/CD pipeline |
| `chore` | Maintenance tasks (update .gitignore, etc.) |
| `revert` | Reverts a previous commit |

### Scopes (project-specific)

```
auth, internships, jobs, applications, profile, courses, notifications,
search, admin, employer, student, database, frontend, backend, shared,
api, ui, security, performance, config, docker, deploy
```

### Commit Message Examples

```bash
# Feature commit
feat(internships): add stipend range filter with slider UI

Implements a dual-handle slider for filtering internships by
stipend range (₹0 to ₹50,000). Filter state is persisted in
URL query parameters for shareability.

Closes #INT-42

# Bug fix commit
fix(auth): prevent refresh token reuse after rotation

The old refresh token was not being marked as revoked atomically
with the issuance of the new token, creating a race condition
window for token reuse.

Fixes #INT-91

# Performance commit
perf(database): add covering index on Applications table

Added IX_Applications_StudentId with INCLUDE columns to eliminate
key lookups in the student applications listing query.
Query cost reduced from 12ms to 0.8ms at 10K rows.

Resolves #INT-90

# CI commit
ci: add parallel lint and test jobs to pr workflow

Split the CI pipeline into 3 parallel jobs (lint, test-frontend,
test-backend) reducing total CI time from 8 minutes to 3 minutes.
```

### Rules

- Subject line: max 72 characters, imperative mood, no period at end.
- Body: wrap at 100 characters, explain *why* not *what*.
- Footer: reference issues with `Closes`, `Fixes`, or `Resolves` keywords.
- Breaking changes: add `BREAKING CHANGE:` footer or `!` after type:
  ```
  feat(api)!: change internship id from int to guid

  BREAKING CHANGE: All clients must update internship ID references
  from numeric to UUID format. Affects: /api/v1/internships/{id}.
  ```

---

## PR Workflow

### Creating a Pull Request

1. **Branch**: Create from latest `develop` (`git checkout -b feature/INT-42-filter develop`).
2. **Work**: Make commits following Conventional Commits.
3. **Update**: Rebase on latest `develop` before opening PR (`git rebase develop`).
4. **Push**: `git push origin feature/INT-42-filter`.
5. **Open PR**: Title follows commit format: `feat(internships): add stipend filter`.
6. **Fill template**: Complete all sections of the PR template.
7. **Self-review**: Review your own diff before requesting reviews.

### PR Template

```markdown
## Summary

What does this PR do? (1-3 sentences)

## Changes

- [ ] Frontend changes: describe what Angular components/services changed
- [ ] Backend changes: describe what .NET code changed
- [ ] Database changes: describe migration changes (if any)
- [ ] API contract changes: describe endpoint additions/modifications

## Testing

- [ ] Unit tests added/updated
- [ ] Integration tests added/updated
- [ ] Manual testing completed

## Screenshots (if UI changes)

[Before] [After]

## Related Issues

Closes #INT-XX

## Checklist

- [ ] Code follows project coding standards (CLAUDE.md)
- [ ] No TypeScript errors (`ng build`)
- [ ] No ESLint warnings (`ng lint`)
- [ ] No .NET build warnings (`dotnet build -warnaserror`)
- [ ] Migration is reversible (Down() implemented)
- [ ] API documented in Swagger
- [ ] No hardcoded secrets or magic numbers
- [ ] PR title follows Conventional Commits
```

---

## Code Review Process

### Reviewer Responsibilities

1. **Correctness**: Does the code solve the problem correctly? Edge cases covered?
2. **Architecture**: Does it follow Clean Architecture? Correct layer boundaries?
3. **Performance**: Any N+1 queries? Missing indexes? Unnecessary re-renders?
4. **Security**: Input validated? Auth checks in place? No sensitive data leaked?
5. **Tests**: Adequate test coverage? Tests meaningful (not just coverage)?
6. **Code Quality**: Readable? Follows naming conventions? No duplication?
7. **Accessibility**: Interactive elements accessible? ARIA used correctly?

### Review SLA

- PRs must receive first review within 24 hours of opening.
- Author must respond to review comments within 24 hours.
- Blocking comments must be resolved before merge.
- Suggestions (non-blocking) can be merged with "resolve and follow up" noted.

### Review Labels

| Label | Meaning |
|---|---|
| `approved` | Ready to merge |
| `changes-requested` | Must address comments before merge |
| `question` | Clarification needed, not blocking |
| `nitpick` | Style/preference, not blocking |
| `security-review` | Security team must also review |

---

## Merge Strategy

### Feature → Develop

- Strategy: **Squash merge** for small features (1-5 commits).
- Strategy: **Merge commit** for larger features to preserve history.
- Delete source branch after merge (enforced by GitHub setting).
- Branch name convention: linear history maintained on `develop`.

### Develop → Release

```bash
# Create release branch
git checkout develop
git pull origin develop
git checkout -b release/v1.2.0

# Bump version, update CHANGELOG
# Only bug fixes allowed in release branch
git commit -m "chore: bump version to 1.2.0"

# Create PR: release/v1.2.0 → main
# After approval, merge with merge commit (NOT squash — preserve release history)
```

### Release → Main

- Strategy: **Merge commit** (preserves release branch history).
- Immediately tag the merge commit.
- Backport to `develop` immediately after merge.

### Hotfix → Main + Develop

```bash
# Branch from main
git checkout main
git checkout -b hotfix/INT-200-critical-auth-bypass

# Fix, commit, open two PRs:
# 1. hotfix/* → main  (fast-forward or squash)
# 2. hotfix/* → develop  (cherry-pick or squash)
```

---

## Release Strategy

### Semantic Versioning

Format: `MAJOR.MINOR.PATCH` (e.g., `v1.3.2`)

| Increment | When |
|---|---|
| MAJOR | Breaking API changes, database schema incompatible changes |
| MINOR | New features, backward-compatible changes |
| PATCH | Bug fixes, security patches, performance improvements |

### Release Cadence

- **PATCH**: As needed (hotfixes, weekly bug fix batches).
- **MINOR**: Every 2 weeks (sprint cycles).
- **MAJOR**: Planned per roadmap (quarterly).

### Release Checklist

```
[ ] All planned tickets merged to develop
[ ] Release branch created: release/vX.Y.Z
[ ] Version bumped in: package.json, .csproj files
[ ] CHANGELOG.md updated with release notes
[ ] Staging deployment verified (smoke tests pass)
[ ] Performance test completed (k6 results within SLA)
[ ] Security scan clean (OWASP ZAP)
[ ] PR: release/vX.Y.Z → main approved by 2 reviewers
[ ] Merged to main
[ ] Tag created: vX.Y.Z (annotated)
[ ] Deployed to production (slot swap)
[ ] Post-deployment smoke tests passed
[ ] Backmerged to develop
[ ] Release notes published (GitHub Release)
```

---

## Hotfix Workflow

```bash
# 1. Create hotfix branch from main (not develop!)
git checkout main
git pull origin main
git checkout -b hotfix/INT-200-critical-auth-bypass

# 2. Fix the bug with a focused commit
git commit -m "fix(auth): prevent bypass of email verification check

The email verification flag was not checked in the social
login flow, allowing unverified accounts to access protected
resources.

Fixes #INT-200"

# 3. Open PR to main (fast review — security/critical path)
# Get 2 approvals

# 4. Merge to main
git checkout main
git merge --no-ff hotfix/INT-200-critical-auth-bypass
git tag -a v1.2.1 -m "v1.2.1: Critical auth bypass fix"
git push origin main --tags

# 5. Backport to develop
git checkout develop
git merge --no-ff hotfix/INT-200-critical-auth-bypass
git push origin develop

# 6. Delete hotfix branch
git branch -d hotfix/INT-200-critical-auth-bypass
git push origin --delete hotfix/INT-200-critical-auth-bypass
```

---

## Tagging Strategy

### Annotated Tags (required for releases)

```bash
# Create annotated tag
git tag -a v1.2.0 -m "Release v1.2.0

Features:
- Add stipend filter to internship listings (#INT-42)
- Add employer analytics dashboard (#INT-87)

Bug Fixes:
- Fix refresh token rotation race condition (#INT-91)

Performance:
- Add covering index on Applications table (#INT-90)"

# Push tag
git push origin v1.2.0
```

### Tag Naming

| Tag Format | Usage |
|---|---|
| `v1.2.0` | Production release |
| `v1.2.0-rc.1` | Release candidate |
| `v1.2.0-beta.1` | Beta testing release |
| `v1.2.0-alpha.1` | Internal alpha release |

---

## Git Hooks

Managed by **Husky** + **lint-staged** (Angular side) and `.git/hooks/` (Git native).

### Pre-commit Hook (Angular)

```json
// package.json
{
  "lint-staged": {
    "*.ts": ["eslint --fix --max-warnings=0", "prettier --write"],
    "*.html": ["prettier --write"],
    "*.scss": ["prettier --write", "stylelint --fix"]
  }
}
```

```bash
# .husky/pre-commit
#!/usr/bin/env sh
. "$(dirname -- "$0")/_/husky.sh"
cd frontend && npx lint-staged
```

### Commit-msg Hook (Conventional Commits validation)

```bash
# .husky/commit-msg
#!/usr/bin/env sh
. "$(dirname -- "$0")/_/husky.sh"
npx --no -- commitlint --edit ${1}
```

```javascript
// commitlint.config.js
module.exports = {
  extends: ['@commitlint/config-conventional'],
  rules: {
    'scope-enum': [2, 'always', [
      'auth', 'internships', 'jobs', 'applications', 'profile',
      'courses', 'notifications', 'search', 'admin', 'employer',
      'student', 'database', 'frontend', 'backend', 'shared',
      'api', 'ui', 'security', 'performance', 'config', 'docker', 'deploy'
    ]],
    'subject-max-length': [2, 'always', 72],
    'body-max-line-length': [2, 'always', 100]
  }
};
```

### Pre-push Hook (.NET build verification)

```bash
# .husky/pre-push
#!/usr/bin/env sh
. "$(dirname -- "$0")/_/husky.sh"
cd backend && dotnet build -c Release --no-incremental -warnaserror
```

---

## Conventional Commits Reference Card

```
QUICK REFERENCE:
  feat(scope): add X
  fix(scope): fix Y
  perf(scope): improve Z performance
  refactor(scope): restructure W
  test(scope): add tests for V
  docs(scope): update U documentation
  ci: update pipeline
  chore: update dependencies

BREAKING CHANGE:
  feat(api)!: rename internship endpoint
  — or —
  feat(api): rename internship endpoint

  BREAKING CHANGE: /internships/{id} renamed to /listings/{id}

MULTI-SCOPE:
  feat(internships,jobs): add shared skills filter component

REVERT:
  revert: feat(auth): add social login

  This reverts commit 3a1b2c3 due to OAuth provider issues.
  Refs: #INT-156
```

---

## Environment Workflow

### Environment-to-Branch Mapping

| Environment | Source Branch | Deploy Trigger | URL |
|---|---|---|---|
| Local | Any | Manual (`ng serve`, `dotnet run`) | `localhost:4200` / `localhost:5000` |
| Development | `feature/*`, `fix/*` | Manual (developer) | — |
| Staging | `develop` | Auto on merge | `staging.internshala-clone.com` |
| Production | `main` | Manual (slot swap) | `internshala-clone.com` |

### Environment Config Files

```
Angular:
  environment.ts          → development (localhost API)
  environment.staging.ts  → staging (staging API URL)
  environment.prod.ts     → production (prod API URL)

.NET:
  appsettings.json             → base config (non-sensitive defaults)
  appsettings.Development.json → dev overrides (Seq logging, Mailhog)
  appsettings.Staging.json     → staging overrides
  appsettings.Production.json  → production overrides (no secrets here!)
```

### Secrets by Environment

| Secret | Dev Storage | Staging Storage | Prod Storage |
|---|---|---|---|
| DB Connection String | `user-secrets` | GitHub Secrets → App Config | Azure Key Vault |
| JWT Private Key | `user-secrets` | GitHub Secrets | Azure Key Vault |
| SendGrid API Key | `user-secrets` | GitHub Secrets | Azure Key Vault |
| Azure Storage Key | `user-secrets` | GitHub Secrets | Azure Key Vault |

### Never Commit

```gitignore
# .gitignore additions for this project
appsettings.*.local.json
*.pfx
*.key
.env.local
.env.*.local
frontend/.env
secrets/
```
