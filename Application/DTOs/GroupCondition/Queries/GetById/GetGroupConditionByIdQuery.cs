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
    public class GetGroupConditionByIdQuery : IRequest<Response<Domain.Entities.GroupCondition>>
    {
        public int Id { get; set; }
        public class GetGroupConditionByIdQueryHandler : IRequestHandler<GetGroupConditionByIdQuery, Response<Domain.Entities.GroupCondition>>
        {
            private readonly IGroupConditionRepositoryAsync _GroupConditionRepository;
            public GetGroupConditionByIdQueryHandler(IGroupConditionRepositoryAsync GroupConditionRepository)
            {
                _GroupConditionRepository = GroupConditionRepository;
            }
            public async Task<Response<Domain.Entities.GroupCondition>> Handle(GetGroupConditionByIdQuery query, CancellationToken cancellationToken)
            {
                var GroupCondition = await _GroupConditionRepository.GetByIdAsync(query.Id);
                if (GroupCondition == null) throw new ApiException($"Group Not Found.");
                return new Response<Domain.Entities.GroupCondition>(GroupCondition);
            }
        }
    }
}
