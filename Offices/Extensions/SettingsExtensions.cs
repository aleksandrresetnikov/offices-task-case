using Offices.Models.Settings;

namespace Offices.Extensions;

internal static class SettingsExtensions
{
    public static IServiceCollection AddProjectSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbConnectionSettings>(configuration.GetSection(nameof(DbConnectionSettings)));
        services.Configure<RuntimeSettings>(configuration.GetSection(nameof(RuntimeSettings)));
        services.Configure<OriginSettings>(configuration.GetSection(nameof(OriginSettings)));
        services.Configure<ImportSettings>(configuration.GetSection(nameof(ImportSettings)));

        return services;
    }
}