﻿using Application.DTOs.Account;
using Application.DTOs.Account.Commands.UpdateAccount;
using Application.Wrappers;
using Domain.Entities;
using Domain.Models;
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
        Task<Response<string>> ChangePassword(VerifyEmailRequest model); 
         Task<ApplicationUser> GetByIdAsync(string id);
        Task<IReadOnlyList<ApplicationUser>> GetPagedReponseUsersAsync(int pageNumber, int pageSize);
        Task<IdentityResult> UpdateAsync(UpdateBasicUserCommand updateUserCommand);
        Task DeleteAsync(string id);
        Task<ApplicationUser> GetByClaimsPrincipalAsync(ClaimsPrincipal user);
        Task<bool> BanAsync(string id, string comment);
        Task<PagedResponse<IEnumerable<object>>> GetPagedReponseStudentUsersAsync(int pageNumber, int pageSize, int? groupDefinitionId, int? groupInstanceId, string studentName);
        Task<List<ApplicationUser>> GetAllRankedusers();
        Task SendMessageToInstructor(string subject, string message, string studentId);
        Task UpdatePhoneNumber(string newPhoneNumber, string studentId);
        Task UpdatePhoto(string base64photo, string studentId);
        Task<PagedResponse<List<TeachersModel>>> GetAllTeachers(int pageNumber, int pageSize, string teacherName);
        Task UpdateAdlerCardBalance(string studentId, int balance);
    }
}
