using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class TripParticipantConfiguration : BaseEntityConfiguration<TripParticipant>
{
    public override void Configure(EntityTypeBuilder<TripParticipant> builder)
    {
        base.Configure(builder);

        builder.ToTable("TripParticipant");

        builder.HasKey(tp => new { tp.TripId, tp.UserId });

        builder.HasOne(tp => tp.Trip)
            .WithMany(t => t.TripParticipants)
            .HasForeignKey(tp => tp.TripId);

        builder.HasOne(tp => tp.User)
            .WithMany(u => u.TripParticipants)
            .HasForeignKey(tp => tp.UserId);
    }
}