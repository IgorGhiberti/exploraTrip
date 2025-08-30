using Domain.DomainResults;

namespace Domain.ValueObjects;

public partial record LocalBudget
{
    private LocalBudget(decimal? value) => LocalValue = value;
    public decimal? LocalValue { get; set; }
    public static bool IsLessThanZero(decimal? value) => value < 0;
    public static ResultData<LocalBudget> CreateTripBudget(decimal? localBudget, TripBudget tripBudget)
    {
        if (IsLessThanZero(localBudget) || tripBudget.TotalValue < localBudget)
            return ResultData<LocalBudget>.Error("Budget cannot be lass than 0 or bigger than total value.");

        return ResultData<LocalBudget>.Success(new LocalBudget(localBudget));
    }
}