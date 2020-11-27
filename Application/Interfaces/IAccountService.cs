using Application.DTOs.Account;
using Application.DTOs.Account.Commands.UpdateAccount;
using Application.Wrappers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService : IGenericRepositoryAsync<Account>
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> AddAccountAsync(RegisterRequest request, string origin, string role);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Account> GetByIdAsync(string id);
        Task<IReadOnlyList<Account>> GetPagedReponseUsersAsync(string role, int pageNumber, int pageSize);
        Task<IdentityResult> UpdateAsync(UpdateBasicUserCommand updateUserCommand);
        Task DeleteAsync(string id);
        Task<Account> GetByClaimsPrincipalAsync(ClaimsPrincipal user);
    }
}
