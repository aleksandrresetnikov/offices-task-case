using System.Text.Json.Serialization;

namespace Offices.Models.Parsing;

internal sealed class PhoneParsingDto
{
    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }
}