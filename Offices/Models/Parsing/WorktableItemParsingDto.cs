using System.Text.Json.Serialization;

namespace Offices.Models.Parsing;

internal sealed class WorktableItemParsingDto
{
    [JsonPropertyName("timetable")]
    public string? Timetable { get; set; }
}