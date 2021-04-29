using Application.DTOs;
using Application.DTOs.Account;
using Application.DTOs.Account.Commands.UpdateAccount;
using Application.DTOs.AccountDTO;
using Application.Wrappers;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
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
        Task<Response<bool>> CheckRegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> AddApplicationUserAsync(AddAccountRequest request, string origin, int role);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<string>> ChangePassword(VerifyEmailRequest model);
        Task<ApplicationUser> GetByIdAsync(string id);
        IList<GetAllUsersViewModel> GetPagedReponseUsersAsync(int pageNumber, int pageSize, string role, out int count);
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
        Task<PagedResponse<IEnumerable<UserClaimsModel>>> GetNonAllUserClaims(int pageNumber, int pageSize, string email, string name, string claimtype);
        IList<GetAllUsersViewModel> GetPagedReponseStaffAsync(int pageNumber, int pageSize, string role, out int count);
        Task AddPaymentTransaction(PaymentTransactionInputModel inputModel);

        List<StudentAnalysisReportModel> GetStudentAnalysisReport(int pageNumber, int PageSize, string studentName, DateTime? from, DateTime? to
          , int? attendancefrom, int? attendanceto, int? LateSubmissionsfrom, int? LateSubmissionsto,
          int? MissedSubmissionsfrom, int? MissedSubmissionsto, int? CurrentProgressPointsfrom, int? CurrentProgressPointsTo, out int count);
    }
}
