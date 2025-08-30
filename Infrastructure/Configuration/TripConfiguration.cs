using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class TripConfiguration : BaseEntityConfiguration<Trip>
{
    public override void Configure(EntityTypeBuilder<Trip> builder)
    {
        base.Configure(builder);

        builder.ToTable("Trip");

        builder.HasKey(t => t.TripId);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(t => t.Notes);

        builder.Property(t => t.DateStart)
            .IsRequired();

        builder.Property(t => t.DateEnd)
            .IsRequired();

        builder.Property(t => t.TripBudget);

        builder.HasMany(t => t.Locals)
            .WithOne(l => l.Trip)
            .HasForeignKey(l => l.TripId)
            .OnDelete(DeleteBehavior.NoAction);
        
    }
}