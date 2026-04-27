using Microsoft.AspNetCore.Mvc;
using Offices.Models.Dto.Offices;
using Offices.Models.Dto.Shared;
using Offices.Services.Office;

namespace Offices.Controllers;

[ApiController]
[Route("v1/api/offices")]
public class OfficesController(IOfficeService _officeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<OfficesListResponse>> GetOfficesAsync([FromQuery] GetOfficeByCityRequest req,
        [FromQuery] PaginationRequest pagination, CancellationToken ct)
    {
        var resultPayload = await _officeService.GetOfficesAsync(req, pagination, ct);
        return Ok(resultPayload);
    }
    
    [HttpGet("city-code")]
    public async Task<ActionResult<OfficeCityCodeResponse>> GetCityCodeAsync([FromQuery] GetOfficeByCityRequest req,
        CancellationToken ct)
    {
        var resultPayload = await _officeService.GetCityCodeAsync(req, ct);
        return Ok(resultPayload);
    }
}