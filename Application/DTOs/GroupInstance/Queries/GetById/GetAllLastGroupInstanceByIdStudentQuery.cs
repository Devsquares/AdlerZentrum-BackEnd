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
    public class GetAllLastGroupInstanceByIdStudentQuery : IRequest<Response<List<GroupInstanceModel>>>
    {
        public string StudentId { get; set; }

        public class GetAllLastGroupInstanceByIdStudentQueryHandler : IRequestHandler<GetAllLastGroupInstanceByIdStudentQuery, Response<List<GroupInstanceModel>>>
        {
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            public GetAllLastGroupInstanceByIdStudentQueryHandler(IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync)
            {
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
            }
            public async Task<Response<List<GroupInstanceModel>>> Handle(GetAllLastGroupInstanceByIdStudentQuery command, CancellationToken cancellationToken)
            {
                var groupInstance =  _groupInstanceStudentRepositoryAsync.GetAllLastByStudentId(command.StudentId);
                return new Response<List<GroupInstanceModel>>(groupInstance);
            }
        }
    }
}
