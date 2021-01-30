using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.DTOs.GroupInstance.Commands
{
    public class CreateGroupInstanceWithInterestedOverPaymentStudentCommand : IRequest<Response<StudentsGroupInstanceModel>>
    {
        public int GroupDefinitionId { get; set; }
        public class CreateGroupInstanceWithInterestedOverPaymentStudentCommandHandler : IRequestHandler<CreateGroupInstanceWithInterestedOverPaymentStudentCommand, Response<StudentsGroupInstanceModel>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepositoryAsync;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
            private readonly IInterestedStudentRepositoryAsync _interestedStudentRepositoryAsync;
            private readonly IOverPaymentStudentRepositoryAsync _overPaymentStudentRepositoryAsync;
            public CreateGroupInstanceWithInterestedOverPaymentStudentCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
                  IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepositoryAsync,
             IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
             IGroupDefinitionRepositoryAsync GroupDefinitionRepository,
             IInterestedStudentRepositoryAsync interestedStudentRepositoryAsync,
              IOverPaymentStudentRepositoryAsync overPaymentStudentRepositoryAsync)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _groupConditionPromoCodeRepositoryAsync = groupConditionPromoCodeRepositoryAsync;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _GroupDefinitionRepositoryAsync = GroupDefinitionRepository;
                _interestedStudentRepositoryAsync = interestedStudentRepositoryAsync;
                _overPaymentStudentRepositoryAsync = overPaymentStudentRepositoryAsync;
            }
            public async Task<Response<StudentsGroupInstanceModel>> Handle(CreateGroupInstanceWithInterestedOverPaymentStudentCommand command, CancellationToken cancellationToken)
            {


                var groupDefinitionobject = _GroupDefinitionRepositoryAsync.GetById(command.GroupDefinitionId);
                if (groupDefinitionobject == null)
                {
                    throw new ApiException($"Group definition Not Found");
                }
                if (groupDefinitionobject.Status == (int)GroupDefinationStatusEnum.Finished || groupDefinitionobject.Status == (int)GroupDefinationStatusEnum.Canceld)
                {
                    throw new ApiException($"Group definition finished or canceled");
                }

                Domain.Entities.GroupInstance groupInstanceobject = new Domain.Entities.GroupInstance()
                {
                    GroupDefinitionId = groupDefinitionobject.Id,
                    CreatedDate = DateTime.Now,
                    Status = (int)GroupInstanceStatusEnum.Pending,

                };
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    await _groupInstanceRepositoryAsync.AddAsync(groupInstanceobject);

                    var interestedStudentsList = _interestedStudentRepositoryAsync.GetListByGroupDefinitionId(command.GroupDefinitionId);
                    var overPaymentStudentList = _overPaymentStudentRepositoryAsync.GetListByGroupDefinitionId(command.GroupDefinitionId);
                    bool canApplyInSpecificGroup = false;
                    int studentCount = 0;
                    int totalStudents = groupDefinitionobject.GroupCondition.NumberOfSlots;
                    List<GroupInstanceStudents> interestedGroupInstanceStudents = new List<GroupInstanceStudents>();
                    List<InterestedStudent> acceptedInterestedGroupInstanceStudents = new List<InterestedStudent>();
                    // Add interested with  promoCOdes
                    if (interestedStudentsList != null && interestedStudentsList.Count > 0)
                    {

                        foreach (var interestedStudent in interestedStudentsList)
                        {
                            canApplyInSpecificGroup = _groupConditionPromoCodeRepositoryAsync.CheckPromoCodeCountInGroupInstance(groupInstanceobject.Id, interestedStudent.PromoCodeId, interestedGroupInstanceStudents);
                            if (canApplyInSpecificGroup && studentCount < totalStudents)
                            {
                                interestedGroupInstanceStudents.Add(new GroupInstanceStudents
                                {
                                    GroupInstanceId = groupInstanceobject.Id,
                                    StudentId = interestedStudent.Student.Id,
                                    PromoCodeId = interestedStudent.PromoCode.Id,
                                    CreatedDate = DateTime.Now,
                                    IsDefault = true
                                });

                                studentCount++;
                                acceptedInterestedGroupInstanceStudents.Add(interestedStudent);
                            }
                            else if (studentCount == totalStudents)
                            {
                                break;
                            }
                        }
                        if (interestedGroupInstanceStudents != null && interestedGroupInstanceStudents.Count > 0)
                        {
                            await _groupInstanceStudentRepositoryAsync.AddBulkAsync(interestedGroupInstanceStudents);
                            await _interestedStudentRepositoryAsync.DeleteBulkAsync(acceptedInterestedGroupInstanceStudents);
                        }
                    }

                    List<GroupInstanceStudents> OverPaymentsGroupInstanceStudents = new List<GroupInstanceStudents>();
                    List<OverPaymentStudent> acceptedOverPaymentStudent = new List<OverPaymentStudent>();
                    // Add overpayment students
                    if (overPaymentStudentList != null && overPaymentStudentList.Count > 0)
                    {
                        foreach (var overPaymentStudent in overPaymentStudentList)
                        {
                            if (studentCount < totalStudents)
                            {
                                OverPaymentsGroupInstanceStudents.Add(new GroupInstanceStudents
                                {
                                    GroupInstanceId = groupInstanceobject.Id,
                                    StudentId = overPaymentStudent.Student.Id,
                                    IsDefault = true
                                });

                                studentCount++;
                                acceptedOverPaymentStudent.Add(overPaymentStudent);
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (OverPaymentsGroupInstanceStudents != null && OverPaymentsGroupInstanceStudents.Count > 0)
                        {
                            await _groupInstanceStudentRepositoryAsync.AddBulkAsync(OverPaymentsGroupInstanceStudents);
                            await _overPaymentStudentRepositoryAsync.DeleteBulkAsync(acceptedOverPaymentStudent);
                        }
                    }

                    if (studentCount == totalStudents)
                    {
                        groupInstanceobject.Status = (int)GroupInstanceStatusEnum.SlotCompleted;
                        await _groupInstanceRepositoryAsync.UpdateAsync(groupInstanceobject);
                    }
                    transactionScope.Complete();
                }
                List<int> groupInstanceId = new List<int>();
                groupInstanceId.Add(groupInstanceobject.Id);
                var groupInstanceStudents = _groupInstanceRepositoryAsync.GetListByGroupDefinitionId(command.GroupDefinitionId, groupInstanceId);
                return new Response<StudentsGroupInstanceModel>((groupInstanceStudents != null && groupInstanceStudents.Count > 0) ? groupInstanceStudents[0] : null);

            }
        }
    }
}
