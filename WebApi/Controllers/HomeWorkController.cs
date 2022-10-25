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
        public async Task<IActionResult> GetHomeworkForStudent([FromQuery] GetAllHomeWorkSubmitionsForStudentQuery query)
        {
            if (AuthenticatedUserService.Role == "Student" && AuthenticatedUserService.GroupInstanceId == null)
            {
                return Ok(new Response<object>("Not registerd in any group."));
            }

            return Ok(await Mediator.Send(new GetAllHomeWorkSubmitionsForStudentQuery()
            {
                StudentId = query.StudentId,
                GroupInstanceId = query.GroupInstanceId
            }));
        }

        [HttpGet("GetHomeworkForStudentByGroupInstanceId")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> GetHomeworkForStudentByGroupInstanceId([FromQuery] GetAllHomeWorkSubmitionsForStudentQuery query)
        {
            return Ok(await Mediator.Send(new GetAllHomeWorkSubmitionsForStudentQuery()
            {
                StudentId = query.StudentId,
                GroupInstanceId = query.GroupInstanceId
            }));
        }

        [HttpGet("GetBounsRequests")]
        // [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetBounsRequests([FromQuery]GetHomeworkBounsRequestsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }


        [HttpPut("update")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Put(UpdateHomeworkBounsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        [HttpGet("GetHomeworkSubmitionByGroupInstance")]
        // [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetHomeworkSubmitionByGroupInstance([FromQuery] GetAllHomeWorkSubmitionsQuery filter)
        {
            return Ok(await Mediator.Send(new GetAllHomeWorkSubmitionsQuery()
            {
                GroupInstanceId = filter.GroupInstanceId,
                Status = filter.Status,
                TeacherId = filter.TeacherId
            }));
        }

        [HttpGet("GetHomeworkSubmitions")]
        // [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetAllHomeworkSubmitions([FromQuery] string TeacherId)
        {
            return Ok(await Mediator.Send(new GetAllHomeWorkSubmitionsQuery()
            {
                GroupInstanceId = null,
                TeacherId = TeacherId
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