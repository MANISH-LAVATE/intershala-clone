# CICD.md — Internshala Clone: CI/CD Pipeline Documentation

## Overview

This project uses **GitHub Actions** for all CI/CD workflows. The pipeline enforces code quality, test coverage, security scanning, and zero-downtime deployments to Azure.

```
Developer Push / PR
       │
       ▼
┌─────────────────────────────────────────────────┐
│  CI Pipeline (every push to any branch)          │
│  ├── Job 1: Angular Lint + Test + Build          │
│  ├── Job 2: .NET Build + Test + Coverage         │
│  └── Job 3: Security Scan (CodeQL + Trivy)       │
└─────────────────────────────────────────────────┘
       │ (on merge to develop)
       ▼
┌─────────────────────────────────────────────────┐
│  CD Pipeline — Staging                           │
│  ├── Build Docker Images                         │
│  ├── Push to Azure Container Registry            │
│  ├── Deploy to Staging Slot                      │
│  └── Run Playwright Smoke Tests                  │
└─────────────────────────────────────────────────┘
       │ (on merge to main / manual approval)
       ▼
┌─────────────────────────────────────────────────┐
│  CD Pipeline — Production                        │
│  ├── Azure App Service Slot Swap                 │
│  └── Post-Deployment Health Checks               │
└─────────────────────────────────────────────────┘
```

---

## GitHub Actions Setup

### Repository Secrets Configuration

Configure the following secrets in **GitHub → Settings → Secrets and Variables → Actions**:

```
# Azure
AZURE_CREDENTIALS              — Service principal JSON for Azure CLI
AZURE_SUBSCRIPTION_ID          — Azure subscription ID
AZURE_RESOURCE_GROUP           — rg-internshala-prod
AZURE_CONTAINER_REGISTRY       — internshalaacr.azurecr.io
ACR_USERNAME                   — ACR admin username
ACR_PASSWORD                   — ACR admin password
AZURE_WEBAPP_NAME              — internshala-api
AZURE_WEBAPP_PUBLISH_PROFILE   — Download from Azure Portal

# Application
DB_CONNECTION_STRING_STAGING   — SQL Server connection string (staging)
DB_CONNECTION_STRING_PROD      — SQL Server connection string (production)
JWT_PRIVATE_KEY                — RSA private key (base64 encoded)
SENDGRID_API_KEY               — SendGrid API key
AZURE_STORAGE_CONNECTION_STRING — Azure Blob Storage connection string

# Quality
SONAR_TOKEN                    — SonarCloud token (optional)
CODECOV_TOKEN                  — Codecov upload token
```

### GitHub Environments

Create two environments with protection rules:

| Environment | Required Reviewers | Deployment Branch |
|---|---|---|
| `staging` | 0 (auto-deploy on develop merge) | `develop` |
| `production` | 1 (manual approval required) | `main` |

---

## Angular Build Pipeline

### File: `.github/workflows/frontend-ci.yml`

```yaml
name: Frontend CI

on:
  push:
    paths:
      - 'frontend/**'
      - '.github/workflows/frontend-ci.yml'
  pull_request:
    paths:
      - 'frontend/**'

defaults:
  run:
    working-directory: frontend

jobs:
  lint:
    name: Lint
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20.x'
          cache: 'npm'
          cache-dependency-path: frontend/package-lock.json

      - name: Install dependencies
        run: npm ci

      - name: Run ESLint
        run: npm run lint -- --max-warnings=0

      - name: Run Prettier check
        run: npm run format:check

      - name: Run Stylelint
        run: npm run lint:styles

  test:
    name: Unit Tests
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20.x'
          cache: 'npm'
          cache-dependency-path: frontend/package-lock.json

      - name: Install dependencies
        run: npm ci

      - name: Run tests with coverage
        run: npm run test:ci

      - name: Upload coverage to Codecov
        uses: codecov/codecov-action@v4
        with:
          directory: frontend/coverage
          flags: frontend
          token: ${{ secrets.CODECOV_TOKEN }}

  build:
    name: Production Build
    runs-on: ubuntu-latest
    needs: [lint, test]
    steps:
      - uses: actions/checkout@v4

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20.x'
          cache: 'npm'
          cache-dependency-path: frontend/package-lock.json

      - name: Install dependencies
        run: npm ci

      - name: Build production bundle
        run: npm run build:prod

      - name: Check bundle sizes
        run: npm run bundle:analyze -- --no-open

      - name: Upload build artifact
        uses: actions/upload-artifact@v4
        with:
          name: angular-dist-${{ github.sha }}
          path: frontend/dist/
          retention-days: 7
```

