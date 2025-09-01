using Domain.Common;
using Domain.DomainResults;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;
public class Trip : BaseEntity
{
    public Trip(string name, DateTime dateStart, DateTime dateEnd, string createdBy, decimal? tripBudget = null, string[]? notes = null) : base(DateTime.UtcNow, createdBy, DateTime.UtcNow, createdBy)
    {
        TripId = Guid.NewGuid();
        Name = name;
        DateStart = ValidateDateStart(dateStart).Data;
        DateEnd = ValidateEndDate(dateEnd, dateStart).Data;
        if (tripBudget.HasValue && ValidateBudget(tripBudget).IsSuccess)
            TripBudget = tripBudget;
        Notes = notes;
        CreatedBy = createdBy;
    }
    public static ResultData<DateTime> ValidateDateStart(DateTime startDate)
    {
        if (startDate < DateTime.UtcNow)
            return ResultData<DateTime>.Error("Start date cannot be lass than the current date.");
        return ResultData<DateTime>.Success(startDate);
    }
    public static ResultData<DateTime> ValidateEndDate(DateTime endDate, DateTime dateStart)
    {
        if (endDate < dateStart)
            return ResultData<DateTime>.Error("End date cannot be lass than the start date.");
        return ResultData<DateTime>.Success(endDate);
    }
    public static ResultData<decimal?> ValidateBudget(decimal? tripBudget)
    {
        if (tripBudget < 0)
            return ResultData<decimal?>.Error("Budget cannot be lass than 0.");
        return ResultData<decimal?>.Success(tripBudget);
    }
    //Pro entity
    private Trip() { }
    public Guid TripId { get; init; }
    public string Name { get; private set; } = string.Empty;
    public DateTime DateStart { get; init; } = DateTime.UtcNow;
    public DateTime DateEnd { get; private set; }
    public decimal? TripBudget { get; private set; }
    public ICollection<Local>? Locals { get; private set; }
    public ICollection<TripParticipant> TripParticipants { get; private set; } = new List<TripParticipant>();
    public string[]? Notes{ get; private set; }
}