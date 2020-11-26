using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.Level.Queries
{
    public class GetGroupInstanceByIdQuery : IRequest<Response<Domain.Entities.GroupInstance>>
    {
        public int Id { get; set; }
        public class GetGroupInstanceByIdQueryHandler : IRequestHandler<GetGroupInstanceByIdQuery, Response<Domain.Entities.GroupInstance>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepository;
            public GetGroupInstanceByIdQueryHandler(IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _groupInstanceRepository = groupInstanceRepository;
            }
            public async Task<Response<Domain.Entities.GroupInstance>> Handle(GetGroupInstanceByIdQuery query, CancellationToken cancellationToken)
            {
                var groupInstance = await _groupInstanceRepository.GetByIdAsync(query.Id);
                if (groupInstance == null) throw new ApiException($"Group Not Found.");
                return new Response<Domain.Entities.GroupInstance>(groupInstance);
            }
        }
    }
}
