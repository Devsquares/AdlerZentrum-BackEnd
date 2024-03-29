using Application.DTOs;
using Application.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class SingleQuestionController : BaseApiController
    {
        // Gat All avilable qutions to create quiz
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] RequestParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllSingleQuestionsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        [HttpGet("GetAllByType")]
        public async Task<IActionResult> GetAllByType([FromQuery] GetAllSingleQuestionsByTypeQuery filter)
        {
            return Ok(await Mediator.Send(new GetAllSingleQuestionsByTypeQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber, TypeId = filter.TypeId }));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetSingleQuestionByIdQuery { Id = id }));
        }

        [HttpPost("Create")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post(int id, CreateSingleQuestionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }


        [HttpPut("update")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Put(int id, UpdateSingleQuestionCommand command)
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
            return Ok(await Mediator.Send(new DeleteSingleQuestionByIdCommand { Id = id }));
        }
    }
}
