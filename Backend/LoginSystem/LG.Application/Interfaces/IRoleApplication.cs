using LG.Application.Commons.Bases;
using LG.Application.Dtos.Role.Request;
using LG.Application.Dtos.Role.Response;

namespace LG.Application.Interfaces
{
    public interface IRoleApplication
    {
        Task<BaseResponse<bool>> AssignRolesToUser(AssignRolesRequestDto request);
        Task<BaseResponse<IEnumerable<RoleResponseDto>>> GetUserRoles(int userId);
    }
}
