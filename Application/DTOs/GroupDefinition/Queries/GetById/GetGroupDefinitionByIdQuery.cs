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
    public class GetGroupDefinitionByIdQuery : IRequest<Response<Domain.Entities.GroupDefinition>>
    {
        public int Id { get; set; }
        public class GetGroupDefinitionByIdQueryHandler : IRequestHandler<GetGroupDefinitionByIdQuery, Response<Domain.Entities.GroupDefinition>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            public GetGroupDefinitionByIdQueryHandler(IGroupDefinitionRepositoryAsync GroupDefinitionRepository)
            {
                _GroupDefinitionRepository = GroupDefinitionRepository;
            }
            public async Task<Response<Domain.Entities.GroupDefinition>> Handle(GetGroupDefinitionByIdQuery query, CancellationToken cancellationToken)
            {
                var GroupDefinition = _GroupDefinitionRepository.GetById(query.Id);
                if (GroupDefinition == null) throw new ApiException($"Group Not Found.");
                return new Response<Domain.Entities.GroupDefinition>(GroupDefinition);
            }
        }
    }
}
