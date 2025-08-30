using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class ActivityConfiguration : BaseEntityConfiguration<Activity>
{
    public override void Configure(EntityTypeBuilder<Activity> builder)
    {
        base.Configure(builder);

        builder.ToTable("Activity");

        builder.HasKey(a => a.ActivityId);

        builder.Property(a => a.ActivityDate)
            .IsRequired();

        builder.Property(a => a.ActivityName)
            .IsRequired()
            .HasMaxLength(55);

        builder.Property(a => a.Notes);

    }
}