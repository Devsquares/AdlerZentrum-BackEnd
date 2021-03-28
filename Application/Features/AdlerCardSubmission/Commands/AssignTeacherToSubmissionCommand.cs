using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AdlerCardSubmission.Commands
{
    public class AssignTeacherToSubmissionCommand : IRequest<Response<int>>
    {
        public int AdlerCardsSubmissionsId { get; set; }
        public string TeacherId { get; set; }
    }
    public class AssignTeacherToSubmissionCommandHandler : IRequestHandler<AssignTeacherToSubmissionCommand, Response<int>>
    {
        private readonly IAdlerCardSubmissionRepositoryAsync _adlercardsubmissionRepository;
        public AssignTeacherToSubmissionCommandHandler(IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository)
        {
            _adlercardsubmissionRepository = adlercardsubmissionRepository;
        }

        public async Task<Response<int>> Handle(AssignTeacherToSubmissionCommand request, CancellationToken cancellationToken)
        {
            var adlercardsubmission = _adlercardsubmissionRepository.GetByIdAsync(request.AdlerCardsSubmissionsId).Result;
            if (adlercardsubmission == null)
            {
                throw new ApiException("No AdlerCardSubmission Found");
            }
            adlercardsubmission.TeacherId = request.TeacherId;
              await _adlercardsubmissionRepository.UpdateAsync(adlercardsubmission);
            return new Response<int>(adlercardsubmission.Id);
        }
    }
}
