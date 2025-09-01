using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Models;

public class TripParticipantModel
{
    public string UserName { get; set; } = string.Empty;
    public Email? UserEmail { get; set; }
    public RoleEnum Role { get; set; }
}