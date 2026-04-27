using Microsoft.AspNetCore.StaticFiles;
using Offices.DataAccess;
using Offices.Models.Settings;

namespace Offices;

public class Program
{
    public static void Main(string[] args)
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
        
        // Настройка Cors
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
    }
    
    private static void SettingUpDatabase(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DbContextBase>((serviceProvider, options) => 
        {
            // todo
        });
    }
}
