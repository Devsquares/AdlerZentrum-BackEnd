using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class HomeWorkController : BaseApiController
    {
        [HttpPost("SubmitHomeWorkForStudent")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SubmitHomeWorkForStudent(int id, CreateHomeWorkSubmitionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
    }
}