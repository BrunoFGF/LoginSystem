using AutoMapper;
using LG.Application.Dtos.Person.Request;
using LG.Application.Dtos.Person.Response;
using LG.Domain.Entities;
using LG.Infrastructure.Commons.Bases.Response;

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

            CreateMap<PersonRequestDto, Person>();

            CreateMap<Person, PersonSelectResponseDto>()
                .ForMember(x => x.PersonId, x => x.MapFrom(y => y.Id))
                .ReverseMap();
        }
    }
}
