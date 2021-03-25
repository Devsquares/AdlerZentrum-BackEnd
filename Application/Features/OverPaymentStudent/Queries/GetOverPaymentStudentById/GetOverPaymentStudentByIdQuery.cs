using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetOverPaymentStudentByIdQuery : IRequest<Response<Domain.Entities.OverPaymentStudent>>
    {
        public int Id { get; set; }
        public class GetOverPaymentStudentByIdQueryHandler : IRequestHandler<GetOverPaymentStudentByIdQuery, Response<Domain.Entities.OverPaymentStudent>>
        {
            private readonly IOverPaymentStudentRepositoryAsync _overpaymentstudentRepository;
            public GetOverPaymentStudentByIdQueryHandler(IOverPaymentStudentRepositoryAsync overpaymentstudentRepository)
            {
                _overpaymentstudentRepository = overpaymentstudentRepository;
            }
            public async Task<Response<Domain.Entities.OverPaymentStudent>> Handle(GetOverPaymentStudentByIdQuery query, CancellationToken cancellationToken)
            {
                var overpaymentstudent = await _overpaymentstudentRepository.GetByIdAsync(query.Id);
                if (overpaymentstudent == null) throw new ApiException($"OverPaymentStudent Not Found.");
                return new Response<Domain.Entities.OverPaymentStudent>(overpaymentstudent);
            }
        }
    }
}
