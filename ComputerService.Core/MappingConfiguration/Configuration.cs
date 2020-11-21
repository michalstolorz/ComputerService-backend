using AutoMapper;
using ComputerService.Core.Dto.Response;
using ComputerService.Data.Models;

namespace ComputerService.Core.MappingConfiguration
{
    public class Configuration : Profile
    {
        public Configuration()
        {
            CreateMap<Repair, RepairDetailsResponse>()
                .ForMember(dest => dest.CustomerFirstName,
                    opt => opt.MapFrom(src => src.Customer.FirstName))
                .ForMember(dest => dest.CustomerLastName,
                    opt => opt.MapFrom(src => src.Customer.LastName))
                .ForMember(dest => dest.CustomerEmail,
                    opt => opt.MapFrom(src => src.Customer.Email))
                .ForMember(dest => dest.CustomerPhoneNumber,
                    opt => opt.MapFrom(src => src.Customer.PhoneNumber))
                ;

        }
    }
}
