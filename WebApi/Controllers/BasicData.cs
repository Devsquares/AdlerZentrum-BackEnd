using Application.DTOs.BasicData;
using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class BasicData : BaseApiController
    {
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await Mediator.Send(new GetAllRolesQuery()));
        }

        [HttpGet("GetAlHomeWorkSubmitionStatus")]
        public async Task<IActionResult> GetAlHomeWorkSubmitionStatus()
        {
            return Ok(await Mediator.Send(new GetAllHomeworkSubmtionStatusEnumQuery()));
        }

    }
}
