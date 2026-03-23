using Microsoft.EntityFrameworkCore;
using Tcc.Core.Entities;

namespace Tcc.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<PersonEntity> Persons => Set<PersonEntity>();
    public DbSet<ApprovalEntity> Approvals => Set<ApprovalEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PersonEntity>(entity =>
        {
            entity.ToTable("Persons");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.DateOfBirth)
                .IsRequired();

            entity.Property(x => x.Address)
                .HasMaxLength(500);

            entity.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.CreatedDate)
                .IsRequired();

            entity.Property(x => x.UpdatedBy)
                .HasMaxLength(100);

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.HasQueryFilter(x => x.IsActive);
        });

        modelBuilder.Entity<ApprovalEntity>(entity =>
        {
            entity.ToTable("Approval");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ItemName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.RequestReason)
                .HasMaxLength(1000);

            entity.Property(x => x.StatusCode)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.ApprovedReason)
                .HasMaxLength(1000);

            entity.Property(x => x.RejectedReason)
                .HasMaxLength(1000);

            entity.Property(x => x.ActionBy)
                .HasMaxLength(100);

            entity.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.CreatedDate)
                .IsRequired();

            entity.Property(x => x.UpdatedBy)
                .HasMaxLength(100);

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.HasQueryFilter(x => x.IsActive);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = string.IsNullOrWhiteSpace(entry.Entity.CreatedBy)
                    ? "SYSTEM"
                    : entry.Entity.CreatedBy;

                entry.Entity.CreatedDate = DateTime.UtcNow;
                entry.Entity.IsActive = true;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedBy = "SYSTEM";
                entry.Entity.UpdatedDate = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}