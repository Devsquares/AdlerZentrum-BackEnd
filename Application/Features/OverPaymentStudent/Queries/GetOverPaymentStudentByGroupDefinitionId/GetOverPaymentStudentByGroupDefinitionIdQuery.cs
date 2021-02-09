using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.OverPaymentStudent.Queries.GetOverPaymentStudentByGroupDefinitionId
{
    public class GetOverPaymentStudentByGroupDefinitionIdQuery : IRequest<Response<List<StudentsModel>>>
    {
        public int GroupDefinitionId { get; set; }
        public class GetOverPaymentStudentByGroupDefinitionIdQueryHandler : IRequestHandler<GetOverPaymentStudentByGroupDefinitionIdQuery, Response<List<StudentsModel>>>
        {
            private readonly IOverPaymentStudentRepositoryAsync _overPaymentsstudentRepository;
            public GetOverPaymentStudentByGroupDefinitionIdQueryHandler(IOverPaymentStudentRepositoryAsync overPaymentsstudentRepository)
            {
                _overPaymentsstudentRepository = overPaymentsstudentRepository;
            }
            public async Task<Response<List<StudentsModel>>> Handle(GetOverPaymentStudentByGroupDefinitionIdQuery command, CancellationToken cancellationToken)
            {
                var overPaymentstudent = _overPaymentsstudentRepository.GetByGroupDefinitionId(command.GroupDefinitionId);
                return new Response<List<StudentsModel>>(overPaymentstudent);
            }
        }
    }
}
