using AgriShare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgriShare.Infrastructure.Persistence.Configurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews", table =>
            table.HasCheckConstraint("CK_Reviews_Rating_Range", "\"Rating\" BETWEEN 1 AND 5"));

        builder.HasKey(review => review.Id);
        builder.HasIndex(review => review.ServiceExecutionId).IsUnique();

        builder.Property(review => review.Comment)
            .HasMaxLength(500);

        builder.Property(review => review.CreatedAt)
            .IsRequired();

        builder.HasOne(review => review.ServiceExecution)
            .WithOne(execution => execution.Review)
            .HasForeignKey<Review>(review => review.ServiceExecutionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
