using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupConditionPromoCode.Commands.DeleteGroupConditionPromoCodeById
{
    public class DeleteGroupConditionPromoCodeByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteGroupConditionPromoCodeByIdCommandHandler : IRequestHandler<DeleteGroupConditionPromoCodeByIdCommand, Response<int>>
        {
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupconditionpromocodeRepository;
            public DeleteGroupConditionPromoCodeByIdCommandHandler(IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository)
            {
                _groupconditionpromocodeRepository = groupconditionpromocodeRepository;
            }
            public async Task<Response<int>> Handle(DeleteGroupConditionPromoCodeByIdCommand command, CancellationToken cancellationToken)
            {
                var groupconditionpromocode = await _groupconditionpromocodeRepository.GetByIdAsync(command.Id);
                if (groupconditionpromocode == null) throw new ApiException($"GroupConditionPromoCode Not Found.");
                await _groupconditionpromocodeRepository.DeleteAsync(groupconditionpromocode);
                return new Response<int>(groupconditionpromocode.Id);
            }
        }
    }
}
