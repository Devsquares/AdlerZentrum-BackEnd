using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateHomeworkBounsCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int BonusPoints { get; set; }
        public int BonusPointsStatus { get; set; }

        public class UpdateHomeworkBounsCommandHandler : IRequestHandler<UpdateHomeworkBounsCommand, Response<int>>
        {
            private readonly IHomeWorkRepositoryAsync _HomeWorkRepository;
            public UpdateHomeworkBounsCommandHandler(IHomeWorkRepositoryAsync HomeWorkRepository)
            {
                _HomeWorkRepository = HomeWorkRepository;
            }
            public async Task<Response<int>> Handle(UpdateHomeworkBounsCommand command, CancellationToken cancellationToken)
            {
                var homeWork = _HomeWorkRepository.GetByIdAsync(command.Id).Result;
                homeWork.BonusPoints = command.BonusPoints;
                homeWork.BonusPointsStatus = command.BonusPointsStatus;
                await _HomeWorkRepository.UpdateAsync(homeWork);
                return new Response<int>(homeWork.Id);

            }
        }
    }
}
