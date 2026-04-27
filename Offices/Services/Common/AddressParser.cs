using System.Text.RegularExpressions;

namespace Offices.Services.Common;

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
public static partial class AddressParser
{
    public static ParsedAddress Parse(string? shortAddress, string? fullAddress)
    {
        if (string.IsNullOrWhiteSpace(fullAddress))
            return new ParsedAddress(null, null, null, null);

        var fullParts = fullAddress.Split(',', StringSplitOptions.TrimEntries);
        var shortParts = shortAddress?.Split(',', StringSplitOptions.TrimEntries) ?? [];

        // регион
        string? region = fullParts.Length > 1 ? fullParts[1] : null;

        // улица. обычно первый элемент :)
        string? street = shortParts.Length > 0 ? shortParts[0] : null;

        // номер дома. обычно последний элемент :(
        string? houseNumber = shortParts.Length > 1 ? shortParts.Last() : ExtractHouseRegex(fullAddress);

        // помещение
        int? apartment = ExtractApartmentRegex(fullAddress);

        return new ParsedAddress(region, street, houseNumber, apartment);
    }

    private static int? ExtractApartmentRegex(string input)
    {
        // ищем "офис 132", "пом. 5", "помещение 1"
        var match = ApartmentRegex().Match(input);
        if (match.Success && int.TryParse(match.Groups[1].Value, out int apt)) return apt;
        
        // если ничего не нашли - вернем null
        return null;
    }

    private static string? ExtractHouseRegex(string input)
    {
        var match = HouseRegex().Match(input);
        return match.Success ? match.Groups[1].Value.Trim() : null;
    }

    [GeneratedRegex(@"(?:офис|пом\.?|помещение|кв\.?)\s*(\d+)", RegexOptions.IgnoreCase)]
    private static partial Regex ApartmentRegex();

    [GeneratedRegex(@"(?:дом\s*№?|д\.)\s*([А-Яа-яA-Za-z0-9\-/]+)", RegexOptions.IgnoreCase)]
    private static partial Regex HouseRegex();
}