using Microsoft.EntityFrameworkCore;

using Offices.DataAccess;
using Offices.Extensions;
using Offices.Services.Middleware;

namespace Offices;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // базовые сервисы
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        
        // настраиваем Корс, DI, и т.д.
        builder.Services
            .AddAutoMapper(_ => { }, typeof(Program).Assembly)
            .AddProjectSettings(builder.Configuration)
            .AddCustomSwaggerGen()
            .AddCorsConfig(builder.Configuration)
            .AddDatabase(builder.Configuration)
            .AddBusinessServices()
            .AddAutoMapper(typeof(Program));

        var app = builder.Build();
        
        // автоматический накат миграций
        await ApplyAutoMigrationAsync(app);
        
        // ловушка для http-ошибок и необработанных исключений
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowFrontend");
        app.UseAuthorization();
        
        app.MapControllers();
        app.Run();
    }

    private static async Task ApplyAutoMigrationAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var db = scope.ServiceProvider.GetRequiredService<DellinDictionaryDbContext>();
        await db.Database.MigrateAsync(); // проверить, есть ли новые миграции
    }
}
