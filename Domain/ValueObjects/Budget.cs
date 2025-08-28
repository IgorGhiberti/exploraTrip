using Domain.DomainResults;

namespace Domain.ValueObjects;

public partial record Budget
{
    private Budget(decimal? value) => TotalValue = value;
    public decimal? TotalValue { get; set; }
    public bool ExceedsTotalValue(decimal? value) => value > TotalValue;
    public bool IsLessThanZero(decimal? value) => value < 0;
    public ResultData<Budget> AddLocalValue(decimal localValue)
    {
        if (ExceedsTotalValue(localValue) || IsLessThanZero(localValue))
            return ResultData<Budget>.Error("Local value cannot be greater than the trip budget or less than 0.");

        TotalValue -= localValue;
        return ResultData<Budget>.Success(new Budget(TotalValue));
    }
    public static ResultData<Budget> CreateTripBudget(decimal? budget)
    {
        if (budget < 0)
            return ResultData<Budget>.Error("Budget cannot be lass than 0.");

        return ResultData<Budget>.Success(new Budget(budget));
    }
    // public static ResultData<Budget> CreateLocalBudget(decimal? budgetLocal)
    // {

    // }
}