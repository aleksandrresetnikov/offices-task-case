using Offices.Models.Dto.Phones;
using Offices.Models.Dto.Shared;
using Offices.Models.Entities;

namespace Offices.Models.Dto.Offices;

public class OfficesListResponse : ResponseListBase<OfficeResponse>
{ }

public class OfficeResponse
{
    // Служебные
    public int Id { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdateDate { get; set; } = DateTime.UtcNow;
    
    // Код и Uuid
    public string? Code { get; set; }
    public string? Uuid { get; set; }
    
    // Тип
    public OfficeType? Type { get; set; }
    
    // Время работы
    public string WorkTime { get; set; }
    
    // Код города и страны
    public int CityCode { get; set; }
    public string CountryCode { get; set; }
    
    // Адрес:
    public string? AddressRegion { get; set; }
    public string? AddressCity { get; set; }
    public string? AddressStreet { get; set; }
    public string? AddressHouseNumber { get; set; }
    public int? AddressApartment { get; set; }
    
    // Координаты (ComplexType, в бд храняться одной сущностью в Offices)
    public Coordinates Coordinates { get; set; }
    
    // Связанные телефонные номера
    public List<PhoneResponse> Phones { get; set; } = [];
}