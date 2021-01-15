using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DeleteGroupConditionByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteGroupConditionByIdCommandHandler : IRequestHandler<DeleteGroupConditionByIdCommand, Response<int>>
        {
            private readonly IGroupConditionRepositoryAsync _groupConditionRepository;
            private readonly IGroupConditionDetailsRepositoryAsync _groupConditionDetailsRepository;
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepository;
            public DeleteGroupConditionByIdCommandHandler(IGroupConditionRepositoryAsync groupCondition,
                 IGroupConditionDetailsRepositoryAsync groupConditionDetailsRepository,
                IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepository)
            {
                _groupConditionRepository = groupCondition;
                _groupConditionDetailsRepository = groupConditionDetailsRepository;
                _groupConditionPromoCodeRepository = groupConditionPromoCodeRepository;
            }
            public async Task<Response<int>> Handle(DeleteGroupConditionByIdCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = await _groupConditionRepository.GetByIdAsync(command.Id);
                if (groupInstance == null) throw new ApiException($"Group Not Found.");
                //delete group condition promocodes and details
                var deletedGroupConditionDetail = _groupConditionDetailsRepository.GetByGroupConditionId(command.Id);
                var deletedPromocodes = _groupConditionPromoCodeRepository.GetByGroupConditionDetailId(deletedGroupConditionDetail);
                await _groupConditionPromoCodeRepository.DeleteBulkAsync(deletedPromocodes);
                await _groupConditionDetailsRepository.DeleteBulkAsync(deletedGroupConditionDetail);
                await _groupConditionRepository.DeleteAsync(groupInstance);

                return new Response<int>(groupInstance.Id);
            }
        }
    }
}
