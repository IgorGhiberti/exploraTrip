namespace Domain.Models;

public class TripModel
{
    public string TripName { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? TripBudget { get; set; }
    public string[]? Notes { get; set; }
    public List<TripParticipantModel> TripParticipantModels { get; set; } = new List<TripParticipantModel>();
}