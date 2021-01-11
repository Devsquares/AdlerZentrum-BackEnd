using Application.Features;
using Application.Features.TestInstance.Commands.DeleteTestInstanceById;
using Application.Features.TestInstance.Commands.UpdateTestInstance;
using Application.Features.TestInstance.Queries.GetAllTestInstances;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Controllers;

namespace WebApi.Controller
{
    public class TestInstanceController : BaseApiController
    {
        [HttpGet]
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
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetTestInstanceByIdQuery { Id = id }));
        }

        [HttpGet("GetQuizzesForStudent")]
        public async Task<IActionResult> GetQuizzesForStudent()
        {
            return Ok(await Mediator.Send(new GetAllTestInstancesByStudentQuery
            {
                StudentId = AuthenticatedUserService.UserId,
                GroupInstanceId = AuthenticatedUserService.GroupInstanceId.Value,
                TestType = Application.Enums.TestTypeEnum.quizz
            }));
        }

        [HttpGet("GetFinalLevelTestsForStudent")]
        public async Task<IActionResult> GetFinalLevelTestsForStudent()
        {
            return Ok(await Mediator.Send(new GetAllTestInstancesByStudentQuery
            {
                StudentId = AuthenticatedUserService.UserId,
                GroupInstanceId = AuthenticatedUserService.GroupInstanceId.Value,
                TestType = Application.Enums.TestTypeEnum.final
            }));
        }

        [HttpGet("GetSubLevelTestsForStudent")]
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
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post(TestInstanceSolutionCommand command)
        {
            command.StudentId = AuthenticatedUserService.UserId;
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]        
        public async Task<IActionResult> Put(int id, UpdateTestInstanceCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("AssginTeacherToTest")]
        //[Authorize(Roles = "SuperAdmin")]        
        public async Task<IActionResult> AssginTeacherToTest(AssginTeacherTestInstanceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetTestInstanceToAssgin")]
        //[Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetTestInstanceToAssgin()
        {
            return Ok(await Mediator.Send(new GetTestInstanceToAssginQuery()));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]        
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteTestInstanceByIdCommand { Id = id }));
        }

    }
}
