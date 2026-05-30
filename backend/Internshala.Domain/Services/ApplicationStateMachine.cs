using Internshala.Domain.Enums;

namespace Internshala.Domain.Services;

/// <summary>
/// Defines allowed status transitions for job/internship applications.
/// </summary>
public static class ApplicationStateMachine
{
    private static readonly IReadOnlyDictionary<ApplicationStatus, IReadOnlySet<ApplicationStatus>> AllowedTransitions =
        new Dictionary<ApplicationStatus, IReadOnlySet<ApplicationStatus>>
        {
            [ApplicationStatus.Applied] = new HashSet<ApplicationStatus>
            {
                ApplicationStatus.UnderReview,
                ApplicationStatus.Shortlisted,
                ApplicationStatus.Rejected,
                ApplicationStatus.Withdrawn
            },
            [ApplicationStatus.UnderReview] = new HashSet<ApplicationStatus>
            {
                ApplicationStatus.Shortlisted,
                ApplicationStatus.Rejected
            },
            [ApplicationStatus.Shortlisted] = new HashSet<ApplicationStatus>
            {
                ApplicationStatus.InterviewScheduled,
                ApplicationStatus.Selected,
                ApplicationStatus.Rejected
            },
            [ApplicationStatus.InterviewScheduled] = new HashSet<ApplicationStatus>
            {
                ApplicationStatus.Selected,
                ApplicationStatus.Rejected
            },
            [ApplicationStatus.Selected] = new HashSet<ApplicationStatus>(),
            [ApplicationStatus.Rejected] = new HashSet<ApplicationStatus>(),
            [ApplicationStatus.Withdrawn] = new HashSet<ApplicationStatus>()
        };

    public static bool CanTransition(ApplicationStatus from, ApplicationStatus to)
        => AllowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);

    public static IReadOnlySet<ApplicationStatus> GetAllowedTransitions(ApplicationStatus from)
        => AllowedTransitions.TryGetValue(from, out var allowed)
            ? allowed
            : new HashSet<ApplicationStatus>();
}
