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

        public class HomeworkCorrectionCommandHandler : IRequestHandler<HomeworkCorrectionCommand, Response<int>>
        {
            private readonly IHomeWorkSubmitionRepositoryAsync _HomeWorkSubmitionRepository;
            public HomeworkCorrectionCommandHandler(IHomeWorkSubmitionRepositoryAsync HomeWorkSubmitionRepository)
            {
                _HomeWorkSubmitionRepository = HomeWorkSubmitionRepository;
            }
            public async Task<Response<int>> Handle(HomeworkCorrectionCommand command, CancellationToken cancellationToken)
            {
                var HomeWorkSubmition = _HomeWorkSubmitionRepository.GetByIdAsync(command.Id).Result;
                HomeWorkSubmition.CorrectionDate = DateTime.Now;
                HomeWorkSubmition.Solution = command.Solution;
                //TODO need to get from login user.
                HomeWorkSubmition.CorrectionTeacherId = command.CorrectionTeacherId;
                HomeWorkSubmition.Status = (int)HomeWorkSubmitionStatusEnum.Corrected;

                await _HomeWorkSubmitionRepository.AddAsync(HomeWorkSubmition);
                return new Response<int>(HomeWorkSubmition.Id);
            }
        }
    }
}
