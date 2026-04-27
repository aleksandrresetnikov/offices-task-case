using System.Text.Json.Serialization;

namespace Offices.Models.Parsing;

public class WorktableItemDto
{
    [JsonPropertyName("timetable")]
    public string? Timetable { get; set; }
}

public class WorktablesDto
{
    [JsonPropertyName("worktable")]
    public List<WorktableItemDto>? WorktableList { get; set; }
}

public class CalcScheduleDto
{
    [JsonPropertyName("derival")]
    public string? Derival { get; set; }
}

public class PhoneDto
{
    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }
}

public class TerminalDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("fullAddress")]
    public string FullAddress { get; set; }

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; }

    [JsonPropertyName("isPVZ")]
    public bool IsPvz { get; set; }

    [JsonPropertyName("isOffice")]
    public bool IsOffice { get; set; }

    [JsonPropertyName("phones")]
    public List<PhoneDto> Phones { get; set; } = [];
    
    [JsonPropertyName("worktables")]
    public WorktablesDto? Worktables { get; set; }

    [JsonPropertyName("calcSchedule")]
    public CalcScheduleDto? CalcSchedule { get; set; }
}

public class TerminalsWrapperDto
{
    [JsonPropertyName("terminal")]
    public List<TerminalDto> Terminals { get; set; } = [];
}

public class CityDto
{
    [JsonPropertyName("cityID")]
    public int? CityId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("terminals")]
    public TerminalsWrapperDto TerminalsWrapper { get; set; }
}

public class TerminalsRootDto
{
    [JsonPropertyName("city")]
    public List<CityDto> Cities { get; set; } = [];
}