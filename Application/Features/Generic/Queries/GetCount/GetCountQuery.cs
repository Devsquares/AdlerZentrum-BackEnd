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

namespace Application.Features.Branch.Queries.GetBranchById
{
    public class GetCountQuery : IRequest<Response<int>>
    {
        public string Table { get; set; }
        public class GetCountQueryHandler : IRequestHandler<GetCountQuery, Response<int>>
        {
            private readonly IGenericTableRepositoryAsync _genericRepository;
            public GetCountQueryHandler(IGenericTableRepositoryAsync genericRepository)
            {
                _genericRepository = genericRepository;
            }
            public async Task<Response<int>> Handle(GetCountQuery query, CancellationToken cancellationToken)
            {
                var count = await _genericRepository.GetCount(query.Table);
                return new Response<int>(count);
            }
        }

    }
}