---

## .NET Build Pipeline

### File: `.github/workflows/backend-ci.yml`

```yaml
name: Backend CI

on:
  push:
    paths:
      - 'backend/**'
      - '.github/workflows/backend-ci.yml'
  pull_request:
    paths:
      - 'backend/**'

defaults:
  run:
    working-directory: backend

jobs:
  build-and-test:
    name: Build & Test
    runs-on: ubuntu-latest

    services:
      sqlserver:
        image: mcr.microsoft.com/mssql/server:2022-latest
        env:
          ACCEPT_EULA: Y
          SA_PASSWORD: TestPassword123!
        ports:
          - 1433:1433
        options: >-
          --health-cmd "/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P TestPassword123! -Q 'SELECT 1'"
          --health-interval 10s
          --health-timeout 5s
          --health-retries 10

    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET 9
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Restore NuGet packages
        run: dotnet restore

      - name: Build (treat warnings as errors)
        run: dotnet build -c Release --no-restore -warnaserror

      - name: Run unit tests
        run: |
          dotnet test Internshala.Domain.Tests/Internshala.Domain.Tests.csproj \
            --no-build -c Release \
            --logger "trx;LogFileName=domain-results.trx" \
            --collect:"XPlat Code Coverage"

          dotnet test Internshala.Application.Tests/Internshala.Application.Tests.csproj \
            --no-build -c Release \
            --logger "trx;LogFileName=application-results.trx" \
            --collect:"XPlat Code Coverage"

      - name: Run integration tests
        env:
          ConnectionStrings__DefaultConnection: "Server=localhost,1433;Database=InternshalaTest;User Id=SA;Password=TestPassword123!;TrustServerCertificate=True"
        run: |
          dotnet test Internshala.IntegrationTests/Internshala.IntegrationTests.csproj \
            --no-build -c Release \
            --logger "trx;LogFileName=integration-results.trx" \
            --collect:"XPlat Code Coverage"

      - name: Publish test results
        uses: dorny/test-reporter@v1
        if: always()
        with:
          name: .NET Test Results
          path: backend/**/*.trx
          reporter: dotnet-trx

      - name: Upload coverage to Codecov
        uses: codecov/codecov-action@v4
        with:
          directory: backend
          flags: backend
          token: ${{ secrets.CODECOV_TOKEN }}

      - name: Verify EF Core migrations
        env:
          ConnectionStrings__DefaultConnection: "Server=localhost,1433;Database=InternshalaTest;User Id=SA;Password=TestPassword123!;TrustServerCertificate=True"
        run: |
          dotnet tool install --global dotnet-ef
          cd Internshala.API && dotnet ef database update --no-build

      - name: Publish API
        run: dotnet publish Internshala.API/Internshala.API.csproj -c Release -o ./publish --no-build

      - name: Upload publish artifact
        uses: actions/upload-artifact@v4
        with:
          name: dotnet-publish-${{ github.sha }}
          path: backend/publish/
          retention-days: 7
```

---

## Auto Linting

### Angular Linting Configuration

```json
// .eslintrc.json (root)
{
  "root": true,
  "ignorePatterns": ["projects/**/*", "dist/**/*"],
  "overrides": [
    {
      "files": ["*.ts"],
      "extends": [
        "eslint:recommended",
        "plugin:@typescript-eslint/recommended-type-checked",
        "plugin:@angular-eslint/recommended",
        "plugin:@angular-eslint/template/process-inline-templates"
      ],
      "parserOptions": { "project": "./tsconfig.json" },
      "rules": {
        "@typescript-eslint/no-explicit-any": "error",
        "@typescript-eslint/no-unused-vars": "error",
        "@typescript-eslint/explicit-function-return-type": "warn",
        "@angular-eslint/prefer-standalone": "error",
        "@angular-eslint/use-lifecycle-interface": "error",
        "no-console": ["error", { "allow": ["warn", "error"] }],
        "no-debugger": "error"
      }
    },
    {
      "files": ["*.html"],
      "extends": [
        "plugin:@angular-eslint/template/recommended",
        "plugin:@angular-eslint/template/accessibility"
      ]
    }
  ]
}
```

