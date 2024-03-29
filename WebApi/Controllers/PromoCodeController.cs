﻿using Application.DTOs;
using Application.DTOs.Level.Queries;
using Application.Features;
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
        public async Task<IActionResult> CreatePromoCodeInstance([FromQuery] CreatePromoCodeInstanceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("DeletePromoCodeInstance")]
        public async Task<IActionResult> DeletePromoCodeInstance([FromQuery] DeletePromoCodeInstanceByIdCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetAllPromocodeInstances")]
        public async Task<IActionResult> GetAllPromocodeInstances([FromQuery] GetAllPromoCodeInstancesQuery command)
        {
            if (command.PageNumber == 0)
            {
                command.PageNumber = 1;
            }
            if (command.PageSize == 0)
            {
                command.PageSize = 10;
            }
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

        [HttpPost("CreatePromoCode")]
        public async Task<IActionResult> CreatePromoCode([FromQuery] CreatePromoCodeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("DeletePromoCode")]
        public async Task<IActionResult> DeletePromoCode([FromQuery] DeletePromoCodeByIdCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}
