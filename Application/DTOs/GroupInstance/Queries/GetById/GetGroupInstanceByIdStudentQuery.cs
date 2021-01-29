using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.GroupInstance.Queries
{
    public class GetGroupInstanceByIdStudentQuery : IRequest<Response<Domain.Entities.GroupInstance>>
    {
        public string StudentId { get; set; }

        public class GetGroupInstanceByIdStudentQueryHandler : IRequestHandler<GetGroupInstanceByIdStudentQuery, Response<Domain.Entities.GroupInstance>>
        {
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            public GetGroupInstanceByIdStudentQueryHandler(IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync)
            {
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
            }
            public async Task<Response<Domain.Entities.GroupInstance>> Handle(GetGroupInstanceByIdStudentQuery command, CancellationToken cancellationToken)
            {
                var groupInstance =  _groupInstanceStudentRepositoryAsync.GetLastByStudentId(command.StudentId);
                return new Response<Domain.Entities.GroupInstance>(groupInstance);
            }
        }
    }
}
