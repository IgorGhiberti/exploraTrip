using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Local : BaseEntity
{
    public Local(string localName, DateTime dateStart, DateTime dateEnd, List<Activitie> activities, Trip trip, string[]? notes = null)
    {
        LocalId = Guid.NewGuid();
        DateStart = dateStart;
        DateEnd = dateEnd;
        Activities = activities;
        Notes = notes;
        Trip = trip;
        LocalName = localName;
    }
    private Local() { }
    public Guid LocalId { get; init; }
    public string LocalName { get; private set; } = string.Empty;
    public DateTime DateStart { get; init; }
    public DateTime DateEnd { get; private set; }
    public ICollection<Activitie>? Activities { get; private set; }
    public string[]? Notes { get; private set; }
    public Guid TripId { get; private set; }
    public Trip Trip { get; private set; }
}