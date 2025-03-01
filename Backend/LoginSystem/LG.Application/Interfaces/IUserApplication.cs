using LG.Application.Commons.Bases;
using LG.Application.Dtos.User.Request;
using LG.Application.Dtos.User.Response;
using LG.Infrastructure.Commons.Bases.Request;
using LG.Infrastructure.Commons.Bases.Response;

namespace LG.Application.Interfaces
{
    public interface IUserApplication
    {
        Task<BaseResponse<BaseEntityResponse<UserResponseDto>>> ListUsers(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<UserSelectResponseDto>>> ListSelectUsers();
        Task<BaseResponse<UserResponseDto>> UserById(int userId);
        Task<BaseResponse<bool>> RegisterUser(UserRequestDto requestDto);
        Task<BaseResponse<bool>> EditUser(int userId, UserRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveUser(int userId);
    }
}
