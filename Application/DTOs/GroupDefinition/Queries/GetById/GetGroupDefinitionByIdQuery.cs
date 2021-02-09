using Application.DTOs.GroupDefinition.Queries.GetById;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetGroupDefinitionByIdQuery : IRequest<Response<GetGroupDefinitionbyIdViewModel>>
    {
        public int Id { get; set; }
        public class GetGroupDefinitionByIdQueryHandler : IRequestHandler<GetGroupDefinitionByIdQuery, Response<GetGroupDefinitionbyIdViewModel>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupconditionpromocodeRepository;
            public GetGroupDefinitionByIdQueryHandler(IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository, 
                IGroupDefinitionRepositoryAsync GroupDefinitionRepository)
            {
                _GroupDefinitionRepository = GroupDefinitionRepository;
                _groupconditionpromocodeRepository = groupconditionpromocodeRepository;
            }
            public async Task<Response<GetGroupDefinitionbyIdViewModel>> Handle(GetGroupDefinitionByIdQuery query, CancellationToken cancellationToken)
            {
                var GroupDefinition = _GroupDefinitionRepository.GetById(query.Id);
                if (GroupDefinition == null) throw new ApiException($"Group Not Found.");
                var promos = _groupconditionpromocodeRepository.GetAllByGroupCondition(GroupDefinition.GroupConditionId);
                GetGroupDefinitionbyIdViewModel groupDefinitionModel = new GetGroupDefinitionbyIdViewModel();
                groupDefinitionModel.GroupDefinition = GroupDefinition;
                groupDefinitionModel.PromoCodes = promos;
                return new Response<GetGroupDefinitionbyIdViewModel>(groupDefinitionModel);
            }
        }
    }
}
