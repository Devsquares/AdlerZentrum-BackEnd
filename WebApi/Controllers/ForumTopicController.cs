using Application.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{ 
    public class ForumTopicController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllForumTopicsParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllForumTopicsQuery() {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber, 
                ForumType = filter.ForumType,
                GroupDefinitionId = filter.GroupDefinitionId,
                GroupInstanceId = filter.GroupInstanceId,
                UserId = AuthenticatedUserService.UserId
            }));
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetForumTopicByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Post(CreateForumTopicCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Put(int id, UpdateForumTopicCommand command)
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
            return Ok(await Mediator.Send(new DeleteForumTopicByIdCommand { Id = id }));
        }

    }
}
