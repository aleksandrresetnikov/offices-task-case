using System.Text.Json.Serialization;

namespace Offices.Models.Parsing;

internal sealed class TerminalsWrapperParsingDto
{
    [JsonPropertyName("terminal")]
    public List<TerminalParsingDto> Terminals { get; set; } = [];
}