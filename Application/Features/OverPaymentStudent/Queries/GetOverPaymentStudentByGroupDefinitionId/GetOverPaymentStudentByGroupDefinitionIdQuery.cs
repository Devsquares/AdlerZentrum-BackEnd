using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.OverPaymentStudent.Queries.GetOverPaymentStudentByGroupDefinitionId
{
    public class GetOverPaymentStudentByGroupDefinitionIdQuery : IRequest<Response<object>>
    {
        public int GroupDefinitionId { get; set; }
        public class GetOverPaymentStudentByGroupDefinitionIdQueryHandler : IRequestHandler<GetOverPaymentStudentByGroupDefinitionIdQuery, Response<object>>
        {
            private readonly IOverPaymentStudentRepositoryAsync _overPaymentsstudentRepository;
            public GetOverPaymentStudentByGroupDefinitionIdQueryHandler(IOverPaymentStudentRepositoryAsync overPaymentsstudentRepository)
            {
                _overPaymentsstudentRepository = overPaymentsstudentRepository;
            }
            public async Task<Response<object>> Handle(GetOverPaymentStudentByGroupDefinitionIdQuery command, CancellationToken cancellationToken)
            {
                var overPaymentstudent = _overPaymentsstudentRepository.GetByGroupDefinitionId(command.GroupDefinitionId);
                return new Response<object>(overPaymentstudent);
            }
        }
    }
}
