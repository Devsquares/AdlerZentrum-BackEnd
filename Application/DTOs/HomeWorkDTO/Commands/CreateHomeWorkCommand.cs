﻿using Application.Enums;
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

namespace Application.DTOs
{
    public class CreateHomeWorkCommand : IRequest<Response<Homework>>
    {
        public string Text { get; set; }
        public int MinCharacters { get; set; }
        public double Points { get; set; }
        public double BonusPoints { get; set; }
        public int GroupInstanceId { get; set; }
        public int LessonInstanceId { get; set; }
        public string TeacherId { get; set; }

        public class CreateHomeWorkCommandHandler : IRequestHandler<CreateHomeWorkCommand, Response<Homework>>
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
            public async Task<Response<Homework>> Handle(CreateHomeWorkCommand command, CancellationToken cancellationToken)
            {
                var HomeWork = new Homework();

                Reflection.CopyProperties(command, HomeWork);
                HomeWork.BonusPointsStatus = (int)BonusPointsStatusEnum.New;

                // Create HomeWorkSubmition for each student. HomeWorkSubmitionStatusEnum.Pending if without bouns. 
                await _HomeWorkRepository.AddAsync(HomeWork);
                var students = _groupInstanceRepositoryAsync.GetStudents(HomeWork.GroupInstanceId);

                await _mediator.Send(new CreateHomeWorkSubmitionCommand { HomeWorkId = HomeWork.Id, Students = students });
                return new Response<Homework>(HomeWork);

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
                        if (!item.Disqualified)
                        {
                            homeWork = new HomeWorkSubmition();
                            homeWork.Status = (int)HomeWorkSubmitionStatusEnum.Pending;
                            homeWork.HomeworkId = command.HomeWorkId;
                            homeWork.StudentId = item.StudentId;
                            // TODO: check with business.
                            homeWork.DueDate = DateTime.Now.AddDays(2);
                            await _HomeWorkSubmitionRepository.AddAsync(homeWork);
                        }

                    }
                    return new Response<bool>(true);
                }
            }
        }
    }
}