using System.Text.RegularExpressions;

namespace Offices.Services.TerminalImport;

public record ParsedAddress(string? Region, string? Street, string? HouseNumber, int? Apartment);

/// <summary>
/// Упрощенный парсер адресов.
/// partial - нужен для скомпилированных регулярных выражений
///
/// Вообще сложность в том, что в РФ сама структура адресов ооочень разниться.
/// В json-е на address и fullAddress, за это можно зацепиться и, предполагая, что в address
/// лежит что-то вроде улицы и дома, вытянуть эти значения.
///
/// В идеале, для этого кейса использовать сторнние сервисы (вроде DaData нашел, или у
/// апи Яндекса должно быть что-то) 
/// </summary>
internal static partial class AddressParser
{
    // регексы:
    [GeneratedRegex(@"(?:дом\s*№?|д\.)\s*([0-9А-Яа-яA-Za-z\-/]+)", RegexOptions.IgnoreCase)]
    private static partial Regex HouseRegex();

    [GeneratedRegex(@"(?:корп\.?|корпус|стр\.?|строение|лит\.?|литера)\s*[0-9А-Яа-яA-Za-z]+", RegexOptions.IgnoreCase)]
    private static partial Regex SubHouseRegex();

    [GeneratedRegex(@"\b(?:офис|оф\.?|пом\.?|помещение|кв\.?|каб\.?)\s*([0-9А-Яа-яA-Za-z\-]+)", RegexOptions.IgnoreCase)]
    private static partial Regex ApartmentRegex();
    
    [GeneratedRegex(@"\s+(г\.?|обл\.?|область|край|респ\.?)$", RegexOptions.IgnoreCase)]
    private static partial Regex SuffixesRegex();
    
    public static ParsedAddress Parse(string? shortAddress, string? fullAddress)
    {
        if (string.IsNullOrWhiteSpace(fullAddress))
            return new ParsedAddress(null, null, null, null);

        var parts = fullAddress.Split(',', StringSplitOptions.TrimEntries);

        // регион
        string? region = parts.Length > 1 ? CleanSuffixes(parts[1]) : null;

        // улица. обычно первый элемент :)
        string? street = parts.Length > 2 ? parts[2] : null;

        // номер дома. обычно последний элемент :(
        string? house = ExtractHouse(fullAddress);

        // помещение
        int? apartment = ExtractApartmentInt(fullAddress);

        return new ParsedAddress(region, street, house, apartment);
    }
    
    private static string CleanSuffixes(string input)
    {
        // убираем "г.", "область" и тд
        return SuffixesRegex().Replace(input, string.Empty).Trim();
    }

    private static string? ExtractHouse(string input)
    {
        // основная часть
        var houseMatch = HouseRegex().Match(input);
        if (!houseMatch.Success) return null;

        var result = houseMatch.Groups[1].Value;
        
        // корпус/строение/литеру, если они есть
        var subMatch = SubHouseRegex().Match(input);
        if (subMatch.Success) result += $" {subMatch.Groups[0].Value.Trim()}";
        
        return result.Trim().TrimEnd(',');
    }
    
    private static int? ExtractApartmentInt(string input)
    {
        var match = ApartmentRegex().Match(input);
        if (!match.Success) return null;
        
        // берем только цифры, если там что-то вроде "12-н"
        var digits = Regex.Match(match.Groups[1].Value, @"\d+");
        if (digits.Success && int.TryParse(digits.Value, out int val))
            return val;
        
        return null;
    }
}