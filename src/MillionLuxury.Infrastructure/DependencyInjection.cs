namespace MillionLuxury.Infrastructure;

#region Usings
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MillionLuxury.Application.Common.Abstractions.Authentication;
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Abstractions.Data;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Users;
using MillionLuxury.Infrastructure.Auth;
using MillionLuxury.Infrastructure.Clock;
using MillionLuxury.Infrastructure.Database;
using MillionLuxury.Infrastructure.Database.Repositories;
#endregion

public static class DependencyInjection
{
    #region Constants
    private const string DbConnectionString = "SqlServerConnection";
    #endregion

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        AddPersistence(services, configuration);
        AddAuthentication(services);
        AddAuthorization(services);

        return services;
    }

    private static void AddPersistence(
        IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(DbConnectionString) ??
                               throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString).UseSnakeCaseNamingConvention());

        // Persistence for entities
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<IDbConnectionFactory>(_ =>
            new DbConnectionFactory(connectionString));
    }

    private static void AddAuthentication(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddTransient<IHashingService, HashingService>();
        services.AddScoped<IUserContext, UserContext>();
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
    }
}
