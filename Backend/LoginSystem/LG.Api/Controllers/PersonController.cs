using LG.Application.Dtos.Person.Request;
using LG.Application.Interfaces;
using LG.Infrastructure.Commons.Bases.Request;
using Microsoft.AspNetCore.Mvc;

namespace LG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonApplication _personApplication;

        public PersonController(IPersonApplication personApplication)
        {
            _personApplication = personApplication;
        }

        [HttpPost]
        public async Task<IActionResult> ListPersons([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _personApplication.ListPersons(filters);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectPersons()
        {
            var response = await _personApplication.ListSelectPersons();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{personId:int}")]
        public async Task<IActionResult> PersonById(int personId)
        {
            var response = await _personApplication.PersonById(personId);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterPerson([FromBody] PersonRequestDto request)
        {
            var response = await _personApplication.RegisterPerson(request);
            return response.IsSuccess ? Created("api/Person", response) : BadRequest(response);
        }

        [HttpPut("{personId:int}")]
        public async Task<IActionResult> EditPerson(int personId, [FromBody] PersonRequestDto request)
        {
            var response = await _personApplication.EditPerson(personId, request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{personId:int}")]
        public async Task<IActionResult> RemovePerson(int personId)
        {
            var response = await _personApplication.RemovePerson(personId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
