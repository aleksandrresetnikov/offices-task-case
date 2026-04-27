using System.Text.Json.Serialization;

namespace Offices.Models.Parsing;

internal sealed class TerminalsRootParsingDto
{
    [JsonPropertyName("city")]
    public List<CityParsingDto> Cities { get; set; } = [];
}