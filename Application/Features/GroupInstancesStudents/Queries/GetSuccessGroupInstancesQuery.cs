using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;

namespace Application.Features
{
    public class GetSuccessGroupInstancesQuery : IRequest<Response<List<GroupInstanceModel>>>
    {
        public string Id { get; set; }
        public class GetSuccessGroupInstancesQueryHandler : IRequestHandler<GetSuccessGroupInstancesQuery, Response<List<GroupInstanceModel>>>
        {
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudent;
            public GetSuccessGroupInstancesQueryHandler(IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync)
            {
                _groupInstanceStudent = groupInstanceStudentRepositoryAsync;
            }
            public async Task<Response<List<GroupInstanceModel>>> Handle(GetSuccessGroupInstancesQuery query, CancellationToken cancellationToken)
            {
                var interestedstudent = await _groupInstanceStudent.GetSuccessGroupInstances(query.Id);
                if (interestedstudent == null) throw new ApiException($"InterestedStudent Not Found.");
                return new Response<List<GroupInstanceModel>>(interestedstudent);
            }
        }
    }
}
