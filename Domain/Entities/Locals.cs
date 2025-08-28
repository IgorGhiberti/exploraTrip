using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Local : BaseEntity
{
    private Local () {}
    public Guid LocalId { get; init; }
    public DateTime DateStar { get; init; }
    public DateTime DateEnd { get; private set; }
    public Budget? BudgetLocal { get; private set; }
    public ICollection<Activitie>? Activities { get; private set; }
    public string[]? Notes { get; private set; }
    public Guid TripId { get; private set; }
    public Trip Trip { get; private set; }
}