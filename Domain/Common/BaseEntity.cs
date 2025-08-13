namespace Domain.Common;

public class BaseEntity
{
    public BaseEntity(DateTime createdDate, string createdBy, DateTime updatedDate, string updatedBy)
    {
        CreatedDate = createdDate;
        CreatedBy = createdBy;
        UpdatedDate = updatedDate;
        UpdatedBy = updatedBy;
    }
    public BaseEntity()
    {

    }
    public DateTime CreatedDate { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime UpdatedDate { get; private set; }
    public string UpdatedBy { get; private set; } = string.Empty;

}