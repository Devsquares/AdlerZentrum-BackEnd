using Application.DTOs.Account;
using Application.DTOs.Account.Commands.UpdateAccount;
using Application.Wrappers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<AuthenticationResponse>> RefreshToken(string token, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> AddApplicationUserAsync(AddAccountRequest request, string origin, int role);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<IReadOnlyList<ApplicationUser>> GetPagedReponseUsersAsync(int pageNumber, int pageSize);
        Task<IdentityResult> UpdateAsync(UpdateBasicUserCommand updateUserCommand);
        Task DeleteAsync(string id);
        Task<ApplicationUser> GetByClaimsPrincipalAsync(ClaimsPrincipal user);

    }
}
