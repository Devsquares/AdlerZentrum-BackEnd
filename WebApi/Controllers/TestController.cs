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

        [HttpGet("GetAllQuizzes")]
        //[Authorize(Roles = "SuperAdmin,Supervisor")]
        public async Task<IActionResult> GetAllQuizzes([FromQuery] GetAllTestsQuery filter)
        {
            return Ok(await Mediator.Send(new GetAllTestsQuery()
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TestType = TestTypeEnum.quizz
            }));
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

    }
}
