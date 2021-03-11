using Application.Features.ForumComment.Commands.CreateForumComment;
using Application.Features.ForumComment.Commands.DeleteForumCommentById;
using Application.Features.ForumComment.Commands.UpdateForumComment;
using Application.Features.ForumComment.Queries.GetAllForumComments;
using Application.Features.ForumComment.Queries.GetForumCommentById;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ForumCommentController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllForumCommentsParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllForumCommentsQuery() {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
                ForumTopicId = filter.ForumTopicId
            }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetForumCommentByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Post(CreateForumCommentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        
        public async Task<IActionResult> Put(int id, UpdateForumCommentCommand command)
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
            return Ok(await Mediator.Send(new DeleteForumCommentByIdCommand { Id = id }));
        }

    }
}
