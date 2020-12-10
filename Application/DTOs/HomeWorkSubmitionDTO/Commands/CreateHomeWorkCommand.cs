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
    public class CreateHomeWorkSubmitionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string StudentId { get; set; } 
        public string URL { get; set; }
        public int HomeworkId { get; set; } 
        public string Text { get; set; }

        public class CreateHomeWorkSubmitionCommandHandler : IRequestHandler<CreateHomeWorkSubmitionCommand, Response<int>>
        {
            private readonly IHomeWorkSubmitionRepositoryAsync _HomeWorkSubmitionRepository;
            public CreateHomeWorkSubmitionCommandHandler(IHomeWorkSubmitionRepositoryAsync HomeWorkSubmitionRepository)
            {
                _HomeWorkSubmitionRepository = HomeWorkSubmitionRepository;
            }
            public async Task<Response<int>> Handle(CreateHomeWorkSubmitionCommand command, CancellationToken cancellationToken)
            {
                var HomeWorkSubmition = new Domain.Entities.HomeWorkSubmition();

                Reflection.CopyProperties(command, HomeWorkSubmition);
                await _HomeWorkSubmitionRepository.AddAsync(HomeWorkSubmition);
                return new Response<int>(HomeWorkSubmition.Id);

            }
        }
    }
}
