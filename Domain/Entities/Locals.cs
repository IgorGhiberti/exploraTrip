using Domain.Common;
using Domain.DomainResults;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Local : BaseEntity
{
    private Local(string localName, DateTime? dateStart, DateTime? dateEnd, Trip trip, decimal? localBudget = null, string[]? notes = null)
    {
        LocalId = Guid.NewGuid();
        DateStart = dateStart;
        DateEnd = dateEnd;
        Notes = notes;
        Trip = trip;
        LocalName = localName;
        LocalBudget = localBudget;
    }
    // Factorys
    public static ResultData<Local> CreateLocal(string localName, DateTime? dateStart, DateTime? dateEnd, Trip trip, decimal? localBudget = null, string[]? notes = null)
    {
        if (localBudget.HasValue)
        {
            var resultValidateBudget = ValidateLocalBudget(localBudget, trip);
            if (!resultValidateBudget.IsSuccess)
                return ResultData<Local>.Error(resultValidateBudget.Message);
        }
        return ResultData<Local>.Success(
                new Local(localName, dateStart, dateEnd, trip, localBudget, notes)
            );
    }
    private Local() {}
    private static ResultData<decimal?> ValidateLocalBudget(decimal? localBudget, Trip trip)
    {
        if (localBudget < 0)
            return ResultData<decimal?>.Error("Local budget cannot be lass than 0");
        if (localBudget > trip.TripBudget)
            return ResultData<decimal?>.Error("Local budget cannot be bigger than trip budget");
        return ResultData<decimal?>.Success(localBudget);
    }
    public Guid LocalId { get; init; }
    public string LocalName { get; private set; } = string.Empty;
    public DateTime? DateStart { get; init; }
    public DateTime? DateEnd { get; private set; }
    public string[]? Notes { get; private set; }
    public Guid TripId { get; private set; }
    public Trip Trip { get; private set; }
    public decimal? LocalBudget { get; private set; }
}