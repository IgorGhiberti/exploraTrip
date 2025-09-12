using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class LocalConfiguration : BaseEntityConfiguration<Local>
{
    public override void Configure(EntityTypeBuilder<Local> builder)
    {
        base.Configure(builder);

        builder.ToTable("Local");

        builder.HasKey(l => l.LocalId);

        builder.Property(l => l.LocalName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(l => l.DateStart);

        builder.Property(l => l.DateEnd);

        builder.Property(l => l.Notes);

        builder.Property(l => l.LocalBudget);
    }
}