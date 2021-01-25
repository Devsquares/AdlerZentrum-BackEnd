using Application.DTOs;
using Application.DTOs.Level.Commands;
using Application.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class SublevelController : BaseApiController
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] RequestParameter filter)
        {
           return Ok(await Mediator.Send(new GetAllSublevelsQuery() ));
        } 

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
           return Ok(await Mediator.Send(new GetSublevelByIdQuery { Id = id }));
        }

        [HttpPost("Create")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post(int id, CreateSublevelCommand command)
        {
           if (id != command.Id)
           {
               return BadRequest();
           }
           return Ok(await Mediator.Send(command));
        }

        [HttpPut("update")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Put(int id, UpdateSublevelCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteSublevelByIdCommand { Id = id }));
        }
    }
}
