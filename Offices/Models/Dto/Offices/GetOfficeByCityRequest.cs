using Microsoft.AspNetCore.Mvc;

namespace Offices.Models.Dto.Offices;

public class GetOfficeByCityRequest
{
    [FromQuery(Name = "cityName")]
    public string? CityName { get; set; }
    
    [FromQuery(Name = "regionName")]
    public string? RegionName { get; set; }
}