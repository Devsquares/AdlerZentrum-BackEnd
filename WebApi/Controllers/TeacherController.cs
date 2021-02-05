using Application.DTOs;
using Application.Features.teacherActions.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class TeacherController : BaseApiController
    {
        [HttpPost("AssignTeacherToGroupInstance")]
        public async Task<IActionResult> AssignTeacherToGroupInstance(string teacherId, int groupInstanceId)
        {

            return Ok(await Mediator.Send(new AssignTeacherToGroupInstanceCommand()
            {
                TeacherId = teacherId,
                GroupInstanceId = groupInstanceId
            })); ;
        }

        [HttpPut("EditTeacherToGroupInstance")]
        public async Task<IActionResult> EditTeacherToGroupInstance(string teacherId, int newGroupInstanceId, int oldGroupInstanceId)
        {

            return Ok(await Mediator.Send(new EditTeacherToGroupInstanceCommand()
            {
                TeacherId = teacherId,
                NewGroupInstanceId= newGroupInstanceId,
                OldGroupInstanceId = oldGroupInstanceId
            }));
        }

        [HttpDelete("RemoveTeacherFromGroupInstance")]
        public async Task<IActionResult> RemoveTeacherFromGroupInstance(string teacherId, int groupInstanceId)
        {

            return Ok(await Mediator.Send(new RemoveTeacherFromGroupInstanceCommand()
            {
                TeacherId = teacherId,
                GroupInstanceId = groupInstanceId
            }));
        }

    }
}
