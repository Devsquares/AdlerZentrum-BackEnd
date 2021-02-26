using Application.DTOs;
using Application.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class QuestionController : BaseApiController
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] RequestParameter filter, int? QuestionTypeId)
        {
            if (filter.PageSize == 0)
            {
                filter.PageSize = 10;
            }
            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            return Ok(await Mediator.Send(new GetAllQuestionsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber, QuestionTypeId = QuestionTypeId }));
        }

        [HttpGet("GetByType")]
        public async Task<IActionResult> GetByType([FromQuery] GetQuestionsByTypeQuery filter)
        {
            return Ok(await Mediator.Send(new GetQuestionsByTypeQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber, QuestionType = filter.QuestionType, NotUsed = false }));
        }

        [HttpGet("GetByTypeNotUsed")]
        public async Task<IActionResult> GetByTypeNotUsed([FromQuery] GetQuestionsByTypeQuery filter)
        {
            return Ok(await Mediator.Send(new GetQuestionsByTypeQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber, QuestionType = filter.QuestionType, NotUsed = true }));
        }


        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetQuestionByIdQuery { Id = id }));
        }

        [HttpPost("Create")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post(CreateQuestionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        [HttpPut("update")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Put(int id, UpdateQuestionCommand command)
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
            return Ok(await Mediator.Send(new DeleteQuestionByIdCommand { Id = id }));
        }
    }
}
