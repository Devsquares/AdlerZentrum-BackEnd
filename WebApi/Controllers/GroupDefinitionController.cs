﻿using System.Threading.Tasks;
using Application.DTOs;
using Application.Features.InterestedStudent.Queries.GetInterestedStudentByGroupDefinitionId;
using Application.Features.OverPaymentStudent.Queries.GetOverPaymentStudentByGroupDefinitionId;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class GroupDefinitionController : BaseApiController
    {

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GroupDefinitionRequestParameter filter,string sublevelName)
        {            
            return Ok(await Mediator.Send(new GetAllGroupDefinitionsQuery()
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
                SubLevel = sublevelName
            }));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetGroupDefinitionByIdQuery { Id = id }));
        }

        [HttpPost("Create")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post(int id, CreateGroupDefinitionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }



        [HttpPut("update")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Put(int id, UpdateGroupDefinitionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }


        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteGroupDefinitionByIdCommand { Id = id }));
        }

        [HttpPut("Cancel")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Cancel(int id)
        {
            return Ok(await Mediator.Send(new CancelGroupDefinitionByIdCommand { Id = id, UserId = AuthenticatedUserService.UserId }));
        }

        [HttpPut("StudentRegister")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> StudentRegister(int groupDefinitionId, int? promoCodeId,int? placmentTest)
        {
            return Ok(await Mediator.Send(new RegisterStudentGroupDefinitionCommand {groupDefinitionId = groupDefinitionId, StudentId = AuthenticatedUserService.UserId, PromoCodeId = promoCodeId,PlacmentTestId = placmentTest}));
        }

        [HttpGet("GetInterestedStudent")]
        public async Task<IActionResult> GetInterestedStudent(int groupDefinitionId)
        {
            return Ok(await Mediator.Send(new GetInterestedStudentByGroupDefinitionIdQuery { GroupDefinitionId = groupDefinitionId }));
        }

        [HttpGet("GetOverPaymentStudent")]
        public async Task<IActionResult> GetOverPaymentStudent(int groupDefinitionId)
        {
            return Ok(await Mediator.Send(new GetOverPaymentStudentByGroupDefinitionIdQuery { GroupDefinitionId = groupDefinitionId }));
        }

        [HttpGet("GetGroupDefinitionDependcies")]
        public async Task<IActionResult> GetGroupDefinitionDependcies(int groupDefinitionId)
        {
            return Ok(await Mediator.Send(new GetGroupDefinitionAllDependenciesByIdQuery { GroupDefinitionId = groupDefinitionId }));
        }

    }
}