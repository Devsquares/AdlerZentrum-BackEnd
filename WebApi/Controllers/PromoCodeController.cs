using Application.DTOs;
using Application.DTOs.Level.Queries;
using Application.Features.PromoCodeInstance.Commands;
using Application.Features.PromoCodeInstance.Commands.CreatePromoCodeInstance;
using Application.Features.PromoCodeInstance.Queries.GetAllPromoCodeInstances;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class PromoCodeController : BaseApiController
    {
        [HttpGet("CheckPromoCode")]
        public async Task<IActionResult> CheckPromoCode([FromQuery] CheckPromoCodeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // [HttpGet("GetGroupByPromoCodeQuery")]
        // public async Task<IActionResult> GetGroupByPromoCodeQuery([FromQuery] GetGroupByPromoCodeQuery command)
        // {
        //     return Ok(await Mediator.Send(command));
        // }

        [HttpPost("CreatePromoCodeInstance")]
        public async Task<IActionResult> CheckPromoCode([FromQuery] CreatePromoCodeInstanceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetAllReport")]
        public async Task<IActionResult> GetAllReport([FromQuery] GetAllPromoCodeInstancesQuery command)
        {
            return Ok(await Mediator.Send(command));
        }


        [HttpGet("GetPromoCodeInstance")]
        public async Task<IActionResult> GetPromoCodeInstance([FromQuery] CheckPromoCodeInstanceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetAllPromoCode")]
        public async Task<IActionResult> GetAllPromoCode([FromQuery] GetAllPromoCodesQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
        
    }
}
