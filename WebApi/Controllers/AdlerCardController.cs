using Application.Features;
using Application.Features.AdlerCard.Commands;
using Application.Features.AdlerCard.Queries.GetAllAdlerCards;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AdlerCardController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet("GetALLAdlerCard")]
        public async Task<IActionResult> Get([FromQuery] GetAllAdlerCardsParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllAdlerCardsQuery()
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

        [HttpGet("GetAdlerCardGroupsForStudent")]
        public async Task<IActionResult> GetAdlerCardGroupsForStudent()
        {
            return Ok(await Mediator.Send(new GetAdlerCardGroupsForStudentQuery()));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetAdlerCardByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost("CreateAdlerCard")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Post(CreateAdlerCardCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Put(int id, UpdateAdlerCardCommand command)
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
            return Ok(await Mediator.Send(new DeleteAdlerCardByIdCommand { Id = id }));
        }

        [HttpPost("ActivateAdlerCard")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> ActivateAdlerCard(int AdlerCardID)
        {
            return Ok(await Mediator.Send(new ActivateAdlerCardCommand { AdlerCardId = AdlerCardID }));
        }

        [HttpGet("GetAdlerCardForStudent")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> GetAdlerCardForStudent(GetAdlerCardForStudent adlerCardForStudent)
        {
            
            return Ok(await Mediator.Send(adlerCardForStudent));
        }

        [HttpGet("GetAllAdlerCardForStudent")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> GetALLAdlerCardForStudent(int adlerCardUnitId, string studentId)
        {
            return Ok(await Mediator.Send(new GetAllAdlerCardsForStudent { AdlerCardUnitId = adlerCardUnitId , StudentId = studentId }));
        }

    }
}
