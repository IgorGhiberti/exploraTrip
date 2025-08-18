namespace Domain.Common;

public class BaseEntity
{
    public BaseEntity(DateTime createdDate, string createdBy, DateTime updatedDate, string updatedBy)
    {
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        CreatedBy = createdBy;
        UpdatedBy = updatedBy;
    }
    public BaseEntity()
    {

    }
    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
    public string CreatedBy { get; private set; } = "User default";
    public DateTime UpdatedDate { get; private set; } = DateTime.UtcNow;
    public string UpdatedBy { get; private set; } = "User default";

}