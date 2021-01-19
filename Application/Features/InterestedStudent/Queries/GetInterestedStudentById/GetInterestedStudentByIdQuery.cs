using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InterestedStudent.Queries.GetInterestedStudentById
{
    public class GetInterestedStudentByIdQuery : IRequest<Response<Domain.Entities.InterestedStudent>>
    {
        public int Id { get; set; }
        public class GetInterestedStudentByIdQueryHandler : IRequestHandler<GetInterestedStudentByIdQuery, Response<Domain.Entities.InterestedStudent>>
        {
            private readonly IInterestedStudentRepositoryAsync _interestedstudentRepository;
            public GetInterestedStudentByIdQueryHandler(IInterestedStudentRepositoryAsync interestedstudentRepository)
            {
                _interestedstudentRepository = interestedstudentRepository;
            }
            public async Task<Response<Domain.Entities.InterestedStudent>> Handle(GetInterestedStudentByIdQuery query, CancellationToken cancellationToken)
            {
                var interestedstudent = await _interestedstudentRepository.GetByIdAsync(query.Id);
                if (interestedstudent == null) throw new ApiException($"InterestedStudent Not Found.");
                return new Response<Domain.Entities.InterestedStudent>(interestedstudent);
            }
        }
    }
}
