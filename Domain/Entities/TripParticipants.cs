using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class TripParticipant : BaseEntity
{
    public TripParticipant(Trip trip, User user, RoleEnum role)
    {
        Trip = trip;
        User = user;
        Role = role;
    }
    private TripParticipant() {}
    public Guid TripId { get; private set; }
    public Trip Trip { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public RoleEnum Role { get; private set; }
}