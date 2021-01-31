using Application.Features;
using Application.Features.TestInstance.Commands.DeleteTestInstanceById;
using Application.Features.TestInstance.Commands.UpdateTestInstance;
using Application.Features.TestInstance.Queries.GetAllTestInstances;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Controllers;

namespace WebApi.Controller
{
    public class TestInstanceController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Supervisor")]
        public async Task<IActionResult> Get([FromQuery] GetAllTestInstancesParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllTestInstancesQuery()
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

        [HttpGet("{id}")]
        //[Authorize(Roles = "SuperAdmin,Supervisor,Secretary,Student")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetTestInstanceByIdQuery { Id = id }));
        }

        [HttpGet("GetQuizzesForStudent")]
        [Authorize(Roles = "SuperAdmin,Student")]
        public async Task<IActionResult> GetQuizzesForStudent()
        {
            if (AuthenticatedUserService.GroupInstanceId == null)
            {
                return Ok(new Response<object>("Not registerd in any group."));
            }

            return Ok(await Mediator.Send(new GetAllTestInstancesByStudentQuery
            {
                StudentId = AuthenticatedUserService.UserId,
                GroupInstanceId = AuthenticatedUserService.GroupInstanceId.Value,
                TestType = Application.Enums.TestTypeEnum.quizz
            }));
        }


        [HttpGet("GetFinalLevelTestsForStudent")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetFinalLevelTestsForStudent()
        {
            if (AuthenticatedUserService.GroupInstanceId == null)
            {
                return Ok(new Response<object>("Not registerd in any group."));
            }
            return Ok(await Mediator.Send(new GetAllTestInstancesByStudentQuery
            {
                StudentId = AuthenticatedUserService.UserId,
                GroupInstanceId = AuthenticatedUserService.GroupInstanceId.Value,
                TestType = Application.Enums.TestTypeEnum.final
            }));
        }

        [HttpGet("GetSubLevelTestsForStudent")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetSubLevelTestsForStudent()
        {
            return Ok(await Mediator.Send(new GetAllTestInstancesByStudentQuery
            {
                StudentId = AuthenticatedUserService.UserId,
                GroupInstanceId = AuthenticatedUserService.GroupInstanceId.Value,
                TestType = Application.Enums.TestTypeEnum.subLevel
            }));
        }

        // TODO need to be reviewd
        [HttpPut("TestInstanceSolution")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Post(TestInstanceSolutionCommand command)
        {
            command.StudentId = AuthenticatedUserService.UserId;
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetTestResults")]
        public async Task<IActionResult> GetTestResults([FromQuery] GetAllTestInstancesResultsQuery query)
        {
            return Ok(await Mediator.Send(new GetAllTestInstancesResultsQuery
            {
                GroupInstanceId = query.GroupInstanceId,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            }));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Put(int id, UpdateTestInstanceCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("AssginTeacherToTest")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AssginTeacherToTest(AssginTeacherTestInstanceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetTestInstanceToAssgin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetTestInstanceToAssgin()
        {
            return Ok(await Mediator.Send(new GetTestInstanceToAssginQuery()));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteTestInstanceByIdCommand { Id = id }));
        }

        [HttpGet("GetTestInstanceToActive")]
        [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> GetTestInstanceToActive()
        {
            return Ok(await Mediator.Send(new GetTestInstanceToActiveQuery()));
        }

        [HttpPut("TestToActive")]
        [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> TestToActive(ActiveTestInstanceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("TestToClose")]
        [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> TestToClose(CloseTestInstanceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}