using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.GroupInstance.Commands;
using Application.DTOs.GroupInstance.Queries;
using Application.DTOs.GroupInstance.Queries.GetAll;
using Application.DTOs.GroupInstance.Queries.GetById;
using Application.Features.GroupInstancesStudents.Commands;
using Application.Filters;
using Domain.Models;
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


        [HttpPost("AssignTeacherToGroupInstance")]
        public async Task<IActionResult> AssignTeacherToGroupInstance([FromQuery] AssignTeacherToGroupInstanceCommand filter)
        {
            return Ok(await Mediator.Send(new AssignTeacherToGroupInstanceCommand()
            {
                GroupInstanceId = filter.GroupInstanceId
            }));
        }



        [HttpGet("GetListByGroupDefinitionId")]
        public async Task<IActionResult> GetListByGroupDefinitionId(int groupDefinitionId)
        {
            return Ok(await Mediator.Send(new GetGroupInstanceByGroupDefinitionIdQuery() { GroupDefinitionId = groupDefinitionId }));
        }


        [HttpGet("GetLastByStudent")]
        public async Task<IActionResult> GetLastByStudent( string studentId )
        {
            return Ok(await Mediator.Send(new GetGroupInstanceByIdStudentQuery()
            {
                StudentId = studentId
            }));
        }

        [HttpGet("GetAllLastByStudent")]
        public async Task<IActionResult> GetByStudent(string studentId)
        {
            return Ok(await Mediator.Send(new GetAllLastGroupInstanceByIdStudentQuery()
            {
                StudentId = studentId
            }));
        }


        [HttpPost("CreateGroupInstanceAutomatic")]
        public async Task<IActionResult> CreateGroupInstanceAutomatic(int groupDefinitionId)
        {
            return Ok(await Mediator.Send(new CreateGroupInstanceWithInterestedOverPaymentStudentCommand()
            {
                GroupDefinitionId = groupDefinitionId
            }));
        }

        [HttpPost("EditGroupInstanceByStudent")]
        public async Task<IActionResult> EditGroupInstanceByStudent(int groupDefinitionId, int srcGroupInstanceId, int desGroupInstanceId, string studentId, int? promoCodeId)
        {
            return Ok(await Mediator.Send(new EditGroupInstanceByAddingStudentFromAnotherCommand()
            {
                GroupDefinitionId = groupDefinitionId,
                srcGroupInstanceId = srcGroupInstanceId,
                desGroupInstanceId = desGroupInstanceId,
                promoCodeId = promoCodeId,
                studentId = studentId
            }));
        }

        [HttpPost("SwapStudentsBetweenGroupInsatnces")]
        public async Task<IActionResult> SwapStudentsBetweenGroupInsatnces(int groupDefinitionId, int srcGroupInstanceId, string srcstudentId, int desGroupInstanceId, string desstudentId)
        {
            return Ok(await Mediator.Send(new SwapTwoStudentsCommand()
            {
                GroupDefinitionId = groupDefinitionId,
                DesGroupInstanceId = desGroupInstanceId,
                DesStudentId = desstudentId,
                SrcGroupInstanceId = srcGroupInstanceId,
                SrcStudentId = srcstudentId
            }));
        }

        [HttpPut("SaveAllGroupInstanceStudents")]
        public async Task<IActionResult> SaveAllGroupInstanceStudents(int groupDefinitionId, List<GroupInstanceStudentsViewModel> groupInstanceStudents)
        {
            return Ok(await Mediator.Send(new SaveAllGroupInstanceStudentsCommand()
            {
                GroupDefinitionId = groupDefinitionId,
                GroupInstancesStudentList = groupInstanceStudents
            }));
        }

    }
}