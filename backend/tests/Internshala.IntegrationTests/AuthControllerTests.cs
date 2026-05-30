using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace Internshala.IntegrationTests;

public sealed class AuthControllerTests(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task RegisterStudent_WithValidData_Returns201()
    {
        var request = new
        {
            email = $"student_{Guid.NewGuid():N}@test.com",
            password = "Test@1234!",
            firstName = "Test",
            lastName = "Student"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/auth/register/student", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task RegisterStudent_WithDuplicateEmail_Returns409()
    {
        var email = $"dup_{Guid.NewGuid():N}@test.com";
        var request = new
        {
            email,
            password = "Test@1234!",
            firstName = "Test",
            lastName = "Student"
        };

        await _client.PostAsJsonAsync("/api/v1/auth/register/student", request);
        var response = await _client.PostAsJsonAsync("/api/v1/auth/register/student", request);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task RegisterEmployer_WithValidData_Returns201()
    {
        var request = new
        {
            email = $"emp_{Guid.NewGuid():N}@test.com",
            password = "Test@1234!",
            firstName = "Test",
            lastName = "Employer",
            companyName = "Test Corp",
            industryId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/v1/auth/register/employer", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Login_WithValidCredentials_Returns200WithToken()
    {
        var email = $"login_{Guid.NewGuid():N}@test.com";
        const string password = "Test@1234!";

        await _client.PostAsJsonAsync("/api/v1/auth/register/student", new
        {
            email, password, firstName = "Login", lastName = "User"
        });

        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new { email, password });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<LoginResponseWrapper>();
        body!.Data.Should().NotBeNull();
        body.Data!.AccessToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithWrongPassword_Returns401()
    {
        var email = $"bad_{Guid.NewGuid():N}@test.com";
        await _client.PostAsJsonAsync("/api/v1/auth/register/student", new
        {
            email, password = "Correct@1234!", firstName = "Test", lastName = "User"
        });

        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new
        {
            email,
            password = "Wrong@9999!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithUnknownEmail_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new
        {
            email = "nobody@unknown.com",
            password = "Test@1234!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    public void Dispose() => _client.Dispose();
}

// Minimal deserialization helpers
file sealed record LoginResponseWrapper(TokenDataDto? Data);
file sealed record TokenDataDto(string AccessToken, string RefreshToken);
