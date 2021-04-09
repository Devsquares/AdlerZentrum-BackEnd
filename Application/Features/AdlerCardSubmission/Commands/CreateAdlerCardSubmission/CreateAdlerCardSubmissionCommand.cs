using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CreateAdlerCardSubmissionCommand : IRequest<Response<int>>
    {
        public int AdlerCardId { get; set; }
        public string StudentId { get; set; }
        public List<SingleQuestionSubmissionInput> SingleQuestions { get; set; }
    }


    public class CreateAdlerCardSubmissionCommandHandler : IRequestHandler<CreateAdlerCardSubmissionCommand, Response<int>>
    {
        private readonly IAdlerCardSubmissionRepositoryAsync _adlercardsubmissionRepository;
        private readonly IAdlerCardRepositoryAsync _adlerCardRepositoryAsync;
        private readonly ISingleQuestionSubmissionRepositoryAsync _singleQuestionSubmission;
        private readonly IChoiceSubmissionRepositoryAsync _choiceSubmissionRepository;
        private readonly IMapper _mapper;
        public CreateAdlerCardSubmissionCommandHandler(IMapper mapper,
        IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository,
        IAdlerCardRepositoryAsync adlerCardRepositoryAsync,
        ISingleQuestionSubmissionRepositoryAsync singleQuestionSubmissionRepositoryAsync,
        IChoiceSubmissionRepositoryAsync choiceSubmissionRepositoryAsync)
        {
            _adlercardsubmissionRepository = adlercardsubmissionRepository;
            _mapper = mapper;
            _adlerCardRepositoryAsync = adlerCardRepositoryAsync;
            _singleQuestionSubmission = singleQuestionSubmissionRepositoryAsync;
            _choiceSubmissionRepository = choiceSubmissionRepositoryAsync;
        }

        public async Task<Response<int>> Handle(CreateAdlerCardSubmissionCommand request, CancellationToken cancellationToken)
        {
            var adlercard = _adlerCardRepositoryAsync.GetByIdAsync(request.AdlerCardId).Result;
            if (adlercard == null)
            {
                throw new ApiException("No AdlerCard Found");
            }
            foreach (var item in request.SingleQuestions)
            {
                SingleQuestionSubmission singleQuestionSubmission = new SingleQuestionSubmission();
                singleQuestionSubmission.AnswerText = item.AnswerText;
                singleQuestionSubmission.SingleQuestionId = item.SingleQuestionId;
                singleQuestionSubmission.TrueOrFalseSubmission = item.TrueOrFalseSubmission;
                singleQuestionSubmission.StudentId = request.StudentId;
                singleQuestionSubmission.Corrected = false;

                var singleQuestionSubmissionId = _singleQuestionSubmission.AddAsync(singleQuestionSubmission).Result.Id;
                if (item.Choices != null)
                {
                    foreach (var choice in item.Choices)
                    {
                        ChoiceSubmission choiceSubmission = new ChoiceSubmission();
                        choiceSubmission.SingleQuestionSubmissionId = singleQuestionSubmissionId;
                        choiceSubmission.ChoiceSubmissionId = choice.ChoiceId;
                        await _choiceSubmissionRepository.AddAsync(choiceSubmission);
                    }
                }
            }

            var adlercardsubmission = _mapper.Map<Domain.Entities.AdlerCardSubmission>(request);
            adlercardsubmission.Status = (int)AdlerCardSubmissionEnum.Solved;
            await _adlercardsubmissionRepository.AddAsync(adlercardsubmission);
            return new Response<int>(adlercardsubmission.Id);
        }
    }
}
