using Microsoft.EntityFrameworkCore;
using PRChecksheetApp.Models;

namespace PRChecksheetApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<PurchaseRequisitionHeader> PurchaseRequisitionHeaders { get; set; }
    public DbSet<PurchaseRequisitionItem> PurchaseRequisitionItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PurchaseRequisitionHeader>(entity =>
        {
            entity.HasIndex(x => x.PRRequestId).IsUnique();
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
            entity.Property(x => x.AttachmentPath).HasMaxLength(500);

            entity.HasMany(x => x.Items)
                .WithOne(x => x.Header)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PurchaseRequisitionItem>(entity =>
        {
            entity.Property(x => x.Quantity).HasPrecision(18, 2);
            entity.Property(x => x.ValuationPrice).HasPrecision(18, 2);
            entity.Property(x => x.TotalValue).HasPrecision(18, 2);
            entity.Property(x => x.QuantityAccountAssignment).HasPrecision(18, 2);
        });
    }
}
