using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;

namespace Internshala.IntegrationTests;

public sealed class InternshipsControllerTests(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetInternships_AnonymousRequest_Returns200WithEmptyList()
    {
        var response = await _client.GetAsync("/api/v1/internships");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("\"success\":true");
    }

    [Fact]
    public async Task GetInternshipById_NonExistent_Returns404()
    {
        var response = await _client.GetAsync("/api/v1/internships/99999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateInternship_Unauthenticated_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/internships", new
        {
            title = "Test Internship",
            description = "Test description"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateInternship_AsStudent_Returns403()
    {
        var token = await RegisterAndLoginAsStudentAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/api/v1/internships", new
        {
            title = "Test Internship",
            description = "Test description",
            categoryId = 1,
            internshipType = "Remote",
            durationMonths = 3,
            startDateType = "Immediate",
            openingsCount = 1,
            isPaid = false,
            skillIds = Array.Empty<int>()
        });

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<string> RegisterAndLoginAsStudentAsync()
    {
        var email = $"s_{Guid.NewGuid():N}@test.com";
        const string password = "Test@1234!";

        await _client.PostAsJsonAsync("/api/v1/auth/register/student", new
        {
            email, password, firstName = "Test", lastName = "Student"
        });

        var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
        var body = await loginResponse.Content.ReadFromJsonAsync<LoginResponseWrapper>();
        return body!.Data!.AccessToken;
    }

    public void Dispose() => _client.Dispose();
}

file sealed record LoginResponseWrapper(TokenDataDto? Data);
file sealed record TokenDataDto(string AccessToken, string RefreshToken);
