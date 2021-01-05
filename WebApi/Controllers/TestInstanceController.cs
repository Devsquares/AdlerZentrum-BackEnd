using Application.Features;
using Application.Features.TestInstance.Commands.CreateTestInstance;
using Application.Features.TestInstance.Commands.DeleteTestInstanceById;
using Application.Features.TestInstance.Commands.UpdateTestInstance;
using Application.Features.TestInstance.Queries.GetAllTestInstances;
using Application.Features.TestInstance.Queries.GetTestInstanceById;
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
                GroupInstanceId = AuthenticatedUserService.GroupInstanceId.Value
            }));
        }
         
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Post(CreateTestInstanceCommand command)
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

        [HttpGet("GetTestInstanceToAssginQuery")]
        //[Authorize(Roles = "SuperAdmin")]        
        public async Task<IActionResult> GetTestInstanceToAssginQuery(GetTestInstanceToAssginQuery command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]        
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteTestInstanceByIdCommand { Id = id }));
        }

    }
}
