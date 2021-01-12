using AutoMapper;
using ComputerService.Core.Dto.Response;
using ComputerService.Core.Models;
using ComputerService.Data.Models;
using ComputerService.Common.Enums;
using ComputerService.Core.Dto.Request;

namespace ComputerService.Core.MappingConfiguration
{
    public class Configuration : Profile
    {
        public Configuration()
        {
            CreateMap<Repair, GetRepairDetailsResponse>()
                .ForMember(dest => dest.CustomerFirstName,
                    opt => opt.MapFrom(src => src.Customer.FirstName))
                .ForMember(dest => dest.CustomerLastName,
                    opt => opt.MapFrom(src => src.Customer.LastName))
                .ForMember(dest => dest.CustomerEmail,
                    opt => opt.MapFrom(src => src.Customer.Email))
                .ForMember(dest => dest.CustomerPhoneNumber,
                    opt => opt.MapFrom(src => src.Customer.PhoneNumber))
                .ForMember(dest => dest.UsedParts,
                    opt => opt.MapFrom(src => src.UsedParts))
                .ForMember(dest => dest.RepairTypes,
                    opt => opt.MapFrom(src => src.RequiredRepairTypes))
                .ForMember(dest => dest.RepairUsers,
                    opt => opt.MapFrom(src => src.EmployeeRepairs));
            CreateMap<UsedPart, PartsUsedInRepair>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.PartId))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Part.Name))
                .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.PartBoughtPrice,
                    opt => opt.MapFrom(src => src.Part.PartBoughtPrice));
            CreateMap<EmployeeRepair, RepairUsers>()
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.UserFirstName,
                    opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.UserLastName,
                    opt => opt.MapFrom(src => src.User.LastName));
            CreateMap<User, LoginResponse>();
            CreateMap<UsedPart, Part>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Part.Name))
                 .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.Quantity));
            CreateMap<RequiredRepairType, RepairType>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.RepairType.Name));
            CreateMap<Repair, GetRepairsResponse>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => (EnumStatus)src.Status))
                 .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
                 .ForMember(dest => dest.CustomerEmail,
                    opt => opt.MapFrom(src => src.Customer.Email))
                 .ForMember(dest => dest.CustomerPhoneNumber,
                    opt => opt.MapFrom(src => src.Customer.PhoneNumber))
                 .ForMember(dest => dest.RepairUsers,
                    opt => opt.MapFrom(src => src.EmployeeRepairs));
            CreateMap<Part, PartModel>().ReverseMap();
            CreateMap<RepairType, RepairTypeModel>()
                .ForMember(dest => dest.RequiredRepairTypesModel,
                    opt => opt.MapFrom(src => src.RequiredRepairTypes));
            CreateMap<Part, PartModel>()
                .ForMember(dest => dest.UsedPartsModel,
                    opt => opt.MapFrom(src => src.UsedParts));
            CreateMap<RequiredRepairType, RequiredRepairTypeModel>()
                .ForPath(dest => dest.RepairTypeModel.Name,
                    opt => opt.MapFrom(src => src.RepairType.Name))
                .ReverseMap();
            CreateMap<RegisterRequest, User>()
               .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));
            CreateMap<Role, RoleResponse>(); 
            CreateMap<User, GetCustomersResponse>()
                 .ForMember(dest => dest.CustomerFirstName,
                     opt => opt.MapFrom(src => src.FirstName))
                 .ForMember(dest => dest.CustomerLastName,
                     opt => opt.MapFrom(src => src.LastName))
                 .ForMember(dest => dest.CustomerEmail,
                     opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.CustomerPhoneNumber,
                     opt => opt.MapFrom(src => src.PhoneNumber));
            CreateMap<Repair, RepairModel>()
                 .ForMember(dest => dest.CustomerModel,
                     opt => opt.MapFrom(src => src.Customer))
                 .ForMember(dest => dest.InvoiceModel,
                     opt => opt.MapFrom(src => src.Invoice));
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
