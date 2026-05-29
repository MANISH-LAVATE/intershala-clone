using Internshala.Domain.Common;
using Internshala.Domain.Enums;

namespace Internshala.Domain.Entities;

public class ApplicationStatusHistory : BaseEntity
{
    public int ApplicationId { get; set; }
    public ApplicationStatus FromStatus { get; set; }
    public ApplicationStatus ToStatus { get; set; }
    public string? Comment { get; set; }
    public int ChangedBy { get; set; }

    public Application Application { get; set; } = null!;
}
