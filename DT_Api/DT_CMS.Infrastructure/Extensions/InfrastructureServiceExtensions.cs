using DT_CMS.Core.Interfaces.Services;
using DT_CMS.Infrastructure.Data;
using DT_CMS.Infrastructure.Security;
using DT_CMS.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DT_CMS.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<DbSeeder>();

        return services;
    }
}
