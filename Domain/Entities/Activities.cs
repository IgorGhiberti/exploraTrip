using Domain.Common;

namespace Domain.Entities;

public class Activitie : BaseEntity
{
    private Activitie() {}
    public Guid ActivitieId { get; init; }
    public string ActivitieName { get; private set; } = string.Empty;
    public DateTime ActivitieDate { get; private set; } = DateTime.UtcNow;
    public string[]? Notes { get; private set; }
    public Guid LocalId { get; private set; }
    public Local Local { get; private set; }
}