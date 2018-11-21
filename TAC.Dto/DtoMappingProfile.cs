using AutoMapper;
using TAC.Domain;

namespace TAC.Dto
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<Customer, CustomerInfo>().ReverseMap();
            CreateMap<Vehicle, VehicleInfo>().ReverseMap();
        }
    }
}
