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
using System.Transactions;

namespace Application.DTOs
{
    public class ActiveGroupInstanceByGroupDefinationCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public class ActiveGroupInstanceByGroupDefinationCommandHandler : IRequestHandler<ActiveGroupInstanceByGroupDefinationCommand, Response<bool>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupDefinitionRepositoryAsync _groupDefinitionRepositoryAsync;
            private readonly IMediator _mediator;
            public ActiveGroupInstanceByGroupDefinationCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
            IMediator mediator,
            IGroupDefinitionRepositoryAsync groupDefinitionRepositoryAsync)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _groupDefinitionRepositoryAsync = groupDefinitionRepositoryAsync;
                _mediator = mediator;
            }
            public async Task<Response<bool>> Handle(ActiveGroupInstanceByGroupDefinationCommand command, CancellationToken cancellationToken)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var groupInstances = _groupInstanceRepositoryAsync.GetByGroupDefinitionAndGroupInstance(command.Id);
                    if (groupInstances == null) throw new ApiException($"Group Not Found.");
                    foreach (var item in groupInstances)
                    {
                        await _mediator.Send(new ActiveGroupInstanceCommand { GroupInstanceId = item.Id });
                    }
                    var groupDefinition = await _groupDefinitionRepositoryAsync.GetByIdAsync(command.Id);
                    groupDefinition.Status = (int)GroupDefinationStatusEnum.Running;
                    await _groupDefinitionRepositoryAsync.UpdateAsync(groupDefinition);
                    scope.Complete();
                }
                return new Response<bool>(true);
            }
        }
    }
}
