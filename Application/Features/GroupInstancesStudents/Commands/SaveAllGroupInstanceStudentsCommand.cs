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
            private readonly IGroupConditionDetailsRepositoryAsync _groupconditiondetailsRepository;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IMapper _mapper;
            public SaveAllGroupInstanceStudentsCommandHandler(IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository,
                IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
                 IMapper mapper)
            {
                _groupconditiondetailsRepository = groupconditiondetailsRepository;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<int>> Handle(SaveAllGroupInstanceStudentsCommand command, CancellationToken cancellationToken)
            {
                var groupconditiondetails = await _groupconditiondetailsRepository.GetByIdAsync(command.GroupDefinitionId);

                if (groupconditiondetails == null)
                {
                    throw new ApiException($"GroupConditionDetails Not Found.");
                }
                // List<GroupInstanceStudents>groupInstanceStidentObject = _mapper.Map<List<GroupInstanceStudents>>(command.GroupInstancesStudentList);
                using (TransactionScope scope = new TransactionScope())
                {
                    _groupInstanceStudentRepositoryAsync.SaveAllGroupInstanceStudents(command.GroupDefinitionId, command.GroupInstancesStudentList);
                    scope.Complete();
                }
                return new Response<int>(groupconditiondetails.Id);

            }
        }
    }
}
