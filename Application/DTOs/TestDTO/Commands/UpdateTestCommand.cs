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
    public class UpdateTestCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; } 

        public class UpdateTestCommandHandler : IRequestHandler<UpdateTestCommand, Response<int>>
        {
            private readonly ITestRepositoryAsync _TestRepository;
            public UpdateTestCommandHandler(ITestRepositoryAsync TestRepository)
            {
                _TestRepository = TestRepository;
            }
            public async Task<Response<int>> Handle(UpdateTestCommand command, CancellationToken cancellationToken)
            {
                var Test = await _TestRepository.GetByIdAsync(command.Id);

                if (Test == null)
                {
                    throw new ApiException($"Test Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, Test);
                    await _TestRepository.UpdateAsync(Test);
                    return new Response<int>(Test.Id);
                }
            }
        }
    }
}
