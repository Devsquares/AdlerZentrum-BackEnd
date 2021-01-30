using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
	public class UpdateGroupConditionPromoCodeCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public int GroupConditionDetailsId { get; set; }
		public int PromoCodeId { get; set; }
		public int Count { get; set; }

        public class UpdateGroupConditionPromoCodeCommandHandler : IRequestHandler<UpdateGroupConditionPromoCodeCommand, Response<int>>
        {
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupconditionpromocodeRepository;
            public UpdateGroupConditionPromoCodeCommandHandler(IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository)
            {
                _groupconditionpromocodeRepository = groupconditionpromocodeRepository;
            }
            public async Task<Response<int>> Handle(UpdateGroupConditionPromoCodeCommand command, CancellationToken cancellationToken)
            {
                var groupconditionpromocode = await _groupconditionpromocodeRepository.GetByIdAsync(command.Id);

                if (groupconditionpromocode == null)
                {
                    throw new ApiException($"GroupConditionPromoCode Not Found.");
                }
                else
                {
				groupconditionpromocode.GroupConditionDetailsId = command.GroupConditionDetailsId;
				groupconditionpromocode.PromoCodeId = command.PromoCodeId;
				groupconditionpromocode.Count = command.Count; 

                    await _groupconditionpromocodeRepository.UpdateAsync(groupconditionpromocode);
                    return new Response<int>(groupconditionpromocode.Id);
                }
            }
        }

    }
}
