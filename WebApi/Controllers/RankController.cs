using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class RankController : BaseApiController
    {
        [HttpGet("GetRankForStudentGroupInstance")]
        public async Task<IActionResult> GetRankForStudentGroupInstance(string StudentId)
        {

            return Ok(await Mediator.Send(new GetAllRankedUsersQuery()
            {
                UserId = AuthenticatedUserService.UserId,
                isInstance = true
            })); ;
        }

        [HttpGet("GetRankForStudentGroupDefinition")]
        public async Task<IActionResult> GetRankForStudentGroupDefinition(string StudentId)
        {

            return Ok(await Mediator.Send(new GetAllRankedUsersQuery()
            {
                UserId = AuthenticatedUserService.UserId,
                isInstance = false
            }));
        }

    }
}
