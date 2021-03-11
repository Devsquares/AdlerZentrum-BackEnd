using Application.Features;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AdlerCardsBundleController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet("GetAllAdlerCardBundle")]
        public async Task<IActionResult> Get([FromQuery] GetAllAdlerCardsBundlesParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllAdlerCardsBundlesQuery() {
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
            return Ok(await Mediator.Send(new GetAdlerCardsBundleByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost("CreateAdlerCardBundle")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Post(CreateAdlerCardsBundleCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Put(int id, UpdateAdlerCardsBundleCommand command)
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
        
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteAdlerCardsBundleByIdCommand { Id = id }));
        }

    }
}
