using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.GroupInstance.Queries.GetById
{
    public class GetGroupInstanceByGroupDefinitionIdQuery : IRequest<Response<List<StudentsGroupInstanceModel>>>
    {
        public int GroupDefinitionId { get; set; }
        public class GetGroupInstanceByGroupDefinitionIdQueryHandler : IRequestHandler<GetGroupInstanceByGroupDefinitionIdQuery, Response<List<StudentsGroupInstanceModel>>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepository;
            public GetGroupInstanceByGroupDefinitionIdQueryHandler(IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _groupInstanceRepository = groupInstanceRepository;
            }
            public async Task<Response<List<StudentsGroupInstanceModel>>> Handle(GetGroupInstanceByGroupDefinitionIdQuery command, CancellationToken cancellationToken)
            {
                var groupInstance =  _groupInstanceRepository.GetListByGroupDefinitionId(command.GroupDefinitionId);
                return new Response<List<StudentsGroupInstanceModel>>(groupInstance);
            }
        }
    }
}
