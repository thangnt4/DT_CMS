using DT_CMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DT_CMS.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<ChucVu> ChucVus => Set<ChucVu>();
    public DbSet<SanPham> SanPhams => Set<SanPham>();
    public DbSet<TinTuc> TinTucs => Set<TinTuc>();
    public DbSet<LoaiTinTuc> LoaiTinTucs => Set<LoaiTinTuc>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