### .NET Linting (EditorConfig + Roslyn Analyzers)

```ini
# .editorconfig
root = true

[*.cs]
indent_style = space
indent_size = 4
end_of_line = lf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

# Naming conventions
dotnet_naming_rule.private_fields.symbols = private_field_symbols
dotnet_naming_rule.private_fields.style = private_field_style
dotnet_naming_rule.private_fields.severity = error
dotnet_naming_symbols.private_field_symbols.applicable_kinds = field
dotnet_naming_symbols.private_field_symbols.applicable_accessibilities = private
dotnet_naming_style.private_field_style.prefix = _
dotnet_naming_style.private_field_style.capitalization = camel_case

# Enable all Roslyn analyzers
dotnet_analyzer_diagnostic.category-Style.severity = warning
dotnet_analyzer_diagnostic.category-Performance.severity = warning
dotnet_analyzer_diagnostic.category-Security.severity = error
```

---

## Auto Testing

### Angular Test Configuration

```json
// package.json scripts
{
  "scripts": {
    "test": "ng test --watch=false",
    "test:ci": "ng test --watch=false --browsers=ChromeHeadless --code-coverage",
    "test:e2e": "playwright test",
    "test:e2e:ci": "playwright test --reporter=github"
  }
}
```

```typescript
// karma.conf.js
module.exports = function(config) {
  config.set({
    basePath: '',
    frameworks: ['jasmine', '@angular-devkit/build-angular'],
    coverageReporter: {
      dir: require('path').join(__dirname, './coverage'),
      subdir: '.',
      reporters: [
        { type: 'html' },
        { type: 'lcov' },
        { type: 'text-summary' }
      ],
      check: {
        global: {
          statements: 70,
          branches: 60,
          functions: 70,
          lines: 70
        }
      }
    }
  });
};
```

---

## Auto Formatting

### Prettier Configuration

```json
// .prettierrc
{
  "semi": true,
  "singleQuote": true,
  "trailingComma": "all",
  "printWidth": 120,
  "tabWidth": 2,
  "useTabs": false,
  "bracketSpacing": true,
  "arrowParens": "always",
  "endOfLine": "lf",
  "overrides": [
    { "files": "*.html", "options": { "printWidth": 160, "htmlWhitespaceSensitivity": "ignore" } },
    { "files": "*.scss", "options": { "singleQuote": false } }
  ]
}
```

### Format Check in CI

```yaml
- name: Check formatting
  run: npx prettier --check "src/**/*.{ts,html,scss,json}"
```

---

## Auto Deployment

### Staging Deployment Pipeline

### File: `.github/workflows/deploy-staging.yml`

