using Application.Interfaces.Repositories;
using Application.Wrappers;
using Application.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTOs
{
    public class UpdateHomeworkBounsCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public double BonusPoints { get; set; }
        public int BonusPointsStatus { get; set; }

        public class UpdateHomeworkBounsCommandHandler : IRequestHandler<UpdateHomeworkBounsCommand, Response<int>>
        {
            private readonly IHomeWorkRepositoryAsync _HomeWorkRepository;
            private readonly IJobRepositoryAsync _jobRepository;

            public UpdateHomeworkBounsCommandHandler(IHomeWorkRepositoryAsync HomeWorkRepository, IJobRepositoryAsync jobRepository)
            {
                _HomeWorkRepository = HomeWorkRepository;
                _jobRepository = jobRepository;
            }
            public async Task<Response<int>> Handle(UpdateHomeworkBounsCommand command, CancellationToken cancellationToken)
            {
                var homeWork = _HomeWorkRepository.GetByIdAsync(command.Id).Result;
                homeWork.BonusPoints = command.BonusPoints;
                homeWork.BonusPointsStatus = command.BonusPointsStatus;
                await _HomeWorkRepository.UpdateAsync(homeWork);
                if (homeWork.BonusPointsStatus == (int)BonusPointsStatusEnum.Approved)
                {
                    await _jobRepository.AddAsync(new Job
                    {
                        Type = (int)JobTypeEnum.ScoreCalculatorForGroup,
                        GroupInstanceId = homeWork.GroupInstanceId,
                        Status = (int)JobStatusEnum.New
                    });
                }
                return new Response<int>(homeWork.Id);

            }
        }
    }
}
