using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class LessonDefinitionController : BaseApiController
    {
        [HttpGet("GetByLevelId")]
        public async Task<IActionResult> GetByLevelId(int subLevelId)
        {
            return Ok(await Mediator.Send(new GetLessonDefinitionByLevelIdQuery { SubLevelId = subLevelId }));
        }
    }
}
