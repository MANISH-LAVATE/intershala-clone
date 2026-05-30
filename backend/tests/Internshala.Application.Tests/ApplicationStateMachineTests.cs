using FluentAssertions;
using Internshala.Domain.Enums;
using Internshala.Domain.Services;

namespace Internshala.Application.Tests;

public sealed class ApplicationStateMachineTests
{
    [Theory]
    [InlineData(ApplicationStatus.Applied, ApplicationStatus.UnderReview, true)]
    [InlineData(ApplicationStatus.Applied, ApplicationStatus.Shortlisted, true)]
    [InlineData(ApplicationStatus.Applied, ApplicationStatus.Rejected, true)]
    [InlineData(ApplicationStatus.Applied, ApplicationStatus.Withdrawn, true)]
    [InlineData(ApplicationStatus.Applied, ApplicationStatus.Selected, false)]
    [InlineData(ApplicationStatus.Applied, ApplicationStatus.InterviewScheduled, false)]
    [InlineData(ApplicationStatus.UnderReview, ApplicationStatus.Shortlisted, true)]
    [InlineData(ApplicationStatus.UnderReview, ApplicationStatus.Rejected, true)]
    [InlineData(ApplicationStatus.UnderReview, ApplicationStatus.Applied, false)]
    [InlineData(ApplicationStatus.Shortlisted, ApplicationStatus.InterviewScheduled, true)]
    [InlineData(ApplicationStatus.Shortlisted, ApplicationStatus.Selected, true)]
    [InlineData(ApplicationStatus.Shortlisted, ApplicationStatus.Rejected, true)]
    [InlineData(ApplicationStatus.Shortlisted, ApplicationStatus.Applied, false)]
    [InlineData(ApplicationStatus.InterviewScheduled, ApplicationStatus.Selected, true)]
    [InlineData(ApplicationStatus.InterviewScheduled, ApplicationStatus.Rejected, true)]
    [InlineData(ApplicationStatus.InterviewScheduled, ApplicationStatus.Shortlisted, false)]
    [InlineData(ApplicationStatus.Selected, ApplicationStatus.Rejected, false)]
    [InlineData(ApplicationStatus.Selected, ApplicationStatus.Applied, false)]
    [InlineData(ApplicationStatus.Rejected, ApplicationStatus.Shortlisted, false)]
    [InlineData(ApplicationStatus.Rejected, ApplicationStatus.Applied, false)]
    [InlineData(ApplicationStatus.Withdrawn, ApplicationStatus.Applied, false)]
    [InlineData(ApplicationStatus.Withdrawn, ApplicationStatus.UnderReview, false)]
    public void CanTransition_ShouldReturnExpectedResult(
        ApplicationStatus from,
        ApplicationStatus to,
        bool expected)
    {
        ApplicationStateMachine.CanTransition(from, to).Should().Be(expected);
    }

    [Fact]
    public void GetAllowedTransitions_ForApplied_ShouldReturnFourStatuses()
    {
        var transitions = ApplicationStateMachine.GetAllowedTransitions(ApplicationStatus.Applied);

        transitions.Should().HaveCount(4);
        transitions.Should().Contain(ApplicationStatus.UnderReview);
        transitions.Should().Contain(ApplicationStatus.Shortlisted);
        transitions.Should().Contain(ApplicationStatus.Rejected);
        transitions.Should().Contain(ApplicationStatus.Withdrawn);
    }

    [Fact]
    public void GetAllowedTransitions_ForTerminalStatuses_ShouldReturnEmpty()
    {
        ApplicationStateMachine.GetAllowedTransitions(ApplicationStatus.Selected).Should().BeEmpty();
        ApplicationStateMachine.GetAllowedTransitions(ApplicationStatus.Rejected).Should().BeEmpty();
        ApplicationStateMachine.GetAllowedTransitions(ApplicationStatus.Withdrawn).Should().BeEmpty();
    }

    [Fact]
    public void CanTransition_Samestatus_ShouldReturnFalse()
    {
        ApplicationStateMachine.CanTransition(ApplicationStatus.Applied, ApplicationStatus.Applied).Should().BeFalse();
        ApplicationStateMachine.CanTransition(ApplicationStatus.Shortlisted, ApplicationStatus.Shortlisted).Should().BeFalse();
    }
}
