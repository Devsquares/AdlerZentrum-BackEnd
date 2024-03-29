using Application.Features;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class BanRequestController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllBanRequestsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("GetAllNewRequests")]
        public async Task<IActionResult> GetAllNewRequests([FromQuery] GetAllNewBanRequestsQuery filter)
        {

            return Ok(await Mediator.Send(new GetAllNewBanRequestsQuery()
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            }));
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetBanRequestByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Post(CreateBanRequestCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Put(int id, UpdateBanRequestCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

    }
}
