using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Features.GroupInstancesStudents.Commands
{
    public class SaveAllGroupInstanceStudentsCommand : IRequest<Response<int>>
    {
        public int GroupDefinitionId { get; set; }
        public List<StudentsGroupInstanceModel> GroupInstancesStudentList { get; set; }
        public class SaveAllGroupInstanceStudentsCommandHandler : IRequestHandler<SaveAllGroupInstanceStudentsCommand, Response<int>>
        {
            private readonly IGroupDefinitionRepositoryAsync _groupDefinitionRepositoryAsync;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IMapper _mapper;
            public SaveAllGroupInstanceStudentsCommandHandler(IGroupDefinitionRepositoryAsync groupDefinitionRepositoryAsync,
                IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
                 IMapper mapper)
            {
                _groupDefinitionRepositoryAsync = groupDefinitionRepositoryAsync;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<int>> Handle(SaveAllGroupInstanceStudentsCommand command, CancellationToken cancellationToken)
            {
                var groupdefinitionobject =  _groupDefinitionRepositoryAsync.GetById(command.GroupDefinitionId);

                if (groupdefinitionobject == null)
                {
                    throw new ApiException($"Group Definition Not Found.");
                }
                // List<GroupInstanceStudents>groupInstanceStidentObject = _mapper.Map<List<GroupInstanceStudents>>(command.GroupInstancesStudentList);
                using (TransactionScope scope = new TransactionScope())
                {
                    List<GroupInstanceStudents> groupInstanceStudentsObject = new List<GroupInstanceStudents>();
                    var students = _groupInstanceStudentRepositoryAsync.SaveAllGroupInstanceStudents(command.GroupDefinitionId, command.GroupInstancesStudentList,out groupInstanceStudentsObject);
                    await _groupInstanceStudentRepositoryAsync.DeleteBulkAsync(students);
                    await _groupInstanceStudentRepositoryAsync.AddBulkAsync(groupInstanceStudentsObject);
                    scope.Complete();
                }
                return new Response<int>(groupdefinitionobject.Id);

            }
        }
    }
}
