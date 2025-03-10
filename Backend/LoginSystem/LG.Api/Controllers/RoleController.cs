using LG.Application.Dtos.Role.Request;
using LG.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleApplication _roleApplication;

        public RoleController(IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var response = await _roleApplication.GetUserRoles(userId);
            return Ok(response);
        }

        [HttpPost("assign")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AssignRoles([FromBody] AssignRolesRequestDto request)
        {
            var response = await _roleApplication.AssignRolesToUser(request);
            return Ok(response);
        }

    }
}
