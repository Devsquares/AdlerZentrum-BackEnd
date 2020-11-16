﻿using Application.DTOs.Account;
using Application.DTOs.Account.Commands.UpdateAccount;
using Application.Wrappers;
using Domain.Entities;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> RegisterBusinessAsync(RegisterBusinessRequest request, string origin);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<string>> AcceptBusinessUser(AcceptBusinessUserRequest model, StringValues stringValues);
        Task<Account> GetByIdAsync(string id);
        Task<IReadOnlyList<Account>> GetPagedReponseUsersAsync(string role, int pageNumber, int pageSize);
        Task UpdateAsync(UpdateBasicUserCommand updateUserCommand);
        Task UpdateAsync(UpdateBusinessUserCommand updateUserCommand);
        Task DeleteAsync(string id);
    }
}
