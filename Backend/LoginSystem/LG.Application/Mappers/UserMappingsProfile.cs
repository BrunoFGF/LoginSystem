using AutoMapper;
using LG.Application.Dtos.User.Request;
using LG.Application.Dtos.User.Response;
using LG.Domain.Commons.Bases.Response;
using LG.Domain.Entities;

namespace LG.Application.Mappers
{
    public class UserMappingsProfile : Profile
    {
        public UserMappingsProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Person, x => x.MapFrom(y => y.Person))
                .ForMember(x => x.Status, x => x.MapFrom(y => y.Status!.Equals("Active") ? "ACTIVO" : "INACTIVO"))
                .ReverseMap();

            CreateMap<UserRequestDto, Person>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.IdentityCard, opt => opt.MapFrom(src => src.IdentityCard))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate));

            

            CreateMap<BaseEntityResponse<User>, BaseEntityResponse<UserResponseDto>>()
                .ReverseMap();

            CreateMap<UserRequestDto, User>();
            CreateMap<UpdateUserRequestDto, User>();
            CreateMap<CreateUserRequestDto, User>();

            CreateMap<User, UserSelectResponseDto>()
                .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id))
                .ReverseMap();

            CreateMap<TokenRequestDto, User>();
        }
    }
}
