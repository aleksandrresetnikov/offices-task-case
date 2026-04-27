using System.Text.Json.Serialization;

namespace Offices.Models.Parsing;

internal sealed class CalcScheduleParsingDto
{
    [JsonPropertyName("derival")]
    public string? Derival { get; set; }
}