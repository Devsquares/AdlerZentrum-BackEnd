using Application.Enums;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateTestCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public int TestDuration { get; set; }
        public int TestTypeId { get; set; }
        public List<int> SingleQuestions { get; set; }
        public int LessonDefinitionId { get; set; }

        public class CreateTestCommandHandler : IRequestHandler<CreateTestCommand, Response<int>>
        {
            private readonly ITestRepositoryAsync _TestRepository;
            private readonly ISingleQuestionRepositoryAsync _singleQuestionRepository;
            private readonly IQuestionRepositoryAsync _questionRepository;

            public CreateTestCommandHandler(
                ISingleQuestionRepositoryAsync singleQuestionRepository,
                IQuestionRepositoryAsync questionRepository,
                ITestRepositoryAsync TestRepository)
            {
                _TestRepository = TestRepository;
                _singleQuestionRepository = singleQuestionRepository;
                _questionRepository = questionRepository;
            }
            public async Task<Response<int>> Handle(CreateTestCommand command, CancellationToken cancellationToken)
            {
                var test = new Test();
                test.Name = command.Name;
                test.TestDuration = command.TestDuration;
                test.TestTypeId = command.TestTypeId;
                test.LessonDefinitionId = command.LessonDefinitionId;

                test = await _TestRepository.AddAsync(test);
                var singleQuestions = await _singleQuestionRepository.GetAllByIdAsync(command.SingleQuestions);

                List<SingleQuestion> MultipleChoice = new List<SingleQuestion>();
                List<SingleQuestion> TrueAndfalse = new List<SingleQuestion>();
                List<SingleQuestion> SingleChoice = new List<SingleQuestion>();
                List<SingleQuestion> Complete = new List<SingleQuestion>();

                foreach (var item in singleQuestions)
                {
                    if (item.SingleQuestionType == (int)SingleQuestionTypeEnum.Complete)
                    {
                        Complete.Add(item);
                    }
                    if (item.SingleQuestionType == (int)SingleQuestionTypeEnum.SingleChoice)
                    {
                        SingleChoice.Add(item);
                    }
                    if (item.SingleQuestionType == (int)SingleQuestionTypeEnum.TrueAndfalse)
                    {
                        TrueAndfalse.Add(item);
                    }
                    if (item.SingleQuestionType == (int)SingleQuestionTypeEnum.MultipleChoice)
                    {
                        MultipleChoice.Add(item);
                    }
                }
                int Order = 0;
                Question question = new Question();
                if (MultipleChoice.Count > 0)
                {
                    question.Header = "Multiple Choice";
                    Order++;
                    question.Order = Order;
                    question.TestId = test.Id;
                    question = await _questionRepository.AddAsync(question);
                    updateChilds(question, MultipleChoice);
                    question = new Question();
                }

                if (SingleChoice.Count > 0)
                {
                    question.Header = "Single Choice";
                    Order++;
                    question.Order = Order;
                    question.TestId = test.Id;
                    question = await _questionRepository.AddAsync(question);
                    updateChilds(question, SingleChoice);
                    question = new Question();
                }

                if (TrueAndfalse.Count > 0)
                {
                    question.Header = "True OR false";
                    Order++;
                    question.Order = Order;
                    question.TestId = test.Id;
                    question = await _questionRepository.AddAsync(question);
                    updateChilds(question, TrueAndfalse);
                    question = new Question();
                }

                if (Complete.Count > 0)
                {
                    question.Header = "Complete";
                    Order++;
                    question.Order = Order;
                    question.TestId = test.Id;
                    question = await _questionRepository.AddAsync(question);
                    updateChilds(question, Complete);
                }
                return new Response<int>(test.Id);
            }

            private void updateChilds(Question question, List<SingleQuestion> SingleQuestions)
            {
                foreach (var item in SingleQuestions)
                {
                    item.QuestionId = item.Id;
                }
                _singleQuestionRepository.UpdateBulkAsync(SingleQuestions);
            }
        }
    }
}