```yaml
name: Deploy to Staging

on:
  push:
    branches: [develop]

concurrency:
  group: staging-deployment
  cancel-in-progress: true

jobs:
  build-and-push:
    name: Build & Push Docker Images
    runs-on: ubuntu-latest
    outputs:
      image-tag: ${{ steps.meta.outputs.tags }}

    steps:
      - uses: actions/checkout@v4

      - name: Log in to Azure Container Registry
        uses: azure/docker-login@v1
        with:
          login-server: ${{ secrets.AZURE_CONTAINER_REGISTRY }}
          username: ${{ secrets.ACR_USERNAME }}
          password: ${{ secrets.ACR_PASSWORD }}

      - name: Extract Docker metadata
        id: meta
        uses: docker/metadata-action@v5
        with:
          images: ${{ secrets.AZURE_CONTAINER_REGISTRY }}/internshala-api
          tags: |
            type=sha,prefix=staging-
            type=raw,value=staging

      - name: Build and push API image
        uses: docker/build-push-action@v5
        with:
          context: ./backend
          file: ./backend/Dockerfile
          push: true
          tags: ${{ steps.meta.outputs.tags }}
          cache-from: type=registry,ref=${{ secrets.AZURE_CONTAINER_REGISTRY }}/internshala-api:staging
          cache-to: type=inline

      - name: Build Angular and push Nginx image
        uses: docker/build-push-action@v5
        with:
          context: ./frontend
          file: ./frontend/Dockerfile
          push: true
          build-args: |
            ANGULAR_ENV=staging
          tags: |
            ${{ secrets.AZURE_CONTAINER_REGISTRY }}/internshala-web:staging-${{ github.sha }}
            ${{ secrets.AZURE_CONTAINER_REGISTRY }}/internshala-web:staging

  deploy-staging:
    name: Deploy to Azure Staging
    runs-on: ubuntu-latest
    needs: build-and-push
    environment: staging

    steps:
      - uses: azure/login@v1
        with:
          creds: ${{ secrets.AZURE_CREDENTIALS }}

      - name: Run database migrations
        uses: azure/CLI@v1
        with:
          inlineScript: |
            az webapp config appsettings set \
              --resource-group ${{ secrets.AZURE_RESOURCE_GROUP }} \
              --name ${{ secrets.AZURE_WEBAPP_NAME }} \
              --slot staging \
              --settings RUN_MIGRATIONS=true

      - name: Deploy API to staging slot
        uses: azure/webapps-deploy@v3
        with:
          app-name: ${{ secrets.AZURE_WEBAPP_NAME }}
          slot-name: staging
          images: ${{ secrets.AZURE_CONTAINER_REGISTRY }}/internshala-api:staging-${{ github.sha }}

      - name: Wait for deployment health check
        run: |
          echo "Waiting for app to start..."
          sleep 30
          curl --fail --retry 10 --retry-delay 10 \
            https://${{ secrets.AZURE_WEBAPP_NAME }}-staging.azurewebsites.net/health

  smoke-tests:
    name: Staging Smoke Tests
    runs-on: ubuntu-latest
    needs: deploy-staging

    steps:
      - uses: actions/checkout@v4

      - uses: actions/setup-node@v4
        with:
          node-version: '20.x'

      - name: Install Playwright
        run: cd frontend && npm ci && npx playwright install --with-deps chromium

      - name: Run smoke tests
        env:
          BASE_URL: https://${{ secrets.AZURE_WEBAPP_NAME }}-staging.azurewebsites.net
        run: cd frontend && npx playwright test --project=chromium tests/smoke/

      - name: Upload smoke test results
        uses: actions/upload-artifact@v4
        if: always()
        with:
          name: playwright-smoke-results
          path: frontend/playwright-report/
```

---

## Environment Management

### Angular Environment Files

```typescript
// src/environments/environment.ts (development)
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5000/api/v1',
  signalRHubUrl: 'http://localhost:5000/hubs',
  enableDevTools: true,
  logLevel: 'debug',
  version: '0.0.0-dev',
};

// src/environments/environment.staging.ts
export const environment = {
  production: false,
  apiBaseUrl: 'https://internshala-api-staging.azurewebsites.net/api/v1',
  signalRHubUrl: 'https://internshala-api-staging.azurewebsites.net/hubs',
  enableDevTools: true,
  logLevel: 'info',
  version: '#{BUILD_VERSION}#',  // Replaced by pipeline
};

// src/environments/environment.prod.ts
export const environment = {
  production: true,
  apiBaseUrl: 'https://api.internshala-clone.com/api/v1',
  signalRHubUrl: 'https://api.internshala-clone.com/hubs',
  enableDevTools: false,
  logLevel: 'error',
  version: '#{BUILD_VERSION}#',
};
```

### .NET Configuration Validation

```csharp
// Program.cs — fail fast on missing required config
builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection("Jwt"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<DatabaseSettings>()
    .Bind(builder.Configuration.GetSection("ConnectionStrings"))
    .Validate(s => !string.IsNullOrEmpty(s.DefaultConnection),
        "Database connection string is required")
    .ValidateOnStart();
```

---

## Secret Management

### Development (dotnet user-secrets)

