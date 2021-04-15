using Application.DTOs;
using Application.DTOs.Level.Queries;
using Application.Exceptions;
using Application.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class FeedbackController : BaseApiController
    {
        [HttpGet("CheckFeedbackSheetCreation")]
        public async Task<IActionResult> CheckFeedbackSheetCreation()
        {
            return Ok(await Mediator.Send(new CheckFeedbackSheetCreationQuery()));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetFeedbackSheetQuery()));
        }
        [HttpPut("Archive")]
        public async Task<IActionResult> Archive([FromQuery] int id)
        {
            return Ok(await Mediator.Send(new UpdateArchiveFeedbackSheetCommand() { Id = id }));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateFeedbackSheetCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetFeedbackSheetInstancesForStudentByGroupInstanceId")]
        public async Task<IActionResult> GetFeedbackSheetInstancesForStudentByGroupInstanceId([FromQuery] string studentId, [FromQuery] int groupInstanceId)
        {
            return Ok(await Mediator.Send(new GetFeedbackSheetInstancesForStudentByGroupInstanceIdQuery()
            {
                StudentId = studentId,
                GroupInstanceId = groupInstanceId
            }
            ));
        }


        [HttpGet("GetFeedbackSheetInstancesForStudent")]
        public async Task<IActionResult> GetFeedbackSheetInstancesForStudent([FromQuery] string studentId)
        {
            return Ok(await Mediator.Send(new GetFeedbackSheetInstancesForStudentByGroupInstanceIdQuery()
            {
                StudentId = studentId,
                GroupInstanceId = null
            }
            ));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllFeedbackSheetsParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllFeedbackSheetsQuery()
            {
                GroupInstanceId = filter.GroupInstanceId,
                LessonInstanceId = filter.LessonInstanceId,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Status = filter.Status,
                StudentName = filter.StudentName
            }));
        }

    }
}
