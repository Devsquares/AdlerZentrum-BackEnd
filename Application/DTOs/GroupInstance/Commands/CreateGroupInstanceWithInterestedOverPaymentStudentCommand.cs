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
                    var overPaymentStudentList = _overPaymentStudentRepositoryAsync.GetDefaultListByGroupDefinitionId(command.GroupDefinitionId);
                    var placemetTestStudentList = _overPaymentStudentRepositoryAsync.GetPlacementTestListByGroupDefinitionId(command.GroupDefinitionId);
                    bool canApplyInSpecificGroup = false;
                    int studentCount = 0;
                    int PlaceMentStudentCount = 0;
                    int totalStudents = groupDefinitionobject.GroupCondition.NumberOfSlots;
                    int totalPlacementTestStudents = groupDefinitionobject.GroupCondition.NumberOfSlotsWithPlacementTest;
                    List<GroupInstanceStudents> interestedGroupInstanceStudents = new List<GroupInstanceStudents>();
                    List<InterestedStudent> acceptedInterestedGroupInstanceStudents = new List<InterestedStudent>();
                    // Add interested with  promoCOdes
                    if (interestedStudentsList != null && interestedStudentsList.Count > 0)
                    {

                        foreach (var interestedStudent in interestedStudentsList)
                        {
                            //Modified
                            canApplyInSpecificGroup = _groupConditionPromoCodeRepositoryAsync.CheckPromoCodeCountInGroupInstance(groupInstanceobject.Id, interestedStudent.PromoCodeInstanceId, interestedGroupInstanceStudents,isAutomaticCreate:true);
                            if (canApplyInSpecificGroup && studentCount < totalStudents)
                            {
                                CheckAndDeleteLogicallyStudent(interestedStudent.Student.Id);
                                interestedGroupInstanceStudents.Add(new GroupInstanceStudents
                                {
                                    GroupInstanceId = groupInstanceobject.Id,
                                    StudentId = interestedStudent.Student.Id,
                                    PromoCodeInstanceId = interestedStudent.PromoCodeInstanceId,
                                    PromoCodeInstance = interestedStudent.PromoCodeInstance,
                                    CreatedDate = DateTime.Now,
                                    IsDefault = true,
                                    IsEligible = interestedStudent.IsEligible
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
                            foreach (var item in interestedGroupInstanceStudents)
                            {
                                item.PromoCodeInstance = null;
                            }
                            await _groupInstanceStudentRepositoryAsync.AddBulkAsync(interestedGroupInstanceStudents);
                            foreach (var item in acceptedInterestedGroupInstanceStudents)
                            {
                                item.PromoCodeInstance = null;
                            }
                            await _interestedStudentRepositoryAsync.DeleteBulkAsync(acceptedInterestedGroupInstanceStudents);
                        }
                    }
                    // Add placementTest Students
                    List<GroupInstanceStudents> PlacementTestGroupInstanceStudents = new List<GroupInstanceStudents>();
                    List<OverPaymentStudent> acceptedPlacementTestStudent = new List<OverPaymentStudent>();
                    if (placemetTestStudentList !=null && placemetTestStudentList.Count>0)
                    {

                        foreach (var placemetTestStudent in placemetTestStudentList)
                        {
                            if (PlaceMentStudentCount < totalPlacementTestStudents)
                            {
                                PlacementTestGroupInstanceStudents.Add(new GroupInstanceStudents
                                {
                                    GroupInstanceId = groupInstanceobject.Id,
                                    StudentId = placemetTestStudent.Student.Id,
                                    IsDefault = true,
                                    IsPlacementTest = true,
                                    IsEligible = placemetTestStudent.IsEligible
                                });
                                PlaceMentStudentCount++;
                                studentCount++;
                                acceptedPlacementTestStudent.Add(placemetTestStudent);
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (PlacementTestGroupInstanceStudents != null && PlacementTestGroupInstanceStudents.Count > 0)
                        {
                            await _groupInstanceStudentRepositoryAsync.AddBulkAsync(PlacementTestGroupInstanceStudents);
                            await _overPaymentStudentRepositoryAsync.DeleteBulkAsync(acceptedPlacementTestStudent);
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
                                    IsDefault = true,
                                    IsEligible = overPaymentStudent.IsEligible
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

            private async void CheckAndDeleteLogicallyStudent(string studentID)
            {
                var groupinstanceStudent = _groupInstanceStudentRepositoryAsync.GetByStudentIdIsDefault(studentID);
                if(groupinstanceStudent !=null && groupinstanceStudent.GroupInstance.Status == (int)GroupInstanceStatusEnum.Pending)
                {
                    groupinstanceStudent.IsDeleted = true;
                    groupinstanceStudent.IsDefault = false;
                    await _groupInstanceStudentRepositoryAsync.UpdateAsync(groupinstanceStudent);

                }
            }
        }
    }
}
