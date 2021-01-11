using Application.Enums;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class TestInstanceSolutionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public List<QuestionSubmissionInput> Questions { get; set; }
        public string StudentId { get; set; }
    }
    public class QuestionSubmissionInput
    {
        public int SingleQuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool TrueOrFalseSubmission { get; set; }
        public ICollection<ChoiceSubmissionInputModel> Choices { get; set; }

    }
    public class ChoiceSubmissionInputModel
    {
        public int ChoiceId { get; set; }
    }

    public class CreateTestInstanceCommandHandler : IRequestHandler<TestInstanceSolutionCommand, Response<int>>
    {
        private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
        private readonly ISingleQuestionSubmissionRepositoryAsync _singleQuestionSubmission;
        private readonly IChoiceSubmissionRepositoryAsync _choiceSubmissionRepository;
        private readonly IMapper _mapper;
        public CreateTestInstanceCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository,
        ISingleQuestionSubmissionRepositoryAsync singleQuestionSubmissionRepositoryAsync,
        IChoiceSubmissionRepositoryAsync choiceSubmissionRepositoryAsync,
         IMapper mapper)
        {
            _testinstanceRepository = testinstanceRepository;
            _singleQuestionSubmission = singleQuestionSubmissionRepositoryAsync;
            _choiceSubmissionRepository = choiceSubmissionRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(TestInstanceSolutionCommand request, CancellationToken cancellationToken)
        {
            var testInstance = _testinstanceRepository.GetByIdAsync(request.Id).Result;
            testInstance.Status = (int)TestInstanceEnum.Solved;
            testInstance.SubmissionDate = DateTime.Now;
            foreach (var item in request.Questions)
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
                        await _choiceSubmissionRepository.AddAsync(choiceSubmission);
                    }
                }
            }
            await _testinstanceRepository.UpdateAsync(testInstance);

            return new Response<int>(testInstance.Id);
        }
    }
}