```powershell
# Initialize user secrets for the API project
cd backend/Internshala.API
dotnet user-secrets init

# Set secrets
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=InternshalaDb;Trusted_Connection=True;"
dotnet user-secrets set "Jwt:PrivateKey" "<base64-encoded-rsa-private-key>"
dotnet user-secrets set "Jwt:PublicKey" "<base64-encoded-rsa-public-key>"
dotnet user-secrets set "Email:SendGridApiKey" "SG.xxx"
dotnet user-secrets set "Storage:ConnectionString" "DefaultEndpointsProtocol=https;..."
```

### Production (Azure Key Vault)

```csharp
// Program.cs — load secrets from Key Vault in production
if (builder.Environment.IsProduction())
{
    var keyVaultUri = builder.Configuration["Azure:KeyVaultUri"]
        ?? throw new InvalidOperationException("Azure Key Vault URI not configured");

    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUri),
        new DefaultAzureCredential());
}
```

### GitHub Secrets → App Service Environment Variables

```yaml
# In deployment workflow
- name: Configure app settings
  uses: azure/CLI@v1
  with:
    inlineScript: |
      az webapp config appsettings set \
        --resource-group ${{ secrets.AZURE_RESOURCE_GROUP }} \
        --name ${{ secrets.AZURE_WEBAPP_NAME }} \
        --settings \
          ASPNETCORE_ENVIRONMENT=Production \
          Azure__KeyVaultUri=${{ secrets.AZURE_KEYVAULT_URI }} \
          ApplicationInsights__ConnectionString=${{ secrets.APPINSIGHTS_CONNECTION_STRING }}
```

---

## Docker Setup

### Angular Dockerfile (Multi-stage)

```dockerfile
# frontend/Dockerfile
# Stage 1: Build
FROM node:20-alpine AS builder
WORKDIR /app
ARG ANGULAR_ENV=production
COPY package*.json ./
RUN npm ci --only=production
COPY . .
RUN npm run build -- --configuration=$ANGULAR_ENV

# Stage 2: Serve with Nginx
FROM nginx:1.27-alpine AS runtime
COPY --from=builder /app/dist/internshala/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Security: run as non-root
RUN chown -R nginx:nginx /usr/share/nginx/html && \
    chmod -R 755 /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### Nginx Configuration for Angular SPA

```nginx
# frontend/nginx.conf
server {
    listen 80;
    server_name _;
    root /usr/share/nginx/html;
    index index.html;

    # Security headers
    add_header X-Frame-Options "DENY" always;
    add_header X-Content-Type-Options "nosniff" always;
    add_header X-XSS-Protection "1; mode=block" always;
    add_header Referrer-Policy "strict-origin-when-cross-origin" always;

    # Angular routing — serve index.html for all routes
    location / {
        try_files $uri $uri/ /index.html;
    }

    # Cache static assets (hashed filenames — safe to cache long-term)
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }

    # No cache for index.html
    location = /index.html {
        add_header Cache-Control "no-cache, no-store, must-revalidate";
    }

    gzip on;
    gzip_vary on;
    gzip_min_length 1024;
    gzip_types text/plain text/css application/json application/javascript text/xml;
}
```

### .NET API Dockerfile (Multi-stage)

```dockerfile
# backend/Dockerfile
# Stage 1: Restore and build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Restore (cached layer if no .csproj changes)
COPY ["Internshala.API/Internshala.API.csproj", "Internshala.API/"]
COPY ["Internshala.Application/Internshala.Application.csproj", "Internshala.Application/"]
COPY ["Internshala.Domain/Internshala.Domain.csproj", "Internshala.Domain/"]
COPY ["Internshala.Infrastructure/Internshala.Infrastructure.csproj", "Internshala.Infrastructure/"]
COPY ["Internshala.Shared/Internshala.Shared.csproj", "Internshala.Shared/"]
RUN dotnet restore "Internshala.API/Internshala.API.csproj"

# Build
COPY . .
RUN dotnet build "Internshala.API/Internshala.API.csproj" -c Release -o /app/build --no-restore -warnaserror

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Internshala.API/Internshala.API.csproj" \
    -c Release -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Security: non-root user
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

COPY --from=publish /app/publish .
RUN chown -R appuser:appgroup /app

USER appuser
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Internshala.API.dll"]
```

### Docker Compose (Local Development)

```yaml
# docker-compose.yml
version: '3.9'

