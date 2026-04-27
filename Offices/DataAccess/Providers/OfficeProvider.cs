using Microsoft.EntityFrameworkCore;
using Offices.Models.Entities;

namespace Offices.DataAccess.Providers;

public class OfficeProvider : ProviderBase<Office>
{
    public OfficeProvider(DellinDictionaryDbContext context) : base(context)
    {
    }
    
    /// <summary>
    /// Поиск офисов с фильтром и пагинацией
    /// </summary>
    public async Task<(List<Office> Items, int TotalCount)> SearchByLocationAsync(string? cityName, string? regionName, 
        int skip, int take, CancellationToken ct = default)
    {
        // базовый query, без доп. where
        var query = GetDbSet().AsNoTracking();

        // еслм есть cityName -> добавляем where
        if (!string.IsNullOrWhiteSpace(cityName))
        {
            var escapedCity = EscapeLikePattern(cityName);
            query = query.Where(o => o.AddressCity != null && EF.Functions.ILike(o.AddressCity, 
                $"%{escapedCity}%", @"\"));
        }
        
        // еслм есть regionName -> добавляем where
        if (!string.IsNullOrWhiteSpace(regionName))
        {
            var escapedRegion = EscapeLikePattern(regionName);
            query = query.Where(o => o.AddressRegion != null && EF.Functions.ILike(o.AddressRegion, 
                $"%{escapedRegion}%", @"\"));
        }

        // считаем общее количество (для пагинации)
        var totalCount = await query.CountAsync(ct);

        // получаем данные
        var items = await query
            .Include(o => o.Phones) // инклудим телефоны
            .OrderBy(o => o.Id)     // сортировка
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Поиск кода города
    /// </summary>
    public async Task<int?> GetCityCodeByLocationAsync(string? cityName, string? regionName, CancellationToken ct = default)
    {
        // базовый query, без доп. where
        var query = GetDbSet()
            .AsNoTracking();

        // еслм есть cityName -> добавляем where
        if (!string.IsNullOrWhiteSpace(cityName))
        {
            var escapedCity = EscapeLikePattern(cityName);
            query = query.Where(o => o.AddressCity != null && EF.Functions.ILike(o.AddressCity, 
                $"%{escapedCity}%", @"\"));
        }

        // еслм есть regionName -> добавляем where
        if (!string.IsNullOrWhiteSpace(regionName))
        {
            var escapedRegion = EscapeLikePattern(regionName);
            query = query.Where(o => o.AddressRegion != null && EF.Functions.ILike(o.AddressRegion, 
                $"%{escapedRegion}%", @"\"));
        }

        // вытягиваем только CityCode
        return await query
            .Select(o => (int?)o.CityCode)
            .FirstOrDefaultAsync(ct);
    }

    /// <summary>
    /// Форматирование спецсимволов для LIKE
    /// </summary>
    private static string EscapeLikePattern(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        
        return value.Trim()
            .Replace(@"\", @"\\")
            .Replace("%", @"\%")
            .Replace("_", @"\_");
    }
}