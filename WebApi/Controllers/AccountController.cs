using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Account;
using Application.DTOs.Account.Queries.GetAllUsers;
using Application.DTOs.Account.Queries.GetUserById;
using Application.DTOs.Account.Commands.UpdateAccount;
using Application.DTOs.Account.Commands.DeleteAccountById;
using Application.DTOs.Email;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Cors.EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        //Email send Mohamed Reda 2-10-2020
        private readonly IEmailService _emailService;


        public AccountController(IAccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;

            //Email send Mohamed Reda 2-10-2020
            _emailService = emailService;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()));
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterAsync(request, origin));
        }

        [HttpPost("register-business")]
        public async Task<IActionResult> RegisterBusinessAsync(RegisterBusinessRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterBusinessAsync(request, origin));
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {

            return Ok(await _accountService.ResetPassword(model));
        }
        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        //CUSTOM:MEB:BEGIN:10.09.2020:Accept Users
        //[Authorize(Roles = "SuperAdmin")]
        //TODO: enable authorization
        [HttpPost("accept-business-user")]
        public async Task<IActionResult> AcceptBusinessUser(AcceptBusinessUserRequest model)
        {
            await _accountService.AcceptBusinessUser(model, Request.Headers["origin"]);
            return Ok();
        }
        //CUSTOM:MEB:END:10.09.2020:Accept Users

        // GET: api/<controller>
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllUsersQuery() { Role = filter.Role, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        // GET: api/<controller>
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        [HttpPut("updateBasic-{id}")]
        public async Task<IActionResult> Put(string id, UpdateBasicUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("updateBusiness-{id}")]
        public async Task<IActionResult> Put(string id, UpdateBusinessUserCommand command)
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
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommand { Id = id }));
        }

    }
}
