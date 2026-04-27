using Offices.Models.Settings;

namespace Offices.Extensions;

internal static class CorsExtensions
{
    public static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var originSettings = configuration
            .GetSection(nameof(OriginSettings))
            .Get<OriginSettings>();
        
        if (originSettings == null) return services;
        
        // настройка Cors
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(originSettings.GetOriginsArray())
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowedToAllowWildcardSubdomains();
            });
        });

        return services;
    }
}