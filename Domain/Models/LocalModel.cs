namespace Domain.Models;

public class LocalModel
{
    public Guid LocalID { get; set; }
    public string LocalName { get; set; } = string.Empty;
    public string LocalDescription { get; set; }= string.Empty;
    public decimal LocalBudget { get; set; }
}