using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Domain.Models;
using MediatR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.GroupInstance.Queries
{
    public class GetGroupInstanceByIdStudentQuery : IRequest<Response<GroupInstanceModel>>
    {
        public string StudentId { get; set; }

        public class GetGroupInstanceByIdStudentQueryHandler : IRequestHandler<GetGroupInstanceByIdStudentQuery, Response<GroupInstanceModel>>
        {
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            public GetGroupInstanceByIdStudentQueryHandler(IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync)
            {
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
            }
            public async Task<Response<GroupInstanceModel>> Handle(GetGroupInstanceByIdStudentQuery command, CancellationToken cancellationToken)
            {
                var groupInstance =  _groupInstanceStudentRepositoryAsync.GetLastByStudentId(command.StudentId);
                return new Response<GroupInstanceModel>(groupInstance);
            }
        }
    }
}
