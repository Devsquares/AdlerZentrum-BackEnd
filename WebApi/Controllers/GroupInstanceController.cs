using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.GroupInstance.Queries;
using Application.DTOs.GroupInstance.Queries.GetAll;
using Application.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class GroupInstanceController : BaseApiController
    {

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GroupInstanceRequestParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllGroupInstancesQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber, DateTimeFrom = filter.DateTimeFrom, DateTimeTo = filter.DateTimeTo }));
        }

    }
}