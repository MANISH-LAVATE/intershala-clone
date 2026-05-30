using FluentAssertions;
using FluentValidation.TestHelper;
using Internshala.Application.Features.Applications.DTOs;
using Internshala.Application.Features.Applications.Validators;
using Internshala.Domain.Enums;

namespace Internshala.Application.Tests;

public sealed class ApplyRequestValidatorTests
{
    private readonly ApplyRequestValidator _validator = new();

    [Fact]
    public void Valid_Request_ShouldPassValidation()
    {
        var request = new ApplyRequest(
            "Internship",
            1,
            "I am very interested in this opportunity.",
            "https://cdn.example.com/resumes/my-cv.pdf",
            DateOnly.FromDateTime(DateTime.Today.AddDays(7)),
            15000);

        _validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Unknown")]
    [InlineData("INTERNSHIP")]
    public void Invalid_ListingType_ShouldFailValidation(string listingType)
    {
        var request = new ApplyRequest(listingType, 1, null, "https://example.com/cv.pdf", null, null);

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.ListingType);
    }

    [Theory]
    [InlineData("Internship")]
    [InlineData("Job")]
    public void Valid_ListingType_ShouldPassValidation(string listingType)
    {
        var request = new ApplyRequest(listingType, 1, null, "https://example.com/cv.pdf", null, null);

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.ListingType);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Invalid_ListingId_ShouldFailValidation(int listingId)
    {
        var request = new ApplyRequest("Internship", listingId, null, "https://example.com/cv.pdf", null, null);

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.ListingId);
    }

    [Fact]
    public void Empty_ResumeUrl_ShouldFailValidation()
    {
        var request = new ApplyRequest("Internship", 1, null, "", null, null);

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.ResumeUrl);
    }

    [Fact]
    public void CoverLetter_ExceedingMaxLength_ShouldFailValidation()
    {
        var request = new ApplyRequest(
            "Internship", 1,
            new string('x', 5001),
            "https://example.com/cv.pdf",
            null, null);

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.CoverLetter);
    }

    [Fact]
    public void Negative_ExpectedStipend_ShouldFailValidation()
    {
        var request = new ApplyRequest("Internship", 1, null, "https://example.com/cv.pdf", null, -100);

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.ExpectedStipend);
    }
}

public sealed class UpdateApplicationStatusRequestValidatorTests
{
    private readonly UpdateApplicationStatusRequestValidator _validator = new();

    [Theory]
    [InlineData(ApplicationStatus.UnderReview)]
    [InlineData(ApplicationStatus.Shortlisted)]
    [InlineData(ApplicationStatus.Selected)]
    [InlineData(ApplicationStatus.Rejected)]
    public void Valid_Status_ShouldPassValidation(ApplicationStatus status)
    {
        var request = new UpdateApplicationStatusRequest(status, null);

        _validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Note_ExceedingMaxLength_ShouldFailValidation()
    {
        var request = new UpdateApplicationStatusRequest(
            ApplicationStatus.Shortlisted,
            new string('x', 1001));

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.Note);
    }

    [Fact]
    public void Note_AtMaxLength_ShouldPassValidation()
    {
        var request = new UpdateApplicationStatusRequest(
            ApplicationStatus.Shortlisted,
            new string('x', 1000));

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.Note);
    }
}
