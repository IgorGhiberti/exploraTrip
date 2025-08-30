using Domain.DomainResults;

namespace Domain.ValueObjects;

public partial record ActivitieBudget
{
    private ActivitieBudget(decimal? value) => ActivitieValue = value;
    public decimal? ActivitieValue { get; set; }
    public static bool IsLessThanZero(decimal? value) => value < 0;
    public static ResultData<ActivitieBudget> CreateTripBudget(decimal? activitieValue, LocalBudget localBudget)
    {
        if (IsLessThanZero(activitieValue) || localBudget.LocalValue < activitieValue)
            return ResultData<ActivitieBudget>.Error("Budget cannot be lass than 0 or bigger than the local value.");

        return ResultData<ActivitieBudget>.Success(new ActivitieBudget(activitieValue));
    }
}