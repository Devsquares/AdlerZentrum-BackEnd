using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupConditionDetails.Commands.UpdateGroupConditionDetails
{
	public class UpdateGroupConditionDetailsCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public int GroupConditionId { get; set; }

        public class UpdateGroupConditionDetailsCommandHandler : IRequestHandler<UpdateGroupConditionDetailsCommand, Response<int>>
        {
            private readonly IGroupConditionDetailsRepositoryAsync _groupconditiondetailsRepository;
            public UpdateGroupConditionDetailsCommandHandler(IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository)
            {
                _groupconditiondetailsRepository = groupconditiondetailsRepository;
            }
            public async Task<Response<int>> Handle(UpdateGroupConditionDetailsCommand command, CancellationToken cancellationToken)
            {
                var groupconditiondetails = await _groupconditiondetailsRepository.GetByIdAsync(command.Id);

                if (groupconditiondetails == null)
                {
                    throw new ApiException($"GroupConditionDetails Not Found.");
                }
                else
                {
				groupconditiondetails.GroupConditionId = command.GroupConditionId;

                    await _groupconditiondetailsRepository.UpdateAsync(groupconditiondetails);
                    return new Response<int>(groupconditiondetails.Id);
                }
            }
        }

    }
}
