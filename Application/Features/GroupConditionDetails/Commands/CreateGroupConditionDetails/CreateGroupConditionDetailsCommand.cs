using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupConditionDetails.Commands.CreateGroupConditionDetails
{
    public partial class CreateGroupConditionDetailsCommand : IRequest<Response<int>>
    {
		public int GroupConditionId { get; set; }
    }

    public class CreateGroupConditionDetailsCommandHandler : IRequestHandler<CreateGroupConditionDetailsCommand, Response<int>>
    {
        private readonly IGroupConditionDetailsRepositoryAsync _groupconditiondetailsRepository;
        private readonly IMapper _mapper;
        public CreateGroupConditionDetailsCommandHandler(IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository, IMapper mapper)
        {
            _groupconditiondetailsRepository = groupconditiondetailsRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateGroupConditionDetailsCommand request, CancellationToken cancellationToken)
        {
            var groupconditiondetails = _mapper.Map<Domain.Entities.GroupConditionDetail>(request);
            await _groupconditiondetailsRepository.AddAsync(groupconditiondetails);
            return new Response<int>(groupconditiondetails.Id);
        }
    }
}
