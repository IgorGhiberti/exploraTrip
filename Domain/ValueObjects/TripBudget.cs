using Domain.DomainResults;

namespace Domain.ValueObjects;

public partial record TripBudget
{
    private TripBudget(decimal? value) => TotalValue = value;
    public decimal? TotalValue { get; set; }
    public static bool IsLessThanZero(decimal? value) => value < 0;
    public static ResultData<TripBudget> CreateTripBudget(decimal? budget)
    {
        if (IsLessThanZero(budget))
            return ResultData<TripBudget>.Error("Budget cannot be lass than 0.");

        return ResultData<TripBudget>.Success(new TripBudget(budget));
    }
}