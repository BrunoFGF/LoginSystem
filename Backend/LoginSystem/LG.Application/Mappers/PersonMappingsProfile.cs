using AutoMapper;
using LG.Application.Dtos.Person.Response;
using LG.Domain.Commons.Bases.Response;
using LG.Domain.Entities;

namespace LG.Application.Mappers
{
    public class PersonMappingsProfile : Profile
    {
        public PersonMappingsProfile()
        {
            CreateMap<Person, PersonResponseDto>()
                .ForMember(x => x.PersonId, x => x.MapFrom(y => y.Id))
                .ReverseMap();

            CreateMap<BaseEntityResponse<Person>, BaseEntityResponse<PersonResponseDto>>()
                .ReverseMap();
        }
    }
}