services:
  api:
    build:
      context: ./backend
      dockerfile: Dockerfile
      target: build
    command: dotnet watch run --project Internshala.API/Internshala.API.csproj
    ports:
      - "5000:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:5000
      - ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=InternshalaDb;User Id=SA;Password=DevPassword123!;TrustServerCertificate=True
      - Redis__ConnectionString=redis:6379
    depends_on:
      sqlserver:
        condition: service_healthy
      redis:
        condition: service_healthy
    volumes:
      - ./backend:/src

  web:
    build:
      context: ./frontend
      target: builder
    command: npm run start
    ports:
      - "4200:4200"
    volumes:
      - ./frontend:/app
      - /app/node_modules
    environment:
      - API_URL=http://api:5000

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    ports:
      - "1433:1433"
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=DevPassword123!
      - MSSQL_DB=InternshalaDb
    volumes:
      - sqlserver-data:/var/opt/mssql
    healthcheck:
      test: /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P DevPassword123! -Q "SELECT 1"
      interval: 10s
      timeout: 5s
      retries: 10

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    healthcheck:
      test: ["CMD", "redis-cli", "ping"]
      interval: 5s
      timeout: 3s
      retries: 5

  mailhog:
    image: mailhog/mailhog:latest
    ports:
      - "1025:1025"   # SMTP
      - "8025:8025"   # Web UI

  seq:
    image: datalust/seq:latest
    ports:
      - "5341:5341"
      - "5342:80"
    environment:
      - ACCEPT_EULA=Y
    volumes:
      - seq-data:/data

volumes:
  sqlserver-data:
  seq-data:
```

---

## Production Deployment

### File: `.github/workflows/deploy-production.yml`

```yaml
name: Deploy to Production

on:
  push:
    branches: [main]
  workflow_dispatch:
    inputs:
      confirm:
        description: 'Type DEPLOY to confirm production deployment'
        required: true

jobs:
  validate-input:
    name: Validate Deployment
    runs-on: ubuntu-latest
    if: github.event_name == 'workflow_dispatch'
    steps:
      - name: Check confirmation
        run: |
          if [ "${{ github.event.inputs.confirm }}" != "DEPLOY" ]; then
            echo "Deployment not confirmed. Exiting."
            exit 1
          fi

  deploy-production:
    name: Swap Staging → Production
    runs-on: ubuntu-latest
    environment: production
    needs: [validate-input]
    if: always() && (needs.validate-input.result == 'success' || github.event_name == 'push')

    steps:
      - uses: azure/login@v1
        with:
          creds: ${{ secrets.AZURE_CREDENTIALS }}

      - name: Swap staging slot to production
        uses: azure/CLI@v1
        with:
          inlineScript: |
            az webapp deployment slot swap \
              --resource-group ${{ secrets.AZURE_RESOURCE_GROUP }} \
              --name ${{ secrets.AZURE_WEBAPP_NAME }} \
              --slot staging \
              --target-slot production

      - name: Wait and run health check
        run: |
          sleep 30
          curl --fail --retry 5 --retry-delay 10 \
            https://api.internshala-clone.com/health

      - name: Run production smoke tests
        run: |
          curl --fail https://api.internshala-clone.com/api/v1/internships?page=1
          curl --fail https://internshala-clone.com

      - name: Notify on success
        if: success()
        uses: 8398a7/action-slack@v3
        with:
          status: success
          text: ':rocket: Production deployment successful! Version ${{ github.sha }}'
          webhook_url: ${{ secrets.SLACK_WEBHOOK_URL }}

      - name: Notify and rollback on failure
        if: failure()
        uses: azure/CLI@v1
        with:
          inlineScript: |
            echo "Deployment failed — swapping back to previous version"
            az webapp deployment slot swap \
              --resource-group ${{ secrets.AZURE_RESOURCE_GROUP }} \
              --name ${{ secrets.AZURE_WEBAPP_NAME }} \
              --slot staging \
              --target-slot production
```

---

## Rollback Strategy

### Automatic Rollback (Zero-Downtime)

Azure App Service slot swaps are reversible:

```bash
# Manual rollback command (run within 5 minutes for best experience)
az webapp deployment slot swap \
  --resource-group rg-internshala-prod \
  --name internshala-api \
  --slot staging \
  --target-slot production
```

### Database Rollback

```bash
# Rollback last EF Core migration (if applicable)
cd backend && dotnet ef database update {PreviousMigrationName} \
  --connection "$(az keyvault secret show ...)"
```

### Container Rollback

```bash
# Redeploy previous image version
az webapp config container set \
  --resource-group rg-internshala-prod \
  --name internshala-api \
  --docker-custom-image-name internshalaacr.azurecr.io/internshala-api:{previous-sha}
```

---

## Health Checks

### .NET Health Check Implementation

```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection")!,
        healthQuery: "SELECT 1",
        name: "sqlserver",
        failureStatus: HealthStatus.Degraded)
    .AddRedis(
        redisConnectionString: builder.Configuration["Redis:ConnectionString"]!,
        name: "redis",
        failureStatus: HealthStatus.Degraded)
    .AddAzureBlobStorage(
        connectionString: builder.Configuration["Storage:ConnectionString"]!,
        name: "blob-storage",
        failureStatus: HealthStatus.Degraded)
    .AddCheck<ApiVersionHealthCheck>("api-version");

// Map endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false  // Liveness: always 200 if app is running
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
```

### Health Check Response Format

```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.1234567",
  "entries": {
    "sqlserver": { "status": "Healthy", "duration": "00:00:00.0456789" },
    "redis": { "status": "Healthy", "duration": "00:00:00.0123456" },
    "blob-storage": { "status": "Healthy", "duration": "00:00:00.0789012" }
  }
}
```

---

## Performance Monitoring

### Azure Application Insights Integration

```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    options.EnableDependencyTrackingTelemetryModule = true;
    options.EnablePerformanceCounterCollectionModule = true;
});

// Custom telemetry: track business events
public class InternshipApplicationTracker
{
    private readonly TelemetryClient _telemetry;

    public void TrackApplicationSubmitted(int internshipId, int studentId)
    {
        _telemetry.TrackEvent("ApplicationSubmitted", new Dictionary<string, string>
        {
            ["InternshipId"] = internshipId.ToString(),
            ["StudentId"] = studentId.ToString()
        });
    }
}
```

### Alert Rules (Azure Monitor)

```json
{
  "alerts": [
    {
      "name": "High Error Rate",
      "condition": "requests/failed > 5% over 5 minutes",
      "severity": 1,
      "action": "email + slack"
    },
    {
      "name": "High Latency",
      "condition": "requests/duration p95 > 500ms over 5 minutes",
      "severity": 2,
      "action": "slack"
    },
    {
      "name": "Database Connection Issues",
      "condition": "dependencies/failed containing 'SQL' > 3 in 1 minute",
      "severity": 1,
      "action": "email + slack + pagerduty"
    },
    {
      "name": "Health Check Failing",
      "condition": "availabilityResults/availabilityPercentage < 99% over 5 minutes",
      "severity": 0,
      "action": "pagerduty"
    }
  ]
}
```

### Angular Performance Monitoring

```typescript
// core/services/performance-monitoring.service.ts
@Injectable({ providedIn: 'root' })
export class PerformanceMonitoringService {
  private readonly appInsights = inject(ApplicationInsightsService);

  trackPageLoad(pageName: string): void {
    const perfEntry = performance.getEntriesByType('navigation')[0] as PerformanceNavigationTiming;
    this.appInsights.trackPageView({
      name: pageName,
      duration: perfEntry.loadEventEnd - perfEntry.startTime,
    });
  }

  trackCoreWebVitals(): void {
    // LCP
    new PerformanceObserver(list => {
      const entry = list.getEntries().at(-1) as PerformancePaintTiming;
      this.appInsights.trackMetric({ name: 'LCP', average: entry.startTime });
    }).observe({ entryTypes: ['largest-contentful-paint'] });

    // CLS
    new PerformanceObserver(list => {
      let clsValue = 0;
      list.getEntries().forEach(e => { clsValue += (e as any).value; });
      this.appInsights.trackMetric({ name: 'CLS', average: clsValue });
    }).observe({ entryTypes: ['layout-shift'] });
  }
}
```
