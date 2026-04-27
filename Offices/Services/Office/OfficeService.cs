using AutoMapper;

using Offices.DataAccess.Providers;
using Offices.Models.Dto.Offices;
using Offices.Models.Dto.Shared;

namespace Offices.Services.Office;

internal class OfficeService(OfficeProvider _officeProvider, IMapper _mapper) : IOfficeService
{
    public async Task<OfficesListResponse> GetOfficesAsync(GetOfficeByCityRequest req, PaginationRequest pagination, CancellationToken ct)
    {
        int page = pagination.Page > 0 ? pagination.Page : 1;
        int count = pagination.Count > 0 ? pagination.Count : 20;
        int skip = (page - 1) * count;

        var (items, totalCount) = await _officeProvider.SearchByLocationAsync(req.CityName, req.RegionName, skip, count, ct);
        var dtos = _mapper.Map<List<OfficeResponse>>(items);

        return new OfficesListResponse
        { 
            Data = dtos, 
            TotalCount = totalCount, 
            Page = page, 
            Count = count 
        };
    }

    public async Task<OfficeCityCodeResponse> GetCityCodeAsync(GetOfficeByCityRequest req, CancellationToken ct)
    {
        var cityCode = await _officeProvider.GetCityCodeByLocationAsync(req.CityName, req.RegionName, ct);
        
        return new OfficeCityCodeResponse 
        { CityCode = cityCode };
    }
}