using DT_CMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DT_CMS.Infrastructure.Configurations;

public class SanPhamConfiguration : IEntityTypeConfiguration<SanPham>
{
    public void Configure(EntityTypeBuilder<SanPham> builder)
    {
        builder.ToTable("Dm_SanPham");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.TenDuLieu).HasMaxLength(500).IsRequired();
        builder.Property(c => c.GhiChu);
        builder.Property(c => c.FileDinhKem).HasMaxLength(500);
        builder.Property(c => c.TrangThai).HasDefaultValue(1);
        builder.Property(c => c.NgayTao).HasDefaultValueSql("SYSUTCDATETIME()");
        builder.Property(c => c.NgayCapNhat).HasColumnType("date");
        builder.Property(c => c.Xoa).HasDefaultValue(false);
        builder.HasQueryFilter(c => c.Xoa != true);
    }
}
