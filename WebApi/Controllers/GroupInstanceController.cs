﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
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
            return Ok(await Mediator.Send(new GetAllGroupInstancesQuery()
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

        [HttpGet("GetByTeacher")]
        public async Task<IActionResult> GetByTeacher([FromQuery] GroupInstanceByTeacherRequestParameter filter)
        {
            return Ok(await Mediator.Send(new GetGroupInstanceByIdTeacherQuery()
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
                TeacherId = filter.teacherId
            }));
        }

        [HttpPost("ActiveGroupInstance")]
        public async Task<IActionResult> ActiveGroupInstanceCommand([FromQuery] ActiveGroupInstanceCommand filter)
        {
            return Ok(await Mediator.Send(new ActiveGroupInstanceCommand()
            {
                GroupInstanceId = filter.GroupInstanceId
            }));
        }
    }
}