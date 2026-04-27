using AutoMapper;
using Offices.Models.Dto.Offices;
using Offices.Models.Dto.Phones;
using Offices.Models.Entities;

namespace Offices;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Office, OfficeResponse>();
        CreateMap<Phone, PhoneResponse>();
    }
}