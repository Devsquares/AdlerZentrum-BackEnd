using Application.Features;
using Application.Features.AdlerCardsUnit.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AdlerCardsUnitController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet("GetALLAdlerCardUnits")]
        public async Task<IActionResult> Get([FromQuery] GetAllAdlerCardsUnitsParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllAdlerCardsUnitsQuery()
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

        [HttpGet("GetAdlerCardUnitsForStudent")]
        public async Task<IActionResult> GetAdlerCardUnitsForStudent([FromQuery] GetAllAdlerCardsUnitsParameter filter)
        {
            return Ok(await Mediator.Send(new GetAdlerCardsUnitByIdQuery() ));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetAdlerCardsUnitByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]
        //TODO: enable authorization
        public async Task<IActionResult> Post(CreateAdlerCardsUnitCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        //TODO: enable authorization
        public async Task<IActionResult> Put(int id, UpdateAdlerCardsUnitCommand command)
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
            return Ok(await Mediator.Send(new DeleteAdlerCardsUnitByIdCommand { Id = id }));
        }

        [HttpGet("GetAdlerCardUnitsByLevelAndType")]
        public async Task<IActionResult> GetAdlerCardUnitsByLevelAndType(int levelId,int adlerCardtypeId)
        {

            return Ok(await Mediator.Send(new GetAdlerCardUnitsByLevelAndTypeQuery()
            {
                AdlerCardTypeId = adlerCardtypeId,
                LevelId = levelId
            }));
        }

    }
}
