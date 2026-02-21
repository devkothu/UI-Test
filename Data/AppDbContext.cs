using Microsoft.EntityFrameworkCore;
using UI_Test.Models;

namespace UI_Test.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ShortUrlEntry> ShortUrls => Set<ShortUrlEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortUrlEntry>(entity =>
        {
            entity.ToTable("ShortUrls");
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsRequired();
            entity.Property(e => e.OriginalUrl)
                .HasMaxLength(2048)
                .IsRequired();
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.VisitCount)
                .HasDefaultValue(0);
            entity.HasIndex(e => e.CreatedAtUtc)
                .HasDatabaseName("IX_ShortUrls_CreatedAtUtc");
        });
    }
}
