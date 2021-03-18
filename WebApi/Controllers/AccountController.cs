using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Account;
using Application.DTOs.Account.Commands.UpdateAccount;
using Application.DTOs.Account.Commands.DeleteAccountById;
using Application.DTOs.Email;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Application.Filters;
using Application.DTOs;
using Domain.Models;
using Application.Features.TestInstance.Queries;
using System.Transactions;

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
        private readonly IEmailService _emailService;

        public AccountController(IAccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()));
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] string RefreshToken)
        {
            var response = _accountService.RefreshToken(RefreshToken, GenerateIPAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            using (TransactionScope scope = new TransactionScope())
            {
                var student = await _accountService.RegisterAsync(request, origin);
                await Mediator.Send(new RegisterStudentGroupDefinitionCommand { groupDefinitionId = request.GroupDefinitionId.Value, StudentId = student.data, PromoCodeInstanceId = request.PromoCodeInstanceID, PlacmentTestId = request.PlacmentTestId, Email = request.Email });
                scope.Complete();
                return Ok();
            }

        }

        [HttpPost("addAccount")]
        public async Task<IActionResult> AddAccountAsync(AddAccountRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.AddApplicationUserAsync(request, origin, request.Role));
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

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(VerifyEmailRequest model)
        {
            return Ok(await _accountService.ChangePassword(model));
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

        [HttpGet("GetAllStaffByRole")]
        public async Task<IActionResult> GetAllStaffByRole([FromQuery] GetAllUsersParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery() { Role = filter.Role, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] RequestParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }


        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        [HttpPut("update-{id}")]
        public async Task<IActionResult> Put(string id, UpdateBasicUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommand { Id = id }));
        }

        [HttpGet("GetAllStudentUsers")]
        public async Task<IActionResult> GetAllStudentUsers(int PageSize, int PageNumber, int? groupDefinitionId, int? groupInstanceId, string studentName)
        {
            if (PageSize == 0)
            {
                PageSize = 10;
            }
            if (PageNumber == 0)
            {
                PageNumber = 1;
            }
            var report = await _accountService.GetPagedReponseStudentUsersAsync(PageNumber, PageSize, groupDefinitionId, groupInstanceId, studentName);
            return Ok(report);
        }

        [HttpPost("SendMessageToAdmin")]
        public async Task<IActionResult> SendMessageToAdmin([FromQuery] string subject, [FromQuery] string message, [FromQuery] string studentId)
        {
            // TODO: Which admin we will send to him.
            return Ok();
        }

        [HttpPost("SendMessageToInstructor")]
        public async Task<IActionResult> SendMessageToInstructor([FromQuery] string subject, [FromQuery] string message, [FromQuery] string studentId)
        {
            // TODO: fix it.
            // await _accountService.SendMessageToInstructor(subject, message, studentId);
            return Ok();
        }
        [HttpPut("UpdatePhoto")]
        public async Task<IActionResult> UpdatePhoto([FromBody] UserPhotoModel userPhoto)
        {
            await _accountService.UpdatePhoto(userPhoto.base64photo, userPhoto.UserId);
            return Ok();
        }

        [HttpPut("UpdatePhoneNumber")]
        public async Task<IActionResult> UpdatePhoneNumber(string newPhoneNumber, string studentId)
        {
            await _accountService.UpdatePhoneNumber(newPhoneNumber, studentId);
            return Ok();
        }

        [HttpGet("GetCurrentProgress")]
        public async Task<IActionResult> GetCurrentProgress(string studentId)
        {
            return Ok(await Mediator.Send(new GetCurrentProgressForStudent() { StudentId = studentId }));
        }

        [HttpPost("TestRegister")]
        public async Task TestRegisterAsync(TestRegisterRequest request)
        {

            var origin = Request.Headers["origin"];

            int count = request.count;
            string identifier = request.identifier;
            string email;
            string firstName;
            string lastName;
            string userName;

            for (int i = 0; i < count; i++)
            {
                email = identifier + "testStudent" + request.GroupDefinitionId + i + "@gmail.com";
                firstName = identifier + "student" + request.GroupDefinitionId + i;
                lastName = identifier + "student" + request.GroupDefinitionId + i;
                userName = email;

                RegisterRequest registerRequest = new RegisterRequest();
                Reflection.CopyProperties(request, registerRequest);
                registerRequest.Email = email;
                registerRequest.FirstName = firstName;
                registerRequest.LastName = lastName;
                registerRequest.UserName = userName;
                using (TransactionScope scope = new TransactionScope())
                {
                    var student = await _accountService.RegisterAsync(registerRequest, origin);
                    await Mediator.Send(new RegisterStudentGroupDefinitionCommand { groupDefinitionId = request.GroupDefinitionId.Value, StudentId = student.data, PromoCodeInstanceId = request.PromoCodeInstanceID, PlacmentTestId = request.PlacmentTestId, Email = registerRequest.Email });
                    scope.Complete();
                }
            }
        }
    }
}
