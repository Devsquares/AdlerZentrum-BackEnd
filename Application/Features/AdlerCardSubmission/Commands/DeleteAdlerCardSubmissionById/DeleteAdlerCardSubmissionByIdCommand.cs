using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class DeleteAdlerCardSubmissionByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteAdlerCardSubmissionByIdCommandHandler : IRequestHandler<DeleteAdlerCardSubmissionByIdCommand, Response<int>>
        {
            private readonly IAdlerCardSubmissionRepositoryAsync _adlercardsubmissionRepository;
            public DeleteAdlerCardSubmissionByIdCommandHandler(IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository)
            {
                _adlercardsubmissionRepository = adlercardsubmissionRepository;
            }
            public async Task<Response<int>> Handle(DeleteAdlerCardSubmissionByIdCommand command, CancellationToken cancellationToken)
            {
                var adlercardsubmission = await _adlercardsubmissionRepository.GetByIdAsync(command.Id);
                if (adlercardsubmission == null) throw new ApiException($"AdlerCardSubmission Not Found.");
                await _adlercardsubmissionRepository.DeleteAsync(adlercardsubmission);
                return new Response<int>(adlercardsubmission.Id);
            }
        }
    }
}
