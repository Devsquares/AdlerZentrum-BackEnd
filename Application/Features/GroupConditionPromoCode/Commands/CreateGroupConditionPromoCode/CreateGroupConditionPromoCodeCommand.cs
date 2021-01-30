using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CreateGroupConditionPromoCodeCommand : IRequest<Response<int>>
    {
		public int GroupConditionDetailsId { get; set; }
		public int PromoCodeId { get; set; }
		public int Count { get; set; }
    }

    public class CreateGroupConditionPromoCodeCommandHandler : IRequestHandler<CreateGroupConditionPromoCodeCommand, Response<int>>
    {
        private readonly IGroupConditionPromoCodeRepositoryAsync _groupconditionpromocodeRepository;
        private readonly IMapper _mapper;
        public CreateGroupConditionPromoCodeCommandHandler(IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository, IMapper mapper)
        {
            _groupconditionpromocodeRepository = groupconditionpromocodeRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateGroupConditionPromoCodeCommand request, CancellationToken cancellationToken)
        {
            var groupconditionpromocode = _mapper.Map<Domain.Entities.GroupConditionPromoCode>(request);
            await _groupconditionpromocodeRepository.AddAsync(groupconditionpromocode);
            return new Response<int>(groupconditionpromocode.Id);
        }
    }
}
