using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Enums;
using System.Threading.Tasks;
using Application.DTOs.Email;
using Domain.Entities;
using Application.Filters;
using Application.DTOs.Account;
using Application.DTOs.Account.Commands.UpdateAccount;
using Org.BouncyCastle.Ocsp;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;

namespace Infrastructure.Persistence.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
        private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;

        public AccountService(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            IGroupInstanceRepositoryAsync groupInstanceRepositoryAsync,
            IGroupInstanceStudentRepositoryAsync  groupInstanceStudentRepositoryAsync)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            _emailService = emailService;
            _groupInstanceRepositoryAsync = groupInstanceRepositoryAsync;
            _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"No ApplicationUsers Registered with {request.Email}.");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }
            if (!user.EmailConfirmed)
            {
                throw new ApiException($"ApplicationUser Not Confirmed for '{request.Email}'.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            int? activeGroup = null;
            if (roles.Contains("Student"))
            {
                activeGroup = _groupInstanceRepositoryAsync.GetActiveGroupInstance(user.Id);
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, roles, activeGroup);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;
            response.ActiveGroupInstance = activeGroup;
            response.ChangePassword = user.ChangePassword;
            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.");
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,

            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                //TODO need to be change
                user.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RolesEnum.Student.ToString());

                    //var verificationUri = await SendVerificationEmail(user, origin);


                    //return new Response<string>(user.Id, message: $"User Registered. Please confirm your ApplicationUser by visiting this URL {verificationUri}");
                    // _groupInstanceRepositoryAsync.AddStudentToTheGroupInstance(request.GroupInstanceId, user.Id);
                    await _groupInstanceStudentRepositoryAsync.AddAsync(new GroupInstanceStudents
                    {
                        GroupInstanceId = request.GroupInstanceId,
                        StudentId = user.Id,
                        IsDefault = true
                    });
                    return new Response<string>(user.Id, message: $"User Registered.");
                }
                else
                {
                    throw new ApiException($"{result.Errors}");
                }
            }
            else
            {
                throw new ApiException($"Email {request.Email } is already registered.");
            }
        }

        private bool checkeRole(RolesEnum registrationUserRole, int creatorUserRole)
        {
            bool result = false;
            switch (registrationUserRole)
            {
                case RolesEnum.SuperAdmin:
                    if (creatorUserRole == (int)RolesEnum.SuperAdmin) result = true;
                    break;
                case RolesEnum.Supervisor:
                    if (creatorUserRole == (int)RolesEnum.SuperAdmin) result = true;
                    break;
                case RolesEnum.Secretary:
                    if (creatorUserRole == (int)RolesEnum.SuperAdmin || creatorUserRole == (int)RolesEnum.Supervisor) result = true;
                    break;
                case RolesEnum.Teacher:
                    if (creatorUserRole == (int)RolesEnum.SuperAdmin || creatorUserRole == (int)RolesEnum.Supervisor) result = true;
                    break;

                default:
                    break;
            }
            return result;
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user, IList<string> roles, int? activeGroup)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            //TODO: MEB: 13.09.2020: Uncommenting this leads to internal server problem (?)
            //string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                new Claim("GroupInstance",activeGroup.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //TODO: Email Service Call Here

            await _emailService.SendAsync(new Application.DTOs.Email.EmailRequest() { To = user.Email, Body = $"Please confirm your ApplicationUser by visiting this URL {verificationUri}", Subject = "Confirm Registration" });

            return verificationUri;
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new Response<string>(user.Id, message: $"ApplicationUser Confirmed for {user.Email}. You can now use the /api/ApplicationUser/authenticate endpoint.");
            }
            else
            {
                throw new ApiException($"An error occured while confirming {user.Email}.");
            }
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var ApplicationUser = await _userManager.FindByEmailAsync(model.Email);

            // always return ok response to prevent email enumeration
            if (ApplicationUser == null) return;

            var code = await _userManager.GeneratePasswordResetTokenAsync(ApplicationUser);
            var route = "api/ApplicationUser/reset-password/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailRequest = new EmailRequest()
            {
                Body = $"You reset token is - {code}",
                To = model.Email,
                Subject = "Reset Password",
            };
            await _emailService.SendAsync(emailRequest);
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var ApplicationUser = await _userManager.FindByEmailAsync(model.Email);
            if (ApplicationUser == null) throw new ApiException($"No ApplicationUsers Registered with {model.Email}.");
            //string tok = _userManager.GeneratePasswordResetTokenAsync(ApplicationUser).Result;
            //await _userManager.ResetPasswordAsync(ApplicationUser, tok, "P@$$w0rd");
            var result = await _userManager.ChangePasswordAsync(ApplicationUser, model.CurrentPassword, model.Password);
            if (result.Succeeded)
            {
                ApplicationUser.ChangePassword = false;
                await _userManager.UpdateAsync(ApplicationUser);
                return new Response<string>(model.Email, message: $"Password Resetted.");
            }
            else
            {
                string ex = result.Errors.FirstOrDefault()?.Description;
                if (string.IsNullOrEmpty(ex)) ex = $"Error occured while reseting the password.";
                throw new ApiException(ex);
            }
        }


        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ApiException("No user found with id " + id);
            }
            return user;
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetPagedReponseUsersAsync(int pageNumber, int pageSize)
        {
            List<ApplicationUser> users = _userManager.Users.Include(u => u.Role).ToList();
            return users.AsReadOnly();
        }

        public async Task DeleteAsync(string id)
        {
            if (id == null)
                return;
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new ApiException("No user found with id " + id);
            await _userManager.DeleteAsync(user);
        }

        public Task<ApplicationUser> GetByClaimsPrincipalAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = _userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new ApiException("No user found.");
            }
            return user;
        }

        public async Task<Response<string>> AddApplicationUserAsync(AddAccountRequest request, string origin, int role)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.");
            }
            ApplicationUser user = new ApplicationUser();
            Reflection.CopyProperties(request, user);

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                user.ChangePassword = true;
                //TODO need to be change
                user.EmailConfirmed = true;

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    RolesEnum userRole = (RolesEnum)role;
                    // get it from claim.
                    if (!checkeRole((RolesEnum)request.Role, 0))
                    {
                        throw new ApiException($"You are not autrized to create ApplicationUser with role {request.Role}.");
                    }
                    await _userManager.AddToRoleAsync(user, userRole.ToString());

                    //var verificationUri = await SendVerificationEmail(user, origin);

                    //return new Response<string>(user.Id, message: $"User Registered. Please confirm your ApplicationUser by visiting this URL {verificationUri}");

                    return new Response<string>(user.Id, message: $"User Registered.");
                }
                else
                {
                    throw new ApiException($"{result.Errors}");
                }
            }
            else
            {
                throw new ApiException($"Email {request.Email } is already registered.");
            }
        }

        async Task<IdentityResult> IAccountService.UpdateAsync(UpdateBasicUserCommand updateUserCommand)
        {
            if (updateUserCommand == null)
                return null;
            var user = await _userManager.FindByIdAsync(updateUserCommand.Id);
            if (user == null)
                return null;
            Reflection.CopyProperties(updateUserCommand, user);
            return await _userManager.UpdateAsync(user);
        }
    }

}
