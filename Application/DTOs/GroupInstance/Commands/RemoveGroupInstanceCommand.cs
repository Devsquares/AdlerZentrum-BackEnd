﻿using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.DTOs.GroupInstance.Commands
{
    public class RemoveGroupInstanceCommand : IRequest<Response<int>>
    {
        public int? GroupDefinitionId { get; set; }
        public int? GroupInstanceId { get; set; }
        public class RemoveGroupInstanceCommandHandler : IRequestHandler<RemoveGroupInstanceCommand, Response<int>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IOverPaymentStudentRepositoryAsync _overPaymentStudentRepositoryAsync;
            private readonly IInterestedStudentRepositoryAsync _InterestedStudentRepositoryAsync;
            public RemoveGroupInstanceCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
                IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
                IGroupDefinitionRepositoryAsync GroupDefinitionRepository,
                IOverPaymentStudentRepositoryAsync overPaymentStudentRepositoryAsync,
                IInterestedStudentRepositoryAsync InterestedStudentRepositoryAsync)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _GroupDefinitionRepository = GroupDefinitionRepository;
                _InterestedStudentRepositoryAsync = InterestedStudentRepositoryAsync;
                _overPaymentStudentRepositoryAsync = overPaymentStudentRepositoryAsync;
            }
            /// <summary>
            /// add all students from all group instances to interested and overpayment tables then delete them and  the delete all groupInstances
            /// </summary>
            /// <param name="command"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<Response<int>> Handle(RemoveGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                int groupDefinitionID = 0;
                int? groupInstanceId = null;
                if (command.GroupDefinitionId == null && command.GroupInstanceId == null)
                {
                    throw new ApiException("GroupDefinitionId and GroupInstanceId cannot be null");
                }
                if (command.GroupDefinitionId != null)
                {
                    var groupDefinitionInstance = _GroupDefinitionRepository.GetById(command.GroupDefinitionId.Value);
                    if (groupDefinitionInstance == null) throw new ApiException($"Group definition Not Found.");
                    groupDefinitionID = groupDefinitionInstance.Id;
                    groupDefinitionInstance = null;
                }
                if (command.GroupInstanceId != null)
                {
                    var groupInstance = _groupInstanceRepositoryAsync.GetByIdAsync(command.GroupInstanceId.Value).Result;
                    if (groupInstance == null) throw new ApiException($"Group Instance Not Found.");
                    if (groupInstance.Status == (int)GroupInstanceStatusEnum.Running)
                    {
                        throw new ApiException($"Can't remove this group Instance as the status is Running");
                    }
                    groupDefinitionID = groupInstance.GroupDefinitionId;
                    groupInstanceId = groupInstance.Id;
                }

                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var allStudents = _groupInstanceStudentRepositoryAsync.GetAllByGroupDefinition(command.GroupDefinitionId, command.GroupInstanceId);
                    List<InterestedStudent> interestedStudents = new List<InterestedStudent>();
                    List<OverPaymentStudent> overPaymentStudent = new List<OverPaymentStudent>();
                    foreach (var group in allStudents)
                    {
                        var students = group.ToList();
                        foreach (var student in students)
                        {
                            if (student.PromoCodeInstanceId != null)
                            {
                                interestedStudents.Add(new InterestedStudent()
                                {
                                    StudentId = student.StudentId,
                                    GroupDefinitionId = groupDefinitionID,
                                    CreatedDate = DateTime.Now,
                                    IsPlacementTest = false,
                                    PromoCodeInstanceId = student.PromoCodeInstanceId.Value,
                                    RegisterDate = student.CreatedDate.Value,
                                    IsEligible = student.IsEligible
                                });
                            }
                            else
                            {
                                overPaymentStudent.Add(new OverPaymentStudent()
                                {
                                    StudentId = student.StudentId,
                                    GroupDefinitionId = groupDefinitionID,
                                    CreatedDate = DateTime.Now,
                                    IsPlacementTest = student.IsPlacementTest,
                                    RegisterDate = student.CreatedDate.Value,
                                    IsEligible = student.IsEligible
                                });
                            }
                        }
                    }
                    await _InterestedStudentRepositoryAsync.ADDList(interestedStudents);
                    await _overPaymentStudentRepositoryAsync.ADDList(overPaymentStudent);
                    var groupStudents = _groupInstanceStudentRepositoryAsync.GetByGroupDefinitionAndGroupInstance(groupDefinitionID, groupInstanceId);
                    await _groupInstanceStudentRepositoryAsync.DeleteBulkAsync(groupStudents);
                    var groups = await _groupInstanceRepositoryAsync.GetByGroupDefinitionAndGroupInstanceWithoutSublevelAsync(groupDefinitionID, groupInstanceId);
                    await _groupInstanceRepositoryAsync.DeleteBulkAsync(groups);
                    scope.Complete();
                }
                return new Response<int>(groupDefinitionID);
            }
        }
    }
}
