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
            private readonly IJobRepositoryAsync _jobRepository;
            private readonly IMailJobRepositoryAsync _mailJobRepository;
            public UpdateDisqualificationRequestCommandHandler(IDisqualificationRequestRepositoryAsync disqualificationrequestRepository,
               IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
               IJobRepositoryAsync jobRepositoryAsync,
                IMailJobRepositoryAsync mailJobRepositoryAsync)
            {
                _disqualificationrequestRepository = disqualificationrequestRepository;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _jobRepository = jobRepositoryAsync;
                _mailJobRepository = mailJobRepositoryAsync;
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
                        student.IsDefault = false;
                        student.DisqualifiedComment = command.Comment;
                        await _groupInstanceStudentRepositoryAsync.UpdateAsync(student);

                        await _jobRepository.AddAsync(new Job
                        {
                            Type = (int)JobTypeEnum.Disqualifier,
                            GroupInstanceId = student.GroupInstanceId,
                            StudentId = student.StudentId,
                            Status = (int)JobStatusEnum.New
                        });
                        await _mailJobRepository.AddAsync(new MailJob
                        {
                            Type = (int)MailJobTypeEnum.Disqualification,
                            StudentId = student.StudentId,
                            Status = (int)JobStatusEnum.New
                        });
                    }
                }
                return new Response<int>(disqualificationrequest.Id);
            }
        }

    }
}
