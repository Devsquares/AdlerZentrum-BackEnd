using Application.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AdlerCardBundleStudentController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet("GetAllAdlerCardBundleStudent")]
        public async Task<IActionResult> Get([FromQuery] GetAllAdlerCardBundleStudentsParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllAdlerCardBundleStudentsQuery() {
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
        [HttpGet("GetById{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetAdlerCardBundleStudentByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost("BuyAdlerCardBundel")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> BuyBundel(CreateAdlerCardBundleStudentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        //[HttpPut("{id}")]
        ////[Authorize(Roles = "SuperAdmin")]
        //
        //public async Task<IActionResult> Put(int id, UpdateAdlerCardBundleStudentCommand command)
        //{
        //    if (id != command.Id)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(await Mediator.Send(command));
        //}

        // DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        ////[Authorize(Roles = "SuperAdmin")]
        //
        //public async Task<IActionResult> RefundBundle(int id)
        //{
        //    return Ok(await Mediator.Send(new DeleteAdlerCardBundleStudentByIdCommand { Id = id }));
        //}

    }
}
