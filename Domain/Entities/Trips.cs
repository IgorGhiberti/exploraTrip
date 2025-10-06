using Domain.Common;
using Domain.DomainResults;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;
public class Trip : BaseEntity
{
    private Trip(string name, DateTime? dateStart, DateTime? dateEnd, string createdBy, decimal? tripBudget = null, string[]? notes = null) : base(DateTime.UtcNow, createdBy, DateTime.UtcNow, createdBy)
    {
        TripId = Guid.NewGuid();
        Name = name;
        DateStart = dateStart;
        DateEnd = dateEnd;
        TripBudget = tripBudget;
        Notes = notes;
        CreatedBy = createdBy;
    }
    private static ResultData<DateTime?> ValidateDateStart(DateTime? startDate)
    {
        if (startDate < DateTime.UtcNow.Date)
                return ResultData<DateTime?>.Error("Start date cannot be less than the current date.");
        return ResultData<DateTime?>.Success(startDate);
    }
    private static ResultData<DateTime?> ValidateEndDate(DateTime? endDate, DateTime? dateStart)
    {
        if (endDate < dateStart)
            return ResultData<DateTime?>.Error("End date cannot be less than the start date.");
        return ResultData<DateTime?>.Success(endDate);
    }
    private static ResultData<decimal?> ValidateBudget(decimal? tripBudget)
    {
        if (tripBudget < 0)
            return ResultData<decimal?>.Error("Budget cannot be less than 0.");
        return ResultData<decimal?>.Success(tripBudget);
    }

    // Factorys
    public static ResultData<Trip> CreateTrip(string name, DateTime? dateStart, DateTime? dateEnd,
                                          string createdBy, decimal? tripBudget = null, string[]? notes = null)
    {
        var startValidation = ValidateDateStart(dateStart);
        if (!startValidation.IsSuccess)
            return ResultData<Trip>.Error(startValidation.Message);

        var endValidation = ValidateEndDate(dateEnd, dateStart);
        if (!endValidation.IsSuccess)
            return ResultData<Trip>.Error(endValidation.Message);

        var budgetValidation = ValidateBudget(tripBudget);
        if (!budgetValidation.IsSuccess)
            return ResultData<Trip>.Error(budgetValidation.Message);

        return ResultData<Trip>.Success(
            new Trip(name, startValidation.Data, endValidation.Data, createdBy, budgetValidation.Data, notes)
        );
    }
    public ResultData<bool> UpdateTrip(string? name, DateTime? startDate, DateTime? endDate, decimal? tripBudget, string[]? notes)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name;

        if (startDate.HasValue)
        {
            if (!ValidateDateStart(startDate).IsSuccess)
                return ResultData<bool>.Error(ValidateDateStart(startDate).Message);
            DateStart = ValidateDateStart(startDate).Data;
        }

        if (endDate.HasValue)
        {
            if (!ValidateEndDate(endDate, startDate).IsSuccess)
                return ResultData<bool>.Error(ValidateEndDate(endDate, startDate).Message);
            DateEnd = ValidateEndDate(endDate, startDate).Data;
        }

        if (tripBudget.HasValue)
        {
            if (!ValidateBudget(tripBudget).IsSuccess)
                return ResultData<bool>.Error(ValidateBudget(tripBudget).Message);
            TripBudget = ValidateBudget(tripBudget).Data;
        }

        if (notes != null)
            Notes = notes;

        return ResultData<bool>.Success(true);
    }
    //Pro entity
    private Trip() { }
    public Guid TripId { get; init; }
    public string Name { get; private set; } = string.Empty;
    public DateTime? DateStart { get; private set; }
    public DateTime? DateEnd { get; private set; }
    public decimal? TripBudget { get; private set; }
    public ICollection<Local>? Locals { get; private set; }
    public ICollection<TripParticipant> TripParticipants { get; private set; } = new List<TripParticipant>();
    public string[]? Notes{ get; private set; }
}