# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first for layer-cache efficiency
COPY Internshala.sln .
COPY Internshala.API/Internshala.API.csproj Internshala.API/
COPY Internshala.Application/Internshala.Application.csproj Internshala.Application/
COPY Internshala.Domain/Internshala.Domain.csproj Internshala.Domain/
COPY Internshala.Infrastructure/Internshala.Infrastructure.csproj Internshala.Infrastructure/
COPY Internshala.Shared/Internshala.Shared.csproj Internshala.Shared/

RUN dotnet restore Internshala.API/Internshala.API.csproj

# Copy everything else and build
COPY . .
RUN dotnet build Internshala.API/Internshala.API.csproj -c Release --no-restore

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish Internshala.API/Internshala.API.csproj -c Release -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Stage 3: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Security: run as non-root
RUN addgroup --system --gid 1001 appgroup \
 && adduser --system --uid 1001 --ingroup appgroup appuser
USER appuser

COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Internshala.API.dll"]
