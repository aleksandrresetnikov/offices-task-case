using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Offices.DataAccess;
using Offices.DataAccess.Providers;
using Offices.Models.Settings;
using Offices.Services.Hosted;
using Offices.Services.Middleware;
using Offices.Services.Office;
using Offices.Services.TerminalImport;

namespace Offices;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Базовые сервисы
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        
        // AutoMapper
        builder.Services.AddAutoMapper(_ => { }, typeof(Program).Assembly);
        
        // Настраиваем Корс, DI, и т.д.
        SettingUpSettings(builder);
        SettingUpSwagger(builder);
        SettingUpCors(builder);
        SettingUpContexts(builder);
        SettingUpComponents(builder);
        SettingUpDatabase(builder);

        var app = builder.Build();
        
        // Автоматический накат миграций
        await ApplyAutoMigrationAsync(app);
        
        // ловушка для http-ошибок и необработанных исключений
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
    
    private static void SettingUpSettings(WebApplicationBuilder builder)
    {
        builder.Services.Configure<DbConnectionSettings>(
            builder.Configuration.GetSection(nameof(DbConnectionSettings)));
        
        builder.Services.Configure<RuntimeSettings>(
            builder.Configuration.GetSection(nameof(RuntimeSettings)));
        
        builder.Services.Configure<OriginSettings>(
            builder.Configuration.GetSection(nameof(OriginSettings)));
        
        builder.Services.Configure<ImportSettings>(
            builder.Configuration.GetSection(nameof(ImportSettings)));
    }
    
    private static void SettingUpSwagger(WebApplicationBuilder builder)
    {
        // todo
    }
    
    private static void SettingUpCors(WebApplicationBuilder builder)
    {
        var originSettings = builder.Configuration
            .GetSection(nameof(OriginSettings))
            .Get<OriginSettings>();
        
        if (originSettings == null) return;
        
        // настройка Cors
        builder.Services.AddCors(options =>
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
    }
    
    private static void SettingUpContexts(WebApplicationBuilder builder)
    {
        // todo
    }
    
    private static void SettingUpComponents(WebApplicationBuilder builder)
    {
        // разное
        builder.Services.AddTransient<FileExtensionContentTypeProvider>();
        
        // бд-провайдеры
        builder.Services.AddScoped<OfficeProvider>();
        builder.Services.AddScoped<PhoneProvider>();
        
        // сервисы
        builder.Services.AddScoped<ITerminalImportService, TerminalImportService>();
        builder.Services.AddScoped<IOfficeService, OfficeService>();
        
        // фоновые сервисы
        builder.Services.AddHostedService<TerminalImportBackgroundService>();
    }
    
    private static void SettingUpDatabase(WebApplicationBuilder builder)
    {
        var databaseSettings = builder.Configuration
            .GetSection(nameof(DbConnectionSettings))
            .Get<DbConnectionSettings>();
        
        if (databaseSettings == null) return;
        
        builder.Services.AddDbContext<DellinDictionaryDbContext>((serviceProvider, options) => 
        {
            options.UseNpgsql(databaseSettings.ConnectionString);
        });
    }

    private static async Task ApplyAutoMigrationAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var db = scope.ServiceProvider.GetRequiredService<DellinDictionaryDbContext>();
        await db.Database.MigrateAsync(); // проверить, есть ли новые миграции
    }
}
