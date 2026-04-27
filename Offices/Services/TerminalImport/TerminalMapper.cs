using System.Globalization;
using Offices.Models.Entities;
using Offices.Models.Parsing;
using Offices.Services.Common;

namespace Offices.Services.TerminalImport;

/// <summary>
/// 
/// </summary>
public static class TerminalMapper
{
    public static List<Office> Map(TerminalsRootDto rootData)
    {
        var offices = new List<Office>();

        foreach (var city in rootData.Cities)
        {
            if (city.TerminalsWrapper?.Terminals == null) continue;

            foreach (var terminal in city.TerminalsWrapper.Terminals)
            {
                var parsedAddress = AddressParser.Parse(terminal.Address, terminal.FullAddress);

                _ = double.TryParse(terminal.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat);
                _ = double.TryParse(terminal.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon);
                
                offices.Add(new Office
                {
                    Code = terminal.Id,
                    Uuid = Guid.NewGuid().ToString(),
                    CityCode = city.CityId ?? 0,
                    Type = GetOfficeType(terminal),
                    CountryCode = "RU",
                    Coordinates = new Coordinates { Latitude = lat, Longitude = lon },
                    
                    // Адреса
                    AddressCity = city.Name,
                    AddressRegion = parsedAddress.Region,
                    AddressStreet = parsedAddress.Street,
                    AddressHouseNumber = parsedAddress.HouseNumber,
                    AddressApartment = parsedAddress.Apartment,
                    
                    // WorkTime
                    WorkTime = GetWorkTime(terminal),
                    
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    
                    Phones = terminal.Phones?.Select(p => new Phone
                    {
                        PhoneNumber = p.Number ?? string.Empty,
                        Additional = p.Comment,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    }).ToList() ?? []
                });
            }
        }

        return offices;
    }

    private static OfficeType? GetOfficeType(TerminalDto terminal)
    {
        if (terminal.IsPvz) return OfficeType.PVZ;
        if (terminal.IsOffice) return OfficeType.POSTAMAT; 
        
        return OfficeType.WAREHOUSE;
    }

    private static string GetWorkTime(TerminalDto terminal)
    {
        // ищем в worktable
        var timetable = terminal.Worktables?.WorktableList?.FirstOrDefault()?.Timetable;
        if (!string.IsNullOrWhiteSpace(timetable)) return timetable;

        // ищем в calcSchedule
        var derival = terminal.CalcSchedule?.Derival;
        if (!string.IsNullOrWhiteSpace(derival)) return derival;

        return "Не указано";
    }
}