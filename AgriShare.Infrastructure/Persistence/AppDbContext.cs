using AgriShare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgriShare.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<Machinery> Machinery => Set<Machinery>();
    public DbSet<Operator> Operators => Set<Operator>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
    public DbSet<ServiceExecution> ServiceExecutions => Set<ServiceExecution>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(user => user.Email).IsUnique();
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasOne(provider => provider.User)
                .WithOne()
                .HasForeignKey<Provider>(provider => provider.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Machinery>(entity =>
        {
            entity.HasOne(machinery => machinery.Provider)
                .WithMany(provider => provider.Machinery)
                .HasForeignKey(machinery => machinery.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasOne(operatorEntity => operatorEntity.Provider)
                .WithMany(provider => provider.Operators)
                .HasForeignKey(operatorEntity => operatorEntity.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(operatorEntity => operatorEntity.User)
                .WithOne()
                .HasForeignKey<Operator>(operatorEntity => operatorEntity.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(service => service.BasePrice).HasPrecision(18, 2);
            entity.Property(service => service.TransportFee).HasPrecision(18, 2);
            entity.Property(service => service.CoverageArea)
                .HasColumnType("geometry(Polygon, 4326)");
            entity.Property(service => service.Location)
                .HasColumnType("geometry(Point, 4326)");

            entity.HasOne(service => service.Provider)
                .WithMany(provider => provider.Services)
                .HasForeignKey(service => service.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(service => service.Machinery)
                .WithMany()
                .HasForeignKey(service => service.MachineryId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(service => service.Operator)
                .WithMany()
                .HasForeignKey(service => service.OperatorId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ServiceRequest>(entity =>
        {
            entity.Property(request => request.EstimatedSize).HasPrecision(18, 4);
            entity.Property(request => request.TotalCost).HasPrecision(18, 2);
            entity.Property(request => request.FarmLocation)
                .HasColumnType("geometry(Point, 4326)");

            entity.HasOne(request => request.Farmer)
                .WithMany()
                .HasForeignKey(request => request.FarmerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(request => request.Service)
                .WithMany()
                .HasForeignKey(request => request.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ServiceExecution>(entity =>
        {
            entity.HasOne(execution => execution.ServiceRequest)
                .WithOne(request => request.ServiceExecution)
                .HasForeignKey<ServiceExecution>(execution => execution.ServiceRequestId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasOne(review => review.ServiceRequest)
                .WithMany()
                .HasForeignKey(review => review.ServiceRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(review => review.Reviewer)
                .WithMany()
                .HasForeignKey(review => review.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(review => review.Reviewee)
                .WithMany()
                .HasForeignKey(review => review.RevieweeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
