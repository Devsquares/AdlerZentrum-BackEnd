using System.Threading.Tasks;
using Application.DTOs.GroupCondition.Commands;
using Application.DTOs.GroupCondition.Queries;
using Application.DTOs.GroupCondition.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class GroupConditionController : BaseApiController
    {

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GroupConditionRequestParameter filter)
        {
            
            return Ok(await Mediator.Send(new GetAllGroupConditionsQuery()
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

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetGroupConditionByIdQuery { Id = id }));
        }

        [HttpPost("Create")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post(int id, CreateGroupConditionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }



        [HttpPut("update")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Put(int id, UpdateGroupConditionCommand command)
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
            return Ok(await Mediator.Send(new DeleteGroupConditionByIdCommand { Id = id }));
        }
    }
}