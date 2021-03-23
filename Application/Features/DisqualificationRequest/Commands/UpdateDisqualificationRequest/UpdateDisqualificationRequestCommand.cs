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
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            public UpdateDisqualificationRequestCommandHandler(IDisqualificationRequestRepositoryAsync disqualificationrequestRepository,
               IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync)
            {
                _disqualificationrequestRepository = disqualificationrequestRepository;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
            }
            public async Task<Response<int>> Handle(UpdateDisqualificationRequestCommand command, CancellationToken cancellationToken)
            {
                var disqualificationrequest = await _disqualificationrequestRepository.GetByIdAsync(command.Id);

                if (disqualificationrequest == null)
                {
                    new Response<int>($"Disqualification Request Not Found.");
                }
                else
                {
                    disqualificationrequest.StudentId = command.StudentId;
                    disqualificationrequest.Comment = command.Comment;
                    disqualificationrequest.DisqualificationRequestStatus = command.DisqualificationRequestStatus;

                    await _disqualificationrequestRepository.UpdateAsync(disqualificationrequest);
                    var student = _groupInstanceStudentRepositoryAsync.GetByStudentIdIsDefault(command.StudentId);
                    if (command.DisqualificationRequestStatus == (int)DisqualificationRequestStatusEnum.Disqualified)
                    {
                        student.Disqualified = true;
                        student.DisqualifiedComment = command.Comment;
                        await _groupInstanceStudentRepositoryAsync.UpdateAsync(student);
                    }
                }
                return new Response<int>(disqualificationrequest.Id);
            }
        }

    }
}
