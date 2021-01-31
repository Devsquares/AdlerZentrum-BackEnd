using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RegisterStudentGroupDefinitionCommand : IRequest<Response<int>>
    {
        public int groupDefinitionId { get; set; }
        public string StudentId { get; set; }
        public int? PromoCodeId { get; set; }
        public int? PlacmentTestId { get; set; }

        public class RegisterStudentGroupDefinitionCommandHandler : IRequestHandler<RegisterStudentGroupDefinitionCommand, Response<int>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepositoryAsync;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IInterestedStudentRepositoryAsync _interestedStudentRepositoryAsync;
            private readonly IOverPaymentStudentRepositoryAsync _overPaymentStudentRepositoryAsync;
            public RegisterStudentGroupDefinitionCommandHandler(IGroupDefinitionRepositoryAsync GroupDefinitionRepository,
                IGroupInstanceRepositoryAsync groupInstanceRepositoryAsync,
                IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepositoryAsync,
                IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
                IInterestedStudentRepositoryAsync interestedStudentRepositoryAsync,
                IOverPaymentStudentRepositoryAsync overPaymentStudentRepositoryAsync)
            {
                _GroupDefinitionRepositoryAsync = GroupDefinitionRepository;
                _groupInstanceRepositoryAsync = groupInstanceRepositoryAsync;
                _groupConditionPromoCodeRepositoryAsync = groupConditionPromoCodeRepositoryAsync;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _interestedStudentRepositoryAsync = interestedStudentRepositoryAsync;
                _overPaymentStudentRepositoryAsync = overPaymentStudentRepositoryAsync;
            }
            public async Task<Response<int>> Handle(RegisterStudentGroupDefinitionCommand command, CancellationToken cancellationToken)
            {
                var GroupDefinition = await _GroupDefinitionRepositoryAsync.GetByIdAsync(command.groupDefinitionId);
                if (GroupDefinition == null)
                {
                    throw new ApiException($"Group definition Not Found");
                }
                else if (GroupDefinition.Status != (int)GroupDefinationStatusEnum.Pending || GroupDefinition.Status != (int)GroupDefinationStatusEnum.New)
                {
                    throw new ApiException($"Group definition is already running.");
                }

                var groupInstans = _groupInstanceRepositoryAsync.GetByGroupDefinitionId(command.groupDefinitionId);
                if (groupInstans == null)
                {
                    groupInstans = await _groupInstanceRepositoryAsync.AddAsync(new Domain.Entities.GroupInstance()
                    {
                        GroupDefinitionId = GroupDefinition.Id,
                        Serial = 1.ToString(),
                        Status = (int)GroupInstanceStatusEnum.Pending
                    });

                }
                //todo check if already registerd
                //todo check on isdefault
                var registeredStudent = _groupInstanceStudentRepositoryAsync.GetByStudentId(command.StudentId, groupInstans.Id);
                var interestedRegisteredStudent = _interestedStudentRepositoryAsync.GetByStudentId(command.StudentId, GroupDefinition.Id);
                var overRegisteredStudent = _overPaymentStudentRepositoryAsync.GetByStudentId(command.StudentId, GroupDefinition.Id);
                if (registeredStudent != null || interestedRegisteredStudent != null || overRegisteredStudent != null)
                {
                    throw new Exception("the student was already registered");
                }
                bool canApply = false;
                bool canApplyInGroupDefinition = false;
                bool canApplyInSpecificGroup = false;
                int studentCount = _groupInstanceStudentRepositoryAsync.GetCountOfStudents(groupInstans.Id);
                if (command.PromoCodeId.HasValue)
                {
                    canApplyInSpecificGroup = _groupConditionPromoCodeRepositoryAsync.CheckPromoCodeCountInGroupInstance(groupInstans.Id, command.PromoCodeId.Value);
                    canApplyInGroupDefinition = _groupConditionPromoCodeRepositoryAsync.CheckPromoCodeInGroupDefinitionGeneral(command.groupDefinitionId, command.PromoCodeId.Value);
                }
                if (studentCount < groupInstans.GroupDefinition.GroupCondition.NumberOfSlots)
                {
                    if (canApplyInSpecificGroup)
                    {
                        canApply = true;
                        await _groupInstanceStudentRepositoryAsync.AddAsync(new GroupInstanceStudents
                        {
                            GroupInstanceId = groupInstans.Id,
                            StudentId = command.StudentId,
                            PromoCodeId = command.PromoCodeId,
                            IsDefault = true
                        });
                    }
                    else if (!canApplyInSpecificGroup && canApplyInGroupDefinition)
                    {
                        canApply = false;
                        await _interestedStudentRepositoryAsync.AddAsync(new InterestedStudent()
                        {
                            StudentId = command.StudentId,
                            PromoCodeId = command.PromoCodeId.Value,
                            GroupDefinitionId = command.groupDefinitionId
                        });
                    }
                    else
                    {
                        if (command.PlacmentTestId.HasValue)
                        {

                            int studentPlacmentCount = await _groupInstanceStudentRepositoryAsync.GetCountOfPlacmentTestStudents(groupInstans.Id);
                            if (studentPlacmentCount < groupInstans.GroupDefinition.GroupCondition.NumberOfSlotsWithPlacementTest)
                            {
                                canApply = true;
                                await _groupInstanceStudentRepositoryAsync.AddAsync(new GroupInstanceStudents
                                {
                                    GroupInstanceId = groupInstans.Id,
                                    StudentId = command.StudentId,
                                    IsPlacementTest = true,
                                    IsDefault = true
                                });
                            }
                            ///// todo has placmenttest but cannot apply in group need to add in waiting list
                            else { }


                        }
                        else
                        {

                            canApply = true;
                            await _groupInstanceStudentRepositoryAsync.AddAsync(new GroupInstanceStudents
                            {
                                GroupInstanceId = groupInstans.Id,
                                StudentId = command.StudentId,
                                IsDefault = true
                            });
                        }
                    }
                    if (canApply)
                    {
                        if (GroupDefinition.Status == (int)GroupDefinationStatusEnum.New)
                        {
                            GroupDefinition.Status = (int)GroupDefinationStatusEnum.Pending;
                            await _GroupDefinitionRepositoryAsync.UpdateAsync(GroupDefinition);
                        }
                    }
                    if (canApply && studentCount == groupInstans.GroupDefinition.GroupCondition.NumberOfSlots)
                    {
                        groupInstans.Status = (int)GroupInstanceStatusEnum.SlotCompleted;
                        await _groupInstanceRepositoryAsync.UpdateAsync(groupInstans);
                    }
                }
                else
                {

                    if (canApplyInGroupDefinition)
                    {
                        await _interestedStudentRepositoryAsync.AddAsync(new InterestedStudent()
                        {
                            StudentId = command.StudentId,
                            PromoCodeId = command.PromoCodeId.Value,
                            GroupDefinitionId = command.groupDefinitionId
                        });
                    }
                    else
                    {
                        await _overPaymentStudentRepositoryAsync.AddAsync(new OverPaymentStudent()
                        {
                            StudentId = command.StudentId,
                            GroupDefinitionId = command.groupDefinitionId
                        });

                    }
                }
                return new Response<int>(GroupDefinition.Id);
            }
        }
    }
}
