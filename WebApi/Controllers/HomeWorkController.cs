using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class HomeWorkController : BaseApiController
    {
        [HttpPost("SubmitHomeWorkForStudent")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SubmitHomeWorkForStudent(int id, CreateHomeWorkSubmitionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("SubmitHomeWorkCorrection")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SubmitHomeWorkCorrection(HomeworkCorrectionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("CreateAdditionalHomework")]
        public async Task<IActionResult> CreateAdditionalHomework(int id, CreateHomeWorkCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetHomeworkForStudent")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> GetByGroupInstance()
        {
            return Ok(await Mediator.Send(new GetAllHomeWorkSubmitionsForStudentQuery()
            {
                StudentId = AuthenticatedUserService.UserId,
                GroupInstanceId = AuthenticatedUserService.GroupInstanceId.Value
            }));
        }

        [HttpGet("GetHomeworkSubmition")]
        public async Task<IActionResult> GetHomeworkSubmition([FromQuery] GetAllHomeWorkSubmitionsQuery filter)
        {
            return Ok(await Mediator.Send(new GetAllHomeWorkSubmitionsQuery()
            {
                GroupInstanceId = filter.GroupInstanceId
            }));
        }

        [HttpGet("GetHomeworkSubmitionById")]
        public async Task<IActionResult> GetHomeworkSubmitionById([FromQuery] GetHomeWorkSubmitionByIdQuery filter)
        {
            return Ok(await Mediator.Send(new GetHomeWorkSubmitionByIdQuery()
            {
                HomeWorkSubmitionId = filter.HomeWorkSubmitionId
            }));
        }
    }
}