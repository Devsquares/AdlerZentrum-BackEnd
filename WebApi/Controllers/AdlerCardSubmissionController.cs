using Application.Features;
using Application.Features.AdlerCardSubmission.Commands;
using Application.Features.AdlerCardSubmission.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AdlerCardSubmissionController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet("GetAllAdlerCardSubmission")]
        public async Task<IActionResult> Get([FromQuery] GetAllAdlerCardSubmissionsParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllAdlerCardSubmissionsQuery()
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
            return Ok(await Mediator.Send(new GetAdlerCardSubmissionByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost("CreateAdlerCardSubmission")]
        //[Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Post(CreateAdlerCardSubmissionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Put(int id, UpdateAdlerCardSubmissionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteAdlerCardSubmissionByIdCommand { Id = id }));
        }

        [HttpGet("GetAdlerCardsSubmissionsForStaff")]
        public async Task<IActionResult> GetAdlerCardsSubmissionsForStaff([FromQuery] GetAdlerCardsSubmissionsForStaffQuery request)
        {
            if (request.PageNumber == 0)
            {
                request.PageNumber = 1;
            }
            if (request.PageSize == 0)
            {
                request.PageSize = 10;
            }
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GetAdlerCardsSubmissionsForTeacher")]
        public async Task<IActionResult> GetAdlerCardsSubmissionsForTeacher([FromQuery] GetAdlerCardsSubmissionsForStaffQuery request)
        {
            if (request.PageNumber == 0)
            {
                request.PageNumber = 1;
            }
            if (request.PageSize == 0)
            {
                request.PageSize = 10;
            }
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("AssignTeacherToSubmission")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AssignTeacherToSubmission(AssignTeacherToSubmissionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
