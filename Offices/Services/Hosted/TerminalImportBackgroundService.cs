using Microsoft.Extensions.Options;
using Offices.Models.Settings;
using Offices.Services.TerminalImport;

namespace Offices.Services.Hosted;

public class TerminalImportBackgroundService(IServiceScopeFactory _scopeFactory, IOptions<ImportSettings> _importSettingsOptions,
    ILogger<TerminalImportBackgroundService> _logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{Time} INFO: BackgroundService импорта терминалов запущен", DateTime.Now);

        // вытягиваем интервал из конфига
        var importSettings = _importSettingsOptions.Value;
        var intervalHours = importSettings.IntervalHours;
        
        // используем PeriodicTimer, т.к. Task.Wait не учитывает время выполнения задачи
        // var period = TimeSpan.FromHours(intervalHours);
        var period = TimeSpan.FromSeconds(10);
        using var timer = new PeriodicTimer(period);

        try
        {
            // первый импорт сразу при старте приложения
            await DoWorkAsync(stoppingToken);

            // ждем...
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWorkAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("{Time} INFO: BackgroundService импорта терминалов останавливается...", DateTime.Now);
        }
    }

    private async Task DoWorkAsync(CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("{Time} INFO: Запуск цикла импорта терминалов", DateTime.Now);

            // т.к. BackgroundService - это Singleton под капотом, необходимо создать Scoped контекст вручную
            using var scope = _scopeFactory.CreateScope();
            
            // вытягиваем сервис импорта
            var importService = scope.ServiceProvider.GetRequiredService<ITerminalImportService>();

            // запускаем сам импорт
            await importService.ImportAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Time} ERROR: Критическая ошибка в фоновом импорте", DateTime.Now);
        }
    }
}