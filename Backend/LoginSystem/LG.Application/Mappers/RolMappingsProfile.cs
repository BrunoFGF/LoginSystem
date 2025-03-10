using AutoMapper;
using LG.Application.Dtos.Role.Response;
using LG.Domain.Entities;

namespace LG.Application.Mappers
{
    public class RolMappingsProfile : Profile
    {
        public RolMappingsProfile()
        {
            CreateMap<Rol, RoleResponseDto>();
        }
    }
}
