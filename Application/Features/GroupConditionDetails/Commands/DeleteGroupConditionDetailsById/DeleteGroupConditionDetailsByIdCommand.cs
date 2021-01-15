using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupConditionDetails.Commands.DeleteGroupConditionDetailsById
{
    public class DeleteGroupConditionDetailsByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteGroupConditionDetailsByIdCommandHandler : IRequestHandler<DeleteGroupConditionDetailsByIdCommand, Response<int>>
        {
            private readonly IGroupConditionDetailsRepositoryAsync _groupconditiondetailsRepository;
            public DeleteGroupConditionDetailsByIdCommandHandler(IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository)
            {
                _groupconditiondetailsRepository = groupconditiondetailsRepository;
            }
            public async Task<Response<int>> Handle(DeleteGroupConditionDetailsByIdCommand command, CancellationToken cancellationToken)
            {
                var groupconditiondetails = await _groupconditiondetailsRepository.GetByIdAsync(command.Id);
                if (groupconditiondetails == null) throw new ApiException($"GroupConditionDetails Not Found.");
                await _groupconditiondetailsRepository.DeleteAsync(groupconditiondetails);
                return new Response<int>(groupconditiondetails.Id);
            }
        }
    }
}
