using AutoMapper;
using GymManagmentSystem_BLL.ViewModels.Member;
using GymManagmentSystem_BLL.ViewModels.Session;
using GymManagmentSystem_DAL.Data.Models;

namespace GymManagmentSystem_BLL.Services.Classes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Session mappings
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            // Member mappings
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNo = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));

            CreateMap<Member, MemberViewModel>();

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNo))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address ??= new Address();
                    dest.Address.BuildingNo = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                });
        }
    }
}