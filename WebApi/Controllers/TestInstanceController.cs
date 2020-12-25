using Application.Features.TestInstance.Commands.CreateTestInstance;
using Application.Features.TestInstance.Commands.DeleteTestInstanceById;
using Application.Features.TestInstance.Commands.UpdateTestInstance;
using Application.Features.TestInstance.Queries.GetAllTestInstances;
using Application.Features.TestInstance.Queries.GetTestInstanceById;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Controllers;

namespace WebApi.Controller
{
    public class TestInstanceController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllTestInstancesParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllTestInstancesQuery() {
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

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetTestInstanceByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]
        //TODO: enable authorization
        public async Task<IActionResult> Post(CreateTestInstanceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        //TODO: enable authorization
        public async Task<IActionResult> Put(int id, UpdateTestInstanceCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        //TODO: enable authorization
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteTestInstanceByIdCommand { Id = id }));
        }

    }
}
