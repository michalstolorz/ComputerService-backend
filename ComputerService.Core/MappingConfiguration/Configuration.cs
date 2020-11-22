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
                .ForMember(dest => dest.PartsUsedInRepair,
                    opt => opt.MapFrom(src => src.UsedParts))
                .ForMember(dest => dest.RepairTypes,
                    opt => opt.MapFrom(src => src.RequiredRepairTypes));
            CreateMap<UsedPart, Part>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Part.Name))
                 .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.Quantity));
            CreateMap<RequiredRepairType, RepairType>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.RepairType.Name));
            CreateMap<Repair, GetRepairsResponse>()
                .ForMember(dest => dest.CustomerFirstName,
                    opt => opt.MapFrom(src => src.Customer.FirstName))
                .ForMember(dest => dest.CustomerLastName,
                    opt => opt.MapFrom(src => src.Customer.LastName))
                .ForMember(dest => dest.CustomerEmail,
                    opt => opt.MapFrom(src => src.Customer.Email))
                .ForMember(dest => dest.CustomerPhoneNumber,
                    opt => opt.MapFrom(src => src.Customer.PhoneNumber));

        }
    }
}
