using LG.Application.Dtos.User.Request;
using LG.Application.Interfaces;
using LG.Infrastructure.Commons.Bases.Request;
using Microsoft.AspNetCore.Mvc;

namespace LG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApplication _userApplication;

        public UserController(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        [HttpPost]
        public async Task<IActionResult> ListUsers([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _userApplication.ListUsers(filters);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectUsers()
        {
            var response = await _userApplication.ListSelectUsers();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> UserById(int userId)
        {
            var response = await _userApplication.UserById(userId);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRequestDto request)
        {
            var response = await _userApplication.RegisterUser(request);
            return response.IsSuccess ? Created("api/User", response) : BadRequest(response);
        }

        [HttpPut("{userId:int}")]
        public async Task<IActionResult> EditUser(int userId, [FromBody] UserRequestDto request)
        {
            var response = await _userApplication.EditUser(userId, request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            var response = await _userApplication.RemoveUser(userId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
