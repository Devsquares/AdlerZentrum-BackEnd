using Application.DTOs;
using Application.DTOs.HomeWork.Commands;
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

        [HttpPost("SubmitHomeWorkForStudent")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SubmitHomeWorkForStudent(HomeworkCorrectionCommand command)
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
        public async Task<IActionResult> GetByGroupInstance([FromQuery] GetAllHomeWorkSubmitionsForStudentQuery filter)
        {
            return Ok(await Mediator.Send(new GetAllHomeWorkSubmitionsForStudentQuery()
            {
                StudentId = filter.StudentId,
                GroupInstanceId = filter.GroupInstanceId
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
    }
}