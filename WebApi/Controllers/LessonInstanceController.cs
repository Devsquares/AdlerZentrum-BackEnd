using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.GroupInstance.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class LessonInstanceController : BaseApiController
    {
        [HttpGet("GetByGroupInstance")]
        public async Task<IActionResult> GetByGroupInstance([FromQuery] LessonsByGroupInstanceIdRequestParameter filter)
        {
            return Ok(await Mediator.Send(new GetLessonInstanceByGroupInstanceIdQuery()
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
                TeacherId = filter.GroupInstanceId
            }));
        }

        [HttpPost("SubmitLessonInstanceStudent")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SubmitLessonInstanceStudent(CreateLessonInstanceStudentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("UpdateLessonInstanceStudent")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateLessonInstanceStudent(int id, UpdateLessonInstanceStudentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        //[HttpPost("CreateHomeWork")]
        ////[Authorize(Roles = "SuperAdmin")]
        //public async Task<IActionResult> CreateHomeWork(int id, CreateLevelCommand command)
        //{
        //    if (id != command.Id)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(await Mediator.Send(command));
        //}
    }
}