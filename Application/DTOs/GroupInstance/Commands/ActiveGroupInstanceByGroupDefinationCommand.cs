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
    public class ActiveGroupInstanceByGroupDefinationCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public class ActiveGroupInstanceByGroupDefinationCommandHandler : IRequestHandler<ActiveGroupInstanceByGroupDefinationCommand, Response<bool>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IMediator _mediator;
            public ActiveGroupInstanceByGroupDefinationCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
            IMediator mediator)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _mediator = mediator;
            }
            public async Task<Response<bool>> Handle(ActiveGroupInstanceByGroupDefinationCommand command, CancellationToken cancellationToken)
            {
                var groupInstances = _groupInstanceRepositoryAsync.GetByGroupDefinitionAndGroupInstance(command.Id);
                if (groupInstances == null) throw new ApiException($"Group Not Found.");
                foreach (var item in groupInstances)
                {
                    await _mediator.Send(new ActiveGroupInstanceCommand { GroupInstanceId = item.Id });
                }
                return new Response<bool>(true);
            }
        }
    }
}
