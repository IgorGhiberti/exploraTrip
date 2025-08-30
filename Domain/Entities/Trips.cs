using Domain.Common;
using Domain.DomainResults;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;
public class Trip : BaseEntity
{
    public Trip(string name, DateTime dateStart, DateTime dateEnd, User user, RoleEnum role, decimal? budget = null, string[]? notes = null)
    {
        TripId = Guid.NewGuid();
        Name = name; 
        DateStart = ValidateDateStart(dateStart).Data;
        DateEnd = ValidateEndDate(dateEnd).Data;
        if (budget.HasValue && ValidateBudget(budget).IsSuccess)
            TripBudget = budget;
        Notes = notes;
        TripParticipants.Add(new TripParticipant(this, user, role));
    }
    public ResultData<TripParticipant> AddUserToTrip(User user, RoleEnum role)
    {
        if (TripParticipants.Any(u => u.UserId == user.Id))
            return ResultData<TripParticipant>.Error("User already added to the trip.");

        var participant = new TripParticipant(this, user, role);
        TripParticipants.Add(participant);
        return ResultData<TripParticipant>.Success(participant);
    }
    public ResultData<TripParticipant> RemoveUserFromTrip(User user)
    {
        var tripParticipant = TripParticipants.FirstOrDefault(t => t.UserId == user.Id);
        if (tripParticipant == null)
            return ResultData<TripParticipant>.Error("User not found in this trip.");
        TripParticipants.Remove(tripParticipant);
        return ResultData<TripParticipant>.Success(tripParticipant);
    }
    private ResultData<DateTime> ValidateDateStart(DateTime startDate)
    {
        if (startDate < DateTime.UtcNow)
            return ResultData<DateTime>.Error("Start date cannot be lass than the current date.");
        return ResultData<DateTime>.Success(startDate);
    }
    private ResultData<DateTime> ValidateEndDate(DateTime endDate)
    {
        if (endDate < DateStart)
            return ResultData<DateTime>.Error("End date cannot be lass than the start date.");
        return ResultData<DateTime>.Success(endDate);
    }
    private ResultData<decimal?> ValidateBudget(decimal? tripBudget)
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