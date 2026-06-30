using DT_CMS.Core.Entities;
using DT_CMS.Core.Interfaces.Services;
using DT_CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DT_CMS.Infrastructure.Seeds;

/// <summary>
/// Seeds a default admin account (admin / Admin@123) on first run so the
/// CRUD screens can be exercised immediately without touching SQL by hand.
/// Assumes the schema already exists (see sql_create_tables.sql at the
/// solution root).
/// </summary>
public class DbSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public DbSeeder(ApplicationDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        await SeedAdminUserAsync();
    }

    private async Task SeedAdminUserAsync()
    {
        var adminExists = await _db.Users.AnyAsync(u => u.Username == "admin");
        if (adminExists) return;

        _db.Users.Add(new User
        {
            Username = "admin",
            PasswordHash = _passwordHasher.Hash("Admin@123"),
            FullName = "System Administrator",
            Email = "admin@dtcms.local",
            IsActive = true
        });

        await _db.SaveChangesAsync();
    }
}
