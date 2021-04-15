using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;

namespace Application.Features.AdlerCardSubmission.Commands
{
    public class CorrectSubmissionCommand : IRequest<Response<int>>
    {
        public int AdlerCardsSubmissionsId { get; set; }
        public double AchievedScore { get; set; }
        public List<UpdateSingleQuestionSubmissionCommand> SingleQuestionSubmission { get; set; }
    }
    public class CorrectSubmissionCommandHandler : IRequestHandler<CorrectSubmissionCommand, Response<int>>
    {
        private readonly IAdlerCardSubmissionRepositoryAsync _adlercardsubmissionRepository;
        private readonly IMediator _medaitor;
        public CorrectSubmissionCommandHandler(IMediator mediator, IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository)
        {
            _adlercardsubmissionRepository = adlercardsubmissionRepository;
            _medaitor = mediator;
        }

        public async Task<Response<int>> Handle(CorrectSubmissionCommand request, CancellationToken cancellationToken)
        {
            var adlercardsubmission = _adlercardsubmissionRepository.GetByIdAsync(request.AdlerCardsSubmissionsId).Result;
            if (adlercardsubmission == null)
            {
                throw new ApiException("No Adler Card Submission Found");
            }
            await _medaitor.Send(request.SingleQuestionSubmission);
            adlercardsubmission.AchievedScore = request.AchievedScore;
            adlercardsubmission.AchievedScore = (int)AdlerCardSubmissionEnum.Corrected;
            await _adlercardsubmissionRepository.UpdateAsync(adlercardsubmission);
            return new Response<int>(adlercardsubmission.Id);
        }
    }
}
