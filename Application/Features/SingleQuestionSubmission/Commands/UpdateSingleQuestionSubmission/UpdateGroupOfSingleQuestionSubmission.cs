using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class UpdateGroupOfSingleQuestionSubmission : IRequest<Response<int>>
    {
        public List<UpdateSingleQuestionSubmissionCommand> SingleQuestionSubmission { get; set; }

        public class UpdateGroupOfSingleQuestionSubmissionHandler : IRequestHandler<UpdateGroupOfSingleQuestionSubmission, Response<int>>
        {
            private readonly ISingleQuestionSubmissionRepositoryAsync _singlequestionsubmissionRepository;
            private readonly IMediator _medaitor;
            public UpdateGroupOfSingleQuestionSubmissionHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository, IMediator Mediator)
            {
                _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
                _medaitor = Mediator;
            }
            public async Task<Response<int>> Handle(UpdateGroupOfSingleQuestionSubmission command, CancellationToken cancellationToken)
            {
                foreach (var item in command.SingleQuestionSubmission)
                {
                    await _medaitor.Send(item);
                }
                return new Response<int>(1);
            }
        }

    }
}
