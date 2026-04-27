using System.Text.Json;
using Microsoft.EntityFrameworkCore;

using Offices.DataAccess;
using Offices.Models.Parsing;

namespace Offices.Services.TerminalImport;

internal class TerminalImportService(DellinDictionaryDbContext _dbContext, ILogger<TerminalImportService> _logger,
    IHostEnvironment _env) : ITerminalImportService
{
    public async Task ImportAsync(CancellationToken ct = default)
    {
        // _env.ContentRootPath - рабочая дериктория проекта
        var filePath = Path.Combine(_env.ContentRootPath, "files", "terminals.json");

        if (!File.Exists(filePath))
        {
            _logger.LogError("{Time} ERROR: Ошибка импорта: {Exception}", 
                DateTime.Now, $"Файл не найден по пути: {filePath}");
            return;
        }

        try
        {
            await HandleImportAsync(filePath, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Time} ERROR: Ошибка импорта: {Exception}", DateTime.Now, ex.Message);
            throw;
        }
    }

    private async Task HandleImportAsync(string filePath, CancellationToken ct)
    {
        // загружаем JSON
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
        // загружаем рут
        var rootData = await JsonSerializer.DeserializeAsync<TerminalsRootParsingDto>(fileStream, options, ct);

        // если в руте нет массива "city" - ливаем
        if (rootData?.Cities == null || rootData.Cities.Count == 0)
        {
            _logger.LogWarning("{Time} INFO: Загружено 0 терминалов из JSON (пустой файл)", DateTime.Now);
            return;
        }

        // маппинг
        var newOffices = TerminalMapper.Map(rootData);
        _logger.LogInformation("{Time} INFO: Загружено {Count} терминалов из JSON", 
            DateTime.Now, newOffices.Count);

        // очистка старых данных
        var oldCount = await _dbContext.Offices.CountAsync(ct);
        await _dbContext.Offices.ExecuteDeleteAsync(ct);
        _logger.LogInformation("{Time} INFO: Удалено {OldCount} старых записей", DateTime.Now, oldCount);

        // сохранение новых данных
        await _dbContext.Offices.AddRangeAsync(newOffices, ct);
        await _dbContext.SaveChangesAsync(ct);
        _logger.LogInformation("{Time} INFO: Сохранено {NewCount} новых терминалов", 
            DateTime.Now, newOffices.Count);
    }
}