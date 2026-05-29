using Internshala.Domain.Common;

namespace Internshala.Domain.Entities;

public class Location : BaseEntity
{
    public string CityName { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
