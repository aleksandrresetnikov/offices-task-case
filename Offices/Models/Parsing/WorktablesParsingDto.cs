using System.Text.Json.Serialization;

namespace Offices.Models.Parsing;

internal sealed class WorktablesParsingDto
{
    [JsonPropertyName("worktable")]
    public List<WorktableItemParsingDto>? WorktableList { get; set; }
}