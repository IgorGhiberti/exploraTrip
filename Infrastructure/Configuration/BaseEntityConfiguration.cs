using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
         builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.UpdatedDate)
            .IsRequired();

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .IsRequired();
    }
}