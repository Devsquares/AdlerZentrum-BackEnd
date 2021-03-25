using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetInterestedStudentByGroupDefinitionIdQuery : IRequest<Response<List<StudentsModel>>>
    {
        public int GroupDefinitionId { get; set; }
        public class GetInterestedStudentByGroupDefinitionIdQueryHandler : IRequestHandler<GetInterestedStudentByGroupDefinitionIdQuery, Response<List<StudentsModel>>>
        {
            private readonly IInterestedStudentRepositoryAsync _interestedstudentRepository;
            public GetInterestedStudentByGroupDefinitionIdQueryHandler(IInterestedStudentRepositoryAsync interestedstudentRepository)
            {
                _interestedstudentRepository = interestedstudentRepository;
            }
            public async Task<Response<List<StudentsModel>>> Handle(GetInterestedStudentByGroupDefinitionIdQuery command, CancellationToken cancellationToken)
            {
                var interestedstudent = _interestedstudentRepository.GetByGroupDefinitionId(command.GroupDefinitionId);
                return new Response<List<StudentsModel>>(interestedstudent);
            }
        }
    }
}
