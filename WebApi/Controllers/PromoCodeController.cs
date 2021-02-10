using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class PromoCodeController : BaseApiController
    {
        [HttpGet("CheckPromoCode")]
        public async Task<IActionResult> CheckPromoCode([FromQuery]CheckPromoCodeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // [HttpGet("GetGroupByPromoCodeQuery")]
        // public async Task<IActionResult> GetGroupByPromoCodeQuery([FromQuery] GetGroupByPromoCodeQuery command)
        // {
        //     return Ok(await Mediator.Send(command));
        // }
    }
}
