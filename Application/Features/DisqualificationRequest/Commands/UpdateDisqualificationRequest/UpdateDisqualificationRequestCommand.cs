using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class UpdateDisqualificationRequestCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string Comment { get; set; }
        public int DisqualificationRequestStatus { get; set; }

        public class UpdateDisqualificationRequestCommandHandler : IRequestHandler<UpdateDisqualificationRequestCommand, Response<int>>
        {
            private readonly IDisqualificationRequestRepositoryAsync _disqualificationrequestRepository;
            private readonly UserManager<ApplicationUser> _userManager;
            public UpdateDisqualificationRequestCommandHandler(IDisqualificationRequestRepositoryAsync disqualificationrequestRepository,
                UserManager<ApplicationUser> userManager)
            {
                _disqualificationrequestRepository = disqualificationrequestRepository;
                _userManager = userManager;
            }
            public async Task<Response<int>> Handle(UpdateDisqualificationRequestCommand command, CancellationToken cancellationToken)
            {
                var disqualificationrequest = await _disqualificationrequestRepository.GetByIdAsync(command.Id);

                if (disqualificationrequest == null)
                {
                    throw new ApiException($"DisqualificationRequest Not Found.");
                }
                else
                {
                    disqualificationrequest.StudentId = command.StudentId;
                    disqualificationrequest.Comment = command.Comment;
                    disqualificationrequest.DisqualificationRequestStatus = command.DisqualificationRequestStatus;

                    await _disqualificationrequestRepository.UpdateAsync(disqualificationrequest);
                    var student = _userManager.FindByIdAsync(command.StudentId);
                    if (command.DisqualificationRequestStatus == (int)DisqualificationRequestStatusEnum.Disqualified)
                    {
                        student.Result.Disqualified = true;
                        student.Result.DisqualifiedComment = command.Comment;
                        await _userManager.UpdateAsync(student.Result);
                    }
                    return new Response<int>(disqualificationrequest.Id);
                }
            }
        }

    }
}
