using Application.Features;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class SingleQuestionSubmissionController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllSingleQuestionSubmissionsParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllSingleQuestionSubmissionsQuery()
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
                FilterArray = filter.FilterArray,
                FilterRange = filter.FilterRange,
                FilterSearch = filter.FilterSearch,
                FilterValue = filter.FilterValue,
                SortBy = filter.SortBy,
                SortType = filter.SortType,
                NoPaging = filter.NoPaging
            }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetSingleQuestionSubmissionByIdQuery { Id = id }));
        }

         [HttpGet("GetByTestInstanceId/{id}")]
        public async Task<IActionResult> GetByTestInstanceId(int id)
        {
            return Ok(await Mediator.Send(new GetSingleQuestionSubmissionByTestInstanceIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Post(CreateSingleQuestionSubmissionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Put(int id, UpdateSingleQuestionSubmissionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("TestCorrection")]
        public async Task<IActionResult> TestCorrection(int id, UpdateGroupOfSingleQuestionSubmission command)
        {
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteSingleQuestionSubmissionByIdCommand { Id = id }));
        }

    }
}
