using Application.Enums;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class HomeworkCorrectionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Solution { get; set; }
        public string CorrectionTeacherId { get; set; }
        public int Points { get; set; }
        public string Comment { get; set; }

        public class HomeworkCorrectionCommandHandler : IRequestHandler<HomeworkCorrectionCommand, Response<int>>
        {
            private readonly IHomeWorkSubmitionRepositoryAsync _HomeWorkSubmitionRepository;
            private readonly IHomeWorkRepositoryAsync _homework;
            public HomeworkCorrectionCommandHandler(IHomeWorkSubmitionRepositoryAsync HomeWorkSubmitionRepository, IHomeWorkRepositoryAsync homeWorkRepositoryAsync)
            {
                _HomeWorkSubmitionRepository = HomeWorkSubmitionRepository;
                _homework = homeWorkRepositoryAsync;
            }
            public async Task<Response<int>> Handle(HomeworkCorrectionCommand command, CancellationToken cancellationToken)
            {
                var HomeWorkSubmition = _HomeWorkSubmitionRepository.GetByIdAsync(command.Id).Result;
                HomeWorkSubmition.CorrectionDate = DateTime.Now;
                HomeWorkSubmition.CorrectionTeacherId = command.CorrectionTeacherId;
                HomeWorkSubmition.Solution = command.Solution;
                HomeWorkSubmition.Points = command.Points;
                HomeWorkSubmition.Comment = command.Comment;
                HomeWorkSubmition.Status = (int)HomeWorkSubmitionStatusEnum.Corrected;
                
                var homework = _homework.GetByIdAsync(HomeWorkSubmition.HomeworkId).Result;
                int bouns = 0;
                if (homework.BonusPoints > 0 && homework.BonusPointsStatus == (int)BonusPointsStatusEnum.Approved)
                {
                    if (bouns < (command.Points - homework.BonusPoints))
                    {
                        bouns = command.Points - homework.BonusPoints;
                    }
                }
                HomeWorkSubmition.BonusPoints = bouns;

                await _HomeWorkSubmitionRepository.UpdateAsync(HomeWorkSubmition);
                return new Response<int>(HomeWorkSubmition.Id);
            }
        }
    }
}
