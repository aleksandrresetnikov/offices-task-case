using System.Text.Json.Serialization;

namespace Offices.Models.Parsing;

internal sealed class CityParsingDto
{
    [JsonPropertyName("cityID")]
    public int? CityId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("terminals")]
    public TerminalsWrapperParsingDto TerminalsWrapper { get; set; }
}