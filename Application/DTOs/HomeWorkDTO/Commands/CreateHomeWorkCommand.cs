using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.HomeWork.Commands
{
    public class CreateHomeWorkCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int MinCharacters { get; set; }
        public int Points { get; set; }
        public int BonusPoints { get; set; }
        public int Status { get; private set; }
        public int GroupInstanceId { get; set; }

        public class CreateHomeWorkCommandHandler : IRequestHandler<CreateHomeWorkCommand, Response<int>>
        {
            private readonly IHomeWorkRepositoryAsync _HomeWorkRepository;
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IMediator _mediator;
            public CreateHomeWorkCommandHandler(IHomeWorkRepositoryAsync HomeWorkRepository, IMediator Mediator, IGroupInstanceRepositoryAsync groupInstanceRepositoryAsync)
            {
                _HomeWorkRepository = HomeWorkRepository;
                _groupInstanceRepositoryAsync = groupInstanceRepositoryAsync;
                _mediator = Mediator;
            }
            public async Task<Response<int>> Handle(CreateHomeWorkCommand command, CancellationToken cancellationToken)
            {
                var HomeWork = new Homework();

                Reflection.CopyProperties(command, HomeWork);
                HomeWork.Status = (int)AdditionalHomeworkStatusEnum.New;

                // Create HomeWorkSubmition for each student. HomeWorkSubmitionStatusEnum.Pending if without bouns. 
                await _HomeWorkRepository.AddAsync(HomeWork);
                var students = _groupInstanceRepositoryAsync.GetStudents(HomeWork.GroupInstanceId);

                await _mediator.Publish(new CreateHomeWorkSubmitionCommand { HomeWorkId = HomeWork.Id, Students = students });
                return new Response<int>(HomeWork.Id);

            }
        }
        public class CreateHomeWorkSubmitionCommand : IRequest<Response<bool>>
        {
            public int HomeWorkId { get; set; }
            public IReadOnlyList<GroupInstanceStudents> Students { get; set; }
            public class CreateHomeWorkSubmitionCommandHandler : IRequestHandler<CreateHomeWorkSubmitionCommand, Response<bool>>
            {
                private readonly IHomeWorkSubmitionRepositoryAsync _HomeWorkSubmitionRepository;
                public CreateHomeWorkSubmitionCommandHandler(IHomeWorkSubmitionRepositoryAsync HomeWorkRepository)
                {
                    _HomeWorkSubmitionRepository = HomeWorkRepository;
                }
                public async Task<Response<bool>> Handle(CreateHomeWorkSubmitionCommand command, CancellationToken cancellationToken)
                {
                    var homeWork = new HomeWorkSubmition();
                    foreach (var item in command.Students)
                    {
                        homeWork = new HomeWorkSubmition();
                        homeWork.Status = (int)HomeWorkSubmitionStatusEnum.Pending;
                        homeWork.HomeworkId = command.HomeWorkId;
                        homeWork.StudentId = item.StudentId;
                        await _HomeWorkSubmitionRepository.AddAsync(homeWork);
                    }
                    return new Response<bool>(true);
                }
            }
        }

    }
}
