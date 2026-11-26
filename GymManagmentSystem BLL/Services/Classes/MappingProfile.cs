using AutoMapper;
using GymManagmentSystem_BLL.ViewModels;
using GymManagmentSystem_BLL.ViewModels.Member;
using GymManagmentSystem_BLL.ViewModels.Plan;
using GymManagmentSystem_BLL.ViewModels.Session;
using GymManagmentSystem_BLL.ViewModels.Trainer;
using GymManagmentSystem_DAL.Data.Models;

namespace GymManagmentSystem_BLL.Services.Classes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Session mappings
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer != null ? src.Trainer.Name : string.Empty))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, EditSessionViewModel>().ReverseMap();

            // HealthRecord mappings
            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            // Member mappings
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNo = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));

            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    src.Address != null
                        ? $"{src.Address.BuildingNo} - {src.Address.Street} - {src.Address.City}"
                        : string.Empty));

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address != null ? src.Address.BuildingNo : 0))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : string.Empty))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address != null ? src.Address.Street : string.Empty));

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

            // Plan mappings
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, EditPlanViewModel>();
            CreateMap<EditPlanViewModel, Plan>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.Ignore());

            // Trainer mappings
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    src.Address != null
                        ? $"{src.Address.BuildingNo} - {src.Address.Street} - {src.Address.City}"
                        : "N/A"));

            CreateMap<CreateTrainerViewModel, Trainer>();

            CreateMap<Trainer, EditTrainerViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address != null ? src.Address.BuildingNo : 0))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address != null ? src.Address.Street : string.Empty))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : string.Empty));

            CreateMap<EditTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.DateOfBirth, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, opt => opt.Ignore())
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