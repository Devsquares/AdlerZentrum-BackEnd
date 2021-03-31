using Application.Enums;
using Application.Features;
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
        //[Authorize(Roles = "SuperAdmin,Supervisor,Secretary,Student")]
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


        [HttpGet("GetPlacementTestForStudent")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetPlacementTestForStudent()
        {
            return Ok(await Mediator.Send(new GetPlacementByStudentQuery
            {
                StudentId = AuthenticatedUserService.UserId,
            }));
        }

        [HttpPut("TestInstanceSolution")]
        // [Authorize(Roles = "Student")]
        public async Task<IActionResult> Post(TestInstanceSolutionCommand command)
        {
            // command.StudentId = AuthenticatedUserService.UserId;
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
        [Authorize(Roles = "SuperAdmin,Supervisor,Secretary,Teacher")]
        public async Task<IActionResult> GetTestInstanceToAssgin()
        {
            return Ok(await Mediator.Send(new GetTestInstanceToAssginQuery()));
        }

        [HttpGet("GetTestInstanceByTeacher")]
        [Authorize(Roles = "SuperAdmin,Supervisor,Secretary,Teacher")]
        public async Task<IActionResult> GetTestInstanceByTeacher([FromQuery] GetTestInstanceByTeacherQuery command)
        {
            return Ok(await Mediator.Send(new GetTestInstanceByTeacherQuery
            {
                Status = command.Status,
                TestType = command.TestType,
                TeacherId = AuthenticatedUserService.UserId,
                GroupInstanceId = command.GroupInstanceId
            }));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
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
        // [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> TestToClose(CloseTestInstanceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetAllTestsToManage")]
        // [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> GetAllTestsToManage([FromQuery] GetAllTestsToManageQuery query)
        {
            return Ok(await Mediator.Send(new GetAllTestsToManageQuery()
            {
                GroupDefinitionId = query.GroupDefinitionId,
                GroupInstanceId = query.GroupInstanceId,
                TestTypeId = query.TestTypeId,
                Status = query.Status,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            }
            ));
        }

        [HttpPut("TestToActiveByGroup")]
        [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> TestToActiveByGroup(UpdateTestInstanceStatusByGroupCommand command)
        {
            command.Status = (int)TestInstanceEnum.Pending;
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("TestToCloseByGroup")]
        [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> TestToCloseByGroup(UpdateTestInstanceStatusByGroupCommand command)
        {
            command.Status = (int)TestInstanceEnum.Missed;
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("TestToActiveAll")]
        [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> TestToActiveAll(UpdateTestInstanceStatusAllCommand command)
        {
            command.Status = (int)TestInstanceEnum.Pending;
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("TestToCloseAll")]
        [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> TestToCloseAll(UpdateTestInstanceStatusAllCommand command)
        {
            command.Status = (int)TestInstanceEnum.Missed;
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("TestToActiveByStudent")]
        // [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> TestToActiveByStudent(int id)
        {
            UpdateTestInstanceStatusByIdCommand command = new UpdateTestInstanceStatusByIdCommand();
            command.Id = id;
            command.Status = (int)TestInstanceEnum.Pending;
            return Ok(await Mediator.Send(command));
        }


        [HttpGet("TestToCloseByStudent")]
        // [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> TestToCloseByStudent(int id)
        {
            UpdateTestInstanceStatusByIdCommand command = new UpdateTestInstanceStatusByIdCommand();
            command.Id = id;
            command.Status = (int)TestInstanceEnum.Missed;
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("RequestRecorrection")]
        public async Task<IActionResult> RequestRecorrection(int TestInstanceId)
        {
            return Ok(await Mediator.Send(new UpdateTestInstanceReCorrectionRequestCommand() {TestInstanceId = TestInstanceId,status = true }));
        }
        [HttpPut("OpenTestToReview")]
        public async Task<IActionResult> OpenTestToReview(int TestInstanceId)
        {
            return Ok(await Mediator.Send(new UpdateTestInstanceReopenedCommand() { TestInstanceId = TestInstanceId, status = true }));
        }
        [HttpPut("CloseTestToReview")]
        public async Task<IActionResult> CloseTestToReview(int TestInstanceId)
        {
            return Ok(await Mediator.Send(new UpdateTestInstanceReopenedCommand() { TestInstanceId = TestInstanceId, status = false }));
        }

        [HttpGet("GetTestsToReview")]
        // [Authorize(Roles = "SuperAdmin,Supervisor,Secretary")]
        public async Task<IActionResult> GetTestsToReview([FromQuery] GetAllTestsToManageQuery query)
        {
            return Ok(await Mediator.Send(new GetAllTestsToManageQuery()
            {
                GroupDefinitionId = query.GroupDefinitionId,
                GroupInstanceId = query.GroupInstanceId,
                TestTypeId = query.TestTypeId,
                Status = query.Status == null?(int)TestInstanceEnum.Corrected: query.Status,
                reCorrectionRequest = query.reCorrectionRequest == null?true: query.reCorrectionRequest,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            }
            ));
        }

    }
}