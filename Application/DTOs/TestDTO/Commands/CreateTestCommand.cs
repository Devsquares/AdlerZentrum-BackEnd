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
    public class CreateTestCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; } 

        public class CreateTestCommandHandler : IRequestHandler<CreateTestCommand, Response<int>>
        {
            private readonly ITestRepositoryAsync _TestRepository;
            public CreateTestCommandHandler(ITestRepositoryAsync TestRepository)
            {
                _TestRepository = TestRepository;
            }
            public async Task<Response<int>> Handle(CreateTestCommand command, CancellationToken cancellationToken)
            {
                var Test = new Domain.Entities.Test();

                Reflection.CopyProperties(command, Test);
                await _TestRepository.AddAsync(Test);
                return new Response<int>(Test.Id);

            }
        }
    }
}
