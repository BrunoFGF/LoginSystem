using AutoMapper;
using LG.Application.Commons.Bases;
using LG.Application.Dtos.Role.Request;
using LG.Application.Dtos.Role.Response;
using LG.Application.Interfaces;
using LG.Domain.Repositories;
using LG.Utilities.Static;

namespace LG.Application.Services
{
    public class RoleApplication : IRoleApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> AssignRolesToUser(AssignRolesRequestDto request)
        {
            var response = new BaseResponse<bool>();

            var userExists = await _unitOfWork.User.GetByIdAsync(request.UserId);
            if (userExists is null)
            {
                response.IsSuccess = false;
                response.Message = $"Usuario con ID {request.UserId} no encontrado.";
                return response;
            }

            foreach (var roleId in request.RoleIds)
            {
                var roleExists = await _unitOfWork.UserRol.GetByIdAsync(roleId);
                if (roleExists is null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Rol con ID {roleId} no encontrado.";
                    return response;
                }
            }

            await _unitOfWork.UserRol.AssignRolesToUser(request.UserId, request.RoleIds, request.AuditUserId);

            response.IsSuccess = true;
            response.Data = true;
            response.Message = "Roles asignados correctamente.";

            return response;
        }

        public async Task<BaseResponse<IEnumerable<RoleResponseDto>>> GetUserRoles(int userId)
        {
            var response = new BaseResponse<IEnumerable<RoleResponseDto>>();

            var userExists = await _unitOfWork.User.GetByIdAsync(userId);
            if (userExists is null)
            {
                response.IsSuccess = false;
                response.Message = $"Usuario con ID {userId} no encontrado.";
                return response;
            }

            var roles = await _unitOfWork.UserRol.GetRolesByUserId(userId);

            if (roles is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
                response.Message = RepplyMessage.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_QUERY_EMPTY;
            }

            return response;
        }
    }
}
