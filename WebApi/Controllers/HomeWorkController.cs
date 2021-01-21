using Application.DTOs;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class HomeWorkController : BaseApiController
    {
        [HttpPost("SubmitHomeWorkForStudent")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SubmitHomeWorkForStudent(int id, SubmitHomeWorkCommand command)
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
            command.CorrectionTeacherId = AuthenticatedUserService.UserId;
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("CreateAdditionalHomework")]
        public async Task<IActionResult> CreateAdditionalHomework(CreateHomeWorkCommand command)
        {
            command.TeacherId = AuthenticatedUserService.UserId;
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetHomeworkForStudent")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> GetHomeworkForStudent()
        {
            if (AuthenticatedUserService.GroupInstanceId == null)
            {
                return Ok(new Response<object>("Not registerd in any group."));
            }

            return Ok(await Mediator.Send(new GetAllHomeWorkSubmitionsForStudentQuery()
            {
                StudentId = AuthenticatedUserService.UserId,
                GroupInstanceId = AuthenticatedUserService.GroupInstanceId.Value
            }));
        }

        [HttpGet("GetBounsRequests")]
        // [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetBounsRequests()
        {
            return Ok(await Mediator.Send(new GetHomeworkBounsRequestsQuery()));
        }


        [HttpPut("update")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Put(UpdateHomeworkBounsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        [HttpGet("GetHomeworkSubmitionByGroupInstance")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetHomeworkSubmitionByGroupInstance([FromQuery] GetAllHomeWorkSubmitionsQuery filter)
        {
            return Ok(await Mediator.Send(new GetAllHomeWorkSubmitionsQuery()
            {
                GroupInstanceId = filter.GroupInstanceId
            }));
        }

        [HttpGet("GetHomeworkSubmitions")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetAllHomeworkSubmitions()
        {
            return Ok(await Mediator.Send(new GetAllHomeWorkSubmitionsQuery()
            {
                GroupInstanceId = null
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