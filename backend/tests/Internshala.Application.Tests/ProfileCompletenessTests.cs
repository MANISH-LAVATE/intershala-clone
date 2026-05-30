using FluentAssertions;
using FluentValidation.TestHelper;
using Internshala.Application.Features.Profile.DTOs;
using Internshala.Application.Features.Profile.Validators;

namespace Internshala.Application.Tests;

public sealed class UpdateStudentProfileRequestValidatorTests
{
    private readonly UpdateStudentProfileRequestValidator _validator = new();

    [Fact]
    public void Valid_Profile_ShouldPassValidation()
    {
        var request = new UpdateStudentProfileRequest(
            "Priya", "Sharma", "+91-9876543210",
            DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
            "Female", "IIT Bombay", "B.Tech Computer Science",
            2026, 8.5m, "Passionate developer looking for opportunities.",
            "https://linkedin.com/in/priya", "https://github.com/priya", null);

        _validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Empty_FirstName_ShouldFailValidation(string firstName)
    {
        var request = new UpdateStudentProfileRequest(
            firstName, "Sharma", null, null, null, null, null, null, null, null, null, null, null);

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.FirstName);
    }

    [Fact]
    public void Bio_ExceedingMaxLength_ShouldFailValidation()
    {
        var request = new UpdateStudentProfileRequest(
            "Priya", "Sharma", null, null, null, null, null, null, null,
            new string('x', 1001), null, null, null);

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.Bio);
    }

    [Fact]
    public void GraduationYear_OutOfRange_ShouldFailValidation()
    {
        var request = new UpdateStudentProfileRequest(
            "Priya", "Sharma", null, null, null, null, null, 1900, null, null, null, null, null);

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.GraduationYear);
    }
}
