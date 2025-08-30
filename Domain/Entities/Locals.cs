using Domain.Common;
using Domain.DomainResults;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Local : BaseEntity
{
    public Local(string localName, DateTime dateStart, DateTime dateEnd, List<Activity> activities, Trip trip, decimal? localBudget = null, string[]? notes = null)
    {
        LocalId = Guid.NewGuid();
        DateStart = dateStart;
        DateEnd = dateEnd;
        Activities = activities;
        Notes = notes;
        Trip = trip;
        LocalName = localName;
        if (localBudget.HasValue && ValidateLocalBudget(localBudget, trip).IsSuccess)
            LocalBudget = localBudget;
    }
    private Local() { }
    private ResultData<decimal?> ValidateLocalBudget(decimal? localBudget, Trip trip)
    {
        if (localBudget < 0)
            return ResultData<decimal?>.Error("Local budget cannot be lass than 0");
        if (localBudget > trip.TripBudget)
            return ResultData<decimal?>.Error("Local budget cannot be bigger than trip budget");
        return ResultData<decimal?>.Success(localBudget);
    }
    public Guid LocalId { get; init; }
    public string LocalName { get; private set; } = string.Empty;
    public DateTime DateStart { get; init; }
    public DateTime DateEnd { get; private set; }
    public ICollection<Activity>? Activities { get; private set; }
    public string[]? Notes { get; private set; }
    public Guid TripId { get; private set; }
    public Trip Trip { get; private set; }
    public decimal? LocalBudget { get; private set; }
}