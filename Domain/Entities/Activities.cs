using Domain.Common;
using Domain.DomainResults;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Activity : BaseEntity
{
    public Activity(string activitieName, DateTime date, Local local, decimal? activitieBudget = null, string[]? notes = null)
    {
        ActivityId = Guid.NewGuid();
        ActivityName = activitieName;
        ActivityDate = date;
        Notes = notes;
        Local = local;
        if (activitieBudget.HasValue && ValidateActivitieBudget(activitieBudget, local).IsSuccess)
            ActivityBudget = activitieBudget;
    }
    private Activity() { }
    private ResultData<decimal?> ValidateActivitieBudget(decimal? activitieBudget, Local local)
    {
        if (activitieBudget < 0)
            return ResultData<decimal?>.Error("ActivitieBudget budget cannot be lass than 0");
        if (activitieBudget > local.LocalBudget)
            return ResultData<decimal?>.Error("Local budget cannot be bigger than trip budget");
        return ResultData<decimal?>.Success(activitieBudget);
    }
    public Guid ActivityId { get; init; }
    public string ActivityName { get; private set; } = string.Empty;
    public DateTime ActivityDate { get; private set; } = DateTime.UtcNow;
    public decimal? ActivityBudget { get; private set; }
    public string[]? Notes { get; private set; }
    public Guid LocalId { get; private set; }
    public Local Local { get; private set; }
}