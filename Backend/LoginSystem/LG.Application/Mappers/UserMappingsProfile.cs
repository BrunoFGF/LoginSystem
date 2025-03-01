using AutoMapper;
using LG.Application.Dtos.User.Request;
using LG.Application.Dtos.User.Response;
using LG.Domain.Entities;
using LG.Infrastructure.Commons.Bases.Response;

namespace LG.Application.Mappers
{
    public class UserMappingsProfile : Profile
    {
        public UserMappingsProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Person, x => x.MapFrom(y => y.Person))
                .ForMember(x => x.Status, x => x.MapFrom(y => y.Status!.Equals("ACTIVO") ? "ACTIVO" : "INACTIVO"))
                .ReverseMap();

            CreateMap<BaseEntityResponse<User>, BaseEntityResponse<UserResponseDto>>()
                .ReverseMap();

            CreateMap<UserRequestDto, User>();

            CreateMap<User, UserSelectResponseDto>()
                .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id))
                .ReverseMap();
        }
    }
}
