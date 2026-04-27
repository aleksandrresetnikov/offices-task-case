using Offices.Models.Dto.Offices;
using Offices.Models.Dto.Shared;

namespace Offices.Services.Office;

public interface IOfficeService
{
    /// <summary>
    /// Возвращает список офисов по локации
    /// </summary>
    Task<OfficesListResponse> GetOfficesAsync(GetOfficeByCityRequest req, PaginationRequest pagination, CancellationToken ct);

    /// <summary>
    /// Возвращает код города по его названию и региону
    /// </summary>
    Task<OfficeCityCodeResponse> GetCityCodeAsync(GetOfficeByCityRequest req, CancellationToken ct);
}