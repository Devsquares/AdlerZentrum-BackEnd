using Application.DTOs;
using Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class TestController : BaseApiController
    {

        [HttpPost("CreateQuiz")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post(CreateQuizCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("CreateTestCommand")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post(CreateTestCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("UpdateTestCommand")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Put(UpdateTestCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetAllTests")]
        //[Authorize(Roles = "SuperAdmin,Supervisor")]
        public async Task<IActionResult> GetAllTests([FromQuery] GetAllTestsQuery filter)
        {
            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            if (filter.PageSize == 0)
            {
                filter.PageSize = 10;
            }
            return Ok(await Mediator.Send(filter));
        }

        [HttpGet("GetTestById")]
        //[Authorize(Roles = "SuperAdmin,Supervisor")]
        public async Task<IActionResult> GetTestById([FromQuery] GetTestByIdQuery filter)
        {
            return Ok(await Mediator.Send(new GetTestByIdQuery()
            {
                Id = filter.Id
            }));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteTestByIdCommand { Id = id }));
        }

        [HttpPost("PlacementRelease")]
        public async Task<IActionResult> PlacementRelease([FromBody] PlacementReleaseCommand command)
        {
            return Ok(await Mediator.Send(new PlacementReleaseCommand { PlacementTestId = command.PlacementTestId, StartDate = command.StartDate }));
        }

        [HttpPut("PlacementUnrelease")]
        public async Task<IActionResult> PlacementUnrelease(int PlacementTestId)
        {
            return Ok(await Mediator.Send(new PlacementReleaseCommand { PlacementTestId = PlacementTestId, StartDate = null }));
        }

        [HttpGet("GetAllPlacementTests")]
        public async Task<IActionResult> GetAllPlacementTests([FromQuery] GetAllPlacementTestsQuery filter)
        {
            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            if (filter.PageSize == 0)
            {
                filter.PageSize = 10;
            }
            return Ok(await Mediator.Send(filter));
        }

    }
}
