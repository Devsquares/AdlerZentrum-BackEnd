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
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Internal;
using Domain.Models;
using MediatR;
using Application.DTOs;

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
        private readonly IGroupDefinitionRepositoryAsync _groupDefinitionRepositoryAsync;
        private readonly IGroupConditionRepositoryAsync _groupConditionRepositoryAsync;
        private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
        private readonly ISublevelRepositoryAsync _sublevelRepositoryAsync;
        private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignmentRepositoryAsync;

        private readonly ApplicationDbContext _context;

        public AccountService(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            IGroupInstanceRepositoryAsync groupInstanceRepositoryAsync,
            IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
            IGroupDefinitionRepositoryAsync groupDefinitionRepositoryAsync,
            IGroupConditionRepositoryAsync groupConditionRepositoryAsync,
            ApplicationDbContext context,
            ISublevelRepositoryAsync sublevelRepositoryAsync,
            ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignmentRepositoryAsync)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            _emailService = emailService;
            _groupInstanceRepositoryAsync = groupInstanceRepositoryAsync;
            _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
            _groupDefinitionRepositoryAsync = groupDefinitionRepositoryAsync;
            _groupConditionRepositoryAsync = groupConditionRepositoryAsync;
            _context = context;
            _teacherGroupInstanceAssignmentRepositoryAsync = teacherGroupInstanceAssignmentRepositoryAsync;
            _sublevelRepositoryAsync = sublevelRepositoryAsync;
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
            response.Banned = user.Banned;
            response.BanComment = user.BanComment;
            response.SubLevelId = user.SublevelId;
            response.PlacementTestId = user.PlacmentTestId;

            if (user.SublevelId.HasValue && user.SublevelId != 0)
            {
                var subLevel = _sublevelRepositoryAsync.GetByIdAsync(user.SublevelId.Value).Result;
                response.IsFinal = subLevel.IsFinal;
                response.SubLevelName = subLevel.Name;
            }
            response.Profilephoto = user.Profilephoto;

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        public async Task<Response<AuthenticationResponse>> RefreshToken(string token, string ipAddress)
        {
            var user = _userManager.Users.Include(x => x.RefreshTokens).SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.RefreshTokens.Where(x => x.Token == token).SingleOrDefault();

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            int? activeGroup = null;
            if (roles.Contains("Student"))
            {
                activeGroup = _groupInstanceRepositoryAsync.GetActiveGroupInstance(user.Id);
            }

            // generate new jwt
            var jwtToken = await GenerateJWToken(user, roles, activeGroup);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.RefreshToken = refreshToken.Token;
            response.ActiveGroupInstance = activeGroup;
            response.ChangePassword = user.ChangePassword;
            response.SubLevelId = user.SublevelId;
            response.PlacementTestId = user.PlacmentTestId;

            if (user.SublevelId.HasValue && user.SublevelId != 0)
            {
                var subLevel = _sublevelRepositoryAsync.GetByIdAsync(user.SublevelId.Value).Result;
                response.IsFinal = subLevel.IsFinal;
                response.SubLevelName = subLevel.Name;
            }

            return new Response<AuthenticationResponse>(response, $"Token Refreshed.");
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            if (request.Password == null ||
             request.ConfirmPassword == null ||
             request.Email == null ||
             request.LastName == null ||
             request.FirstName == null)
            {
                throw new ApiException($"One of this fields missing: Password, ConfirmPassword, Email, LastName, FirstName.");
            }
            if (request.Password.Length < 6)
            {
                throw new ApiException($"password Minimum length 6.");
            }
            if (request.ConfirmPassword != request.Password)
            {
                throw new ApiException($"Confirm Password wrong.");
            }
            if (!IsValidEmail(request.Email))
            {
                throw new ApiException($"Email Not Vaild.");
            }
            request.UserName = request.Email;
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
                UserName = request.Email,
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
                    if (request.PlacmentTestId.HasValue || request.IsAdlerService)
                    {
                        return new Response<string>(user.Id, message: $"User Registered.");
                    }

                    ////var verificationUri = await SendVerificationEmail(user, origin);


                    ////return new Response<string>(user.Id, message: $"User Registered. Please confirm your ApplicationUser by visiting this URL {verificationUri}");
                    //// _groupInstanceRepositoryAsync.AddStudentToTheGroupInstance(request.GroupInstanceId, user.Id);

                    //int count = _groupInstanceStudentRepositoryAsync.GetCountOfStudents(request.GroupInstanceId);
                    //var groupInstance = _groupInstanceRepositoryAsync.GetByIdAsync(request.GroupInstanceId).Result;

                    //var groupDefinition = _groupDefinitionRepositoryAsync.GetByIdAsync(groupInstance.GroupDefinitionId).Result;
                    //var condtion = _groupConditionRepositoryAsync.GetByIdAsync(groupDefinition.GroupConditionId);

                    //if (count > condtion.Result.NumberOfSolts)
                    //{
                    //    throw new ApiException($"Group is complate now, Contact the admin.");
                    //}
                    //await _groupInstanceStudentRepositoryAsync.AddAsync(new GroupInstanceStudents
                    //{
                    //    GroupInstanceId = request.GroupInstanceId,
                    //    StudentId = user.Id,
                    //    IsDefault = true
                    //});
                    return new Response<string>(user.Id, message: $"User Registered.");
                }
                else
                {
                    throw new ApiException($"{result.Errors.FirstOrDefault().Description}");
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

        public async Task<Response<string>> ChangePassword(VerifyEmailRequest model)
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

        public async Task<bool> BanAsync(string id, string comment)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ApiException("No user found with id " + id);
            }
            user.Banned = true;
            user.BanComment = comment;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<IList<ApplicationUser>> GetPagedReponseUsersAsync(int pageNumber, int pageSize, string role)
        {
            IList<ApplicationUser> users;
            if (role == "Student") return null;
            if (!string.IsNullOrEmpty(role) && role != "Guest")
            {
                users = _userManager.GetUsersInRoleAsync(role).Result;
            }
            else
            {
                users = _userManager.Users.Include(u => u.Role).ToList();
            }
            return users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
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

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new Response<string>(model.Email, message: $"Password Resetted.");
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }
        }
        public async Task<PagedResponse<IEnumerable<object>>> GetPagedReponseStudentUsersAsync(int pageNumber, int pageSize, int? groupDefinitionId, int? groupInstanceId, string studentName)
        {
            var studentRoles = (from user in _userManager.Users
                                join userrole in _context.UserRoles on user.Id equals userrole.UserId
                                join role in _context.Roles on userrole.RoleId equals role.Id
                                join gis in _context.GroupInstanceStudents on user.Id equals gis.StudentId

                                into gj
                                from x in gj.DefaultIfEmpty()
                                where role.NormalizedName.ToLower() == RolesEnum.Student.ToString().ToLower() &&
                                ((groupDefinitionId != null ? x.GroupInstance.GroupDefinitionId == groupDefinitionId : true)) &&
                                (groupInstanceId != null ? x.GroupInstanceId == groupInstanceId : true) &&
                                (!string.IsNullOrEmpty(studentName) ? (user.FirstName.ToLower().Contains(studentName.ToLower()) || user.LastName.ToLower().Contains(studentName.ToLower())) : true)
                                select new
                                {
                                    Id = user.Id,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    Email = user.Email,
                                    PhoneNumber = user.PhoneNumber,
                                    GroupSerial = x.GroupInstance != null ?x.GroupInstance.Serial : string.Empty
                                }).ToList();
            //var studentRoles = _userManager.Users.Include(x => x.Role).Where(x => x.Role.NormalizedName == "STUDENT").LeftJoin(
            //          _context.GroupInstanceStudents,
            //          user => user.Id,
            //          gis => gis.StudentId,
            //          (user, gis) => new
            //          {
            //              user,
            //              gis
            //          }
            //         )
            //   // .Include(x => x.gis.GroupInstance)
            //    .Where(x => (groupDefinitionId != null ? x.gis.GroupInstance.GroupDefinitionId == groupDefinitionId : true) &&
            //     (groupInstanceId != null ? x.gis.GroupInstanceId == groupInstanceId : true))
            //    .Select(x => new
            //    {
            //        Id = x.user.Id,
            //        FirstName = x.user.FirstName,
            //        LastName = x.user.LastName,
            //        Email = x.user.Email,
            //        PhoneNumber = x.user.PhoneNumber,
            //        GroupDefinitionId = x.gis.GroupInstance.GroupDefinitionId,
            //        GroupInstancenId = x.gis.GroupInstanceId
            //    }).ToList();
            int totalCount = studentRoles.Count();
            studentRoles = studentRoles.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResponse<IEnumerable<object>>(studentRoles, pageNumber, pageSize, totalCount);
        }

        public async Task<List<ApplicationUser>> GetAllRankedusers()
        {
            return _userManager.Users.Take(10).ToList();
        }
        public async Task SendMessageToInstructor(string subject, string message, string studentId)
        {
            var groupInstanceObject = _groupInstanceStudentRepositoryAsync.GetLastByStudentId(studentId);
            if (groupInstanceObject == null)
            {
                throw new ApiException("No Group instance for this student");
            }
            var teacher = _teacherGroupInstanceAssignmentRepositoryAsync.GetByGroupInstanceId(groupInstanceObject.GroupInstanceId);
            var student = _userManager.Users.Where(x => x.Id == studentId).FirstOrDefault();
            EmailRequest emailRequest = new EmailRequest()
            {
                From = student.Email,
                To = teacher.Teacher.Email,
                Body = message,
                Subject = subject
            };
            await _emailService.SendAsync(emailRequest);

        }

        public async Task UpdatePhoto(string base64photo, string studentId)
        {
            //string base64Decoded;
            //byte[] data = System.Convert.FromBase64String(base64photo);
            //base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
            var student = _userManager.Users.Where(x => x.Id == studentId).FirstOrDefault();
            student.Profilephoto = base64photo;
            await _userManager.UpdateAsync(student);

        }

        public async Task UpdatePhoneNumber(string newPhoneNumber, string studentId)
        {
            var student = _userManager.Users.Where(x => x.Id == studentId).FirstOrDefault();
            student.PhoneNumber = newPhoneNumber;
            await _userManager.UpdateAsync(student);

        }

        public async Task<PagedResponse<List<TeachersModel>>> GetAllTeachers(int pageNumber, int pageSize, string teacherName)
        {
            var teacherRoles = _userManager.GetUsersInRoleAsync("TEACHER").Result;
            var teachers = teacherRoles.Where(x => (!string.IsNullOrEmpty(teacherName) ? (x.FirstName.ToLower().Contains(teacherName.ToLower()) || x.LastName.ToLower().Contains(teacherName.ToLower())) : true)).Select(x => new TeachersModel()
            {
                TeacherFirstName = x.FirstName,
                TeacherLastName = x.LastName,
                TeacherId = x.Id
            })
              .ToList();
            int totalCount = teachers.Count();
            teachers = teachers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResponse<List<TeachersModel>>(teachers, pageNumber, pageSize, totalCount);
        }

        public async Task UpdateAdlerCardBalance(string studentId, int balance)
        {
            var student = _userManager.FindByIdAsync(studentId).Result;
            student.AdlerCardBalance += balance;
            await _userManager.UpdateAsync(student);
        }

        public async Task<PagedResponse<IEnumerable<UserClaimsModel>>> GetNonAllUserClaims(int pageNumber, int pageSize, string email, string name, string claimtype)
        {
            Claim filterclaim = new Claim(claimtype, claimtype);
            var claimedusers = _userManager.GetUsersForClaimAsync(filterclaim).Result;
            var claimedusersIds = claimedusers.Select(x => x.Id);
            var teachers = _userManager.GetUsersInRoleAsync("TEACHER").Result;
            var query = teachers.Where(x => !claimedusersIds.Contains(x.Id));
            if (!string.IsNullOrEmpty(email))
            {
                query = teachers.Where(x => x.Email.ToLower() == email);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = teachers.Where(x => x.FirstName.ToLower() == name.ToLower() || x.LastName == name.ToLower());
            }


            int totalCount = query.Count();

            var queryData = query
               .Select(x => new UserClaimsModel()
               {
                   userId = x.Id,
                   FirstName = x.FirstName,
                   LastName = x.LastName,
                   Email = x.Email
               })
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .ToList();
            return new PagedResponse<IEnumerable<UserClaimsModel>>(queryData, pageNumber, pageSize, totalCount);
        }
    }

}
