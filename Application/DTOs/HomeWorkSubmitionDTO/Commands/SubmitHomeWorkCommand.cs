using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SubmitHomeWorkCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Text { get; set; }

        public class SubmitHomeWorkCommandHandler : IRequestHandler<SubmitHomeWorkCommand, Response<int>>
        {
            private readonly IHomeWorkSubmitionRepositoryAsync _HomeWorkSubmitionRepository;
            private readonly IMailJobRepositoryAsync _mailJobRepository;
            public SubmitHomeWorkCommandHandler(IHomeWorkSubmitionRepositoryAsync HomeWorkSubmitionRepository,
            IMailJobRepositoryAsync mailJobRepositoryAsync)
            {
                _HomeWorkSubmitionRepository = HomeWorkSubmitionRepository;
                _mailJobRepository = mailJobRepositoryAsync;
            }
            public async Task<Response<int>> Handle(SubmitHomeWorkCommand command, CancellationToken cancellationToken)
            {
                if (command.Status == (int)HomeWorkSubmitionStatusEnum.Draft || command.Status == (int)HomeWorkSubmitionStatusEnum.Solved)
                {
                    var homeWorkSubmition = new HomeWorkSubmition();
                    homeWorkSubmition = _HomeWorkSubmitionRepository.GetByIdAsync(command.Id).Result;
                    homeWorkSubmition.Text = command.Text;
                    homeWorkSubmition.Status = command.Status;
                    homeWorkSubmition.SubmitionDate = DateTime.Now;
                    homeWorkSubmition.CorrectionDueDate = DateTime.Now.AddDays(1);

                    await _HomeWorkSubmitionRepository.UpdateAsync(homeWorkSubmition);
                    await _mailJobRepository.AddAsync(new MailJob
                    {
                        Type = (int)MailJobTypeEnum.HomeworkSubmitted,
                        StudentId = homeWorkSubmition.StudentId,
                        TeacherId = homeWorkSubmition.Homework.TeacherId,
                        HomeworkId = homeWorkSubmition.Id,
                        Status = (int)JobStatusEnum.New
                    });
                    return new Response<int>(homeWorkSubmition.Id);
                }
                else
                {
                    throw new ApiException($"Wrong Status.");
                }


            }
        }
    }
}
