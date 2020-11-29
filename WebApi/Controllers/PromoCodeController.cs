using Application.DTOs.PromoCode.Queries;
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
    }
}
