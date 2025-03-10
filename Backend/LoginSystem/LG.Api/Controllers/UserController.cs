using LG.Application.Dtos.User.Request;
using LG.Application.Interfaces;
using LG.Domain.Commons.Bases.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserApplication _userApplication;

        public UserController(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ListUsers([FromBody] BaseFiltersRequest filters)
        {
            var response = await _userApplication.ListUsers(filters);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Select")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ListSelectUsers()
        {
            var response = await _userApplication.ListSelectUsers();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{userId:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UserById(int userId)
        {
            var response = await _userApplication.UserById(userId);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpPost("Register")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserRequestDto request)
        {
            var response = await _userApplication.RegisterUser(request);
            return response.IsSuccess ? Created("api/User", response) : BadRequest(response);
        }

        [HttpPut("{userId:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> EditUser(int userId, [FromBody] UpdateUserRequestDto request)
        {
            var response = await _userApplication.EditUser(userId, request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{userId:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            var response = await _userApplication.RemoveUser(userId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPost("Generate/Token")]
        public async Task<IActionResult> GenerateToken([FromBody] TokenRequestDto requestDto)
        {
            var response = await _userApplication.GenerateToken(requestDto);
            return Ok(response);
        }
    }
}
