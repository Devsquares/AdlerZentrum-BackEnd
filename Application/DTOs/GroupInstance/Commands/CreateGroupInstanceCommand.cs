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

namespace Application.DTOs.GroupInstance.Commands
{
    public class CreateGroupInstanceCommand : IRequest<Response<int>>
    {
        public int GroupDefinitionId { get; set; }
        public int Serail { get; set; }
        public int Status { get; set; }

        public class CreateGroupInstanceCommandHandler : IRequestHandler<CreateGroupInstanceCommand, Response<int>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            public CreateGroupInstanceCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
            }
            public async Task<Response<int>> Handle(CreateGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = new Domain.Entities.GroupInstance();

                Reflection.CopyProperties(command, groupInstance);
                await _groupInstanceRepositoryAsync.AddAsync(groupInstance);
                return new Response<int>(groupInstance.Id);

            }
        }
    }
}
