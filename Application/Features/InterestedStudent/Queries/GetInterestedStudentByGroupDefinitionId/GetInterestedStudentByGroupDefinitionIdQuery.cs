using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InterestedStudent.Queries.GetInterestedStudentByGroupDefinitionId
{
    public class GetInterestedStudentByGroupDefinitionIdQuery : IRequest<Response<object>>
    {
        public int GroupDefinitionId { get; set; }
        public class GetInterestedStudentByGroupDefinitionIdQueryHandler : IRequestHandler<GetInterestedStudentByGroupDefinitionIdQuery, Response<object>>
        {
            private readonly IInterestedStudentRepositoryAsync _interestedstudentRepository;
            public GetInterestedStudentByGroupDefinitionIdQueryHandler(IInterestedStudentRepositoryAsync interestedstudentRepository)
            {
                _interestedstudentRepository = interestedstudentRepository;
            }
            public async Task<Response<object>> Handle(GetInterestedStudentByGroupDefinitionIdQuery command, CancellationToken cancellationToken)
            {
                var interestedstudent = _interestedstudentRepository.GetByGroupDefinitionId(command.GroupDefinitionId);
                return new Response<object>(interestedstudent);
            }
        }
    }
}
