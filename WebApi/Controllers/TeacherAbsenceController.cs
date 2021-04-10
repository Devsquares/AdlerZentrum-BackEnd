using Application.Features.TeacherAbsence.Commands.CreateTeacherAbsence;
using Application.Features.TeacherAbsence.Commands.DeleteTeacherAbsenceById;
using Application.Features.TeacherAbsence.Commands.UpdateTeacherAbsence;
using Application.Features.TeacherAbsence.Queries.GetAllTeacherAbsences;
using Application.Features.TeacherAbsence.Queries.GetTeacherAbsenceById;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class TeacherAbsenceController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllTeacherAbsencesParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllTeacherAbsencesQuery() {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
                //FilterArray = filter.FilterArray,
                //FilterRange = filter.FilterRange,
                //FilterSearch = filter.FilterSearch,
                //FilterValue = filter.FilterValue,
                //SortBy = filter.SortBy,
                //SortType = filter.SortType,
                //NoPaging = filter.NoPaging
            }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetTeacherAbsenceByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]
        //TODO: enable authorization
        public async Task<IActionResult> Post(CreateTeacherAbsenceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        //TODO: enable authorization
        public async Task<IActionResult> Put(int id, UpdateTeacherAbsenceCommand command)
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
            return Ok(await Mediator.Send(new DeleteTeacherAbsenceByIdCommand { Id = id }));
        }

    }
}
