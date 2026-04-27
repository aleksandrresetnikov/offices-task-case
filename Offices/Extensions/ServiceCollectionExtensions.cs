using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Offices.DataAccess;
using Offices.DataAccess.Providers;
using Offices.Models.Settings;
using Offices.Services.Hosted;
using Offices.Services.Office;
using Offices.Services.TerminalImport;

namespace Offices.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // разное
        services.AddTransient<FileExtensionContentTypeProvider>();

        // провайдеры
        services.AddScoped<OfficeProvider>();
        services.AddScoped<PhoneProvider>();
        
        // сервисы
        services.AddScoped<ITerminalImportService, TerminalImportService>();
        services.AddScoped<IOfficeService, OfficeService>();

        // фоновые сервисы
        services.AddHostedService<TerminalImportBackgroundService>();
        
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(DbConnectionSettings))
            .Get<DbConnectionSettings>();
        
        if (settings == null) throw new Exception("DB Settings not found");

        services.AddDbContext<DellinDictionaryDbContext>(options => 
            options.UseNpgsql(settings.ConnectionString));

        return services;
    }
}