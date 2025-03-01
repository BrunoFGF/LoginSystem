using LG.Application.Commons.Bases;
using LG.Application.Dtos.Person.Request;
using LG.Application.Dtos.Person.Response;
using LG.Infrastructure.Commons.Bases.Request;
using LG.Infrastructure.Commons.Bases.Response;

namespace LG.Application.Interfaces
{
    public interface IPersonApplication
    {
        Task<BaseResponse<BaseEntityResponse<PersonResponseDto>>> ListPersons(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<PersonSelectResponseDto>>> ListSelectPersons();
        Task<BaseResponse<PersonResponseDto>> PersonById(int personId);
        Task<BaseResponse<bool>> RegisterPerson(PersonRequestDto requestDto);
        Task<BaseResponse<bool>> EditPerson(int personId, PersonRequestDto requestDto);
        Task<BaseResponse<bool>> RemovePerson(int personId);
    }
}
 