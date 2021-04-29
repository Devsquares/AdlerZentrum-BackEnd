using Application.DTOs.AccountDTO;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
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
        public int? PromoCodeInstanceId { get; set; }
        public int? PlacmentTestId { get; set; }
        public string Email { get; set; }
        public PaymentTransactionInputModel PaymentTransaction { get; set; }

        public class RegisterStudentGroupDefinitionCommandHandler : IRequestHandler<RegisterStudentGroupDefinitionCommand, Response<int>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepositoryAsync;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IInterestedStudentRepositoryAsync _interestedStudentRepositoryAsync;
            private readonly IOverPaymentStudentRepositoryAsync _overPaymentStudentRepositoryAsync;
            private readonly IPromoCodeInstanceRepositoryAsync _promoCodeInstanceRepositoryAsync;
            private readonly IStudentInfoRepositoryAsync _studentInfoRepositoryAsync;
            private readonly ISublevelRepositoryAsync _sublevelRepositoryAsync;
            private readonly IPlacementReleaseReopsitoryAsync _placementRelease;
            private readonly IPaymentTransactionsRepositoryAsync _paymentTransactionsRepository;
            private readonly IMapper _mapper;
            public RegisterStudentGroupDefinitionCommandHandler(IGroupDefinitionRepositoryAsync GroupDefinitionRepository,
                IGroupInstanceRepositoryAsync groupInstanceRepositoryAsync,
                IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepositoryAsync,
                IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
                IInterestedStudentRepositoryAsync interestedStudentRepositoryAsync,
                IOverPaymentStudentRepositoryAsync overPaymentStudentRepositoryAsync,
                IPromoCodeInstanceRepositoryAsync promoCodeInstanceRepositoryAsync,
                IStudentInfoRepositoryAsync studentInfoRepositoryAsync,
                ISublevelRepositoryAsync sublevelRepositoryAsync,
                IPlacementReleaseReopsitoryAsync placementReleaseReopsitoryAsyncs,
                IPaymentTransactionsRepositoryAsync paymentTransactionsRepositoryAsync,
                IMapper mapper)
            {
                _GroupDefinitionRepositoryAsync = GroupDefinitionRepository;
                _groupInstanceRepositoryAsync = groupInstanceRepositoryAsync;
                _groupConditionPromoCodeRepositoryAsync = groupConditionPromoCodeRepositoryAsync;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _interestedStudentRepositoryAsync = interestedStudentRepositoryAsync;
                _overPaymentStudentRepositoryAsync = overPaymentStudentRepositoryAsync;
                _promoCodeInstanceRepositoryAsync = promoCodeInstanceRepositoryAsync;
                _studentInfoRepositoryAsync = studentInfoRepositoryAsync;
                _sublevelRepositoryAsync = sublevelRepositoryAsync;
                _placementRelease = placementReleaseReopsitoryAsyncs;
                _paymentTransactionsRepository = paymentTransactionsRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<int>> Handle(RegisterStudentGroupDefinitionCommand command, CancellationToken cancellationToken)
            {
                var GroupDefinition = _GroupDefinitionRepositoryAsync.GetById(command.groupDefinitionId);
                if (command.PlacmentTestId.HasValue)
                {
                    var placementRelease = await _placementRelease.GetByIdAsync(command.PlacmentTestId.Value);
                    if (placementRelease != null)
                    {
                        command.PlacmentTestId = placementRelease.TestId;
                        TestInstance obj = new TestInstance
                        {
                            StudentId = command.StudentId,
                            Status = (int)TestInstanceEnum.Closed,
                            TestId = placementRelease.TestId,
                            StartDate = placementRelease.RelaeseDate
                        };
                    }
                }
                if (GroupDefinition == null)
                {
                    throw new ApiException($"Group definition Not Found");
                }
                else if (GroupDefinition.Status != (int)GroupDefinationStatusEnum.Pending && GroupDefinition.Status != (int)GroupDefinationStatusEnum.New)
                {
                    throw new ApiException($"Group definition is already running.");
                }
                var groupInstans = _groupInstanceRepositoryAsync.GetByGroupDefinitionId(command.groupDefinitionId);
                if (groupInstans == null)
                {
                    groupInstans = await _groupInstanceRepositoryAsync.AddAsync(new Domain.Entities.GroupInstance()
                    {
                        GroupDefinitionId = GroupDefinition.Id,
                        Status = (int)GroupInstanceStatusEnum.Pending
                    });

                    groupInstans = _groupInstanceRepositoryAsync.GetByIdAsync(groupInstans.Id).Result;
                }
                var preSublevel = _sublevelRepositoryAsync.GetPreviousByOrder(GroupDefinition.Sublevel.Order);
                var isEligible = true;
                if (preSublevel != null)
                {
                    isEligible = _studentInfoRepositoryAsync.CheckBySublevel(command.StudentId, preSublevel.Id);
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
                if (command.PromoCodeInstanceId.HasValue)
                {
                    var isPromoCodeInstanceStudent = _groupConditionPromoCodeRepositoryAsync.CheckStudentPromoCodeInstance(command.Email, command.StudentId, command.PromoCodeInstanceId.Value);
                    if (!isPromoCodeInstanceStudent)
                    {
                        throw new Exception("this promocode is not for this student");
                    }
                    canApplyInSpecificGroup = _groupConditionPromoCodeRepositoryAsync.CheckPromoCodeCountInGroupInstance(groupInstans.Id, command.PromoCodeInstanceId.Value);
                    canApplyInGroupDefinition = _groupConditionPromoCodeRepositoryAsync.CheckPromoCodeInGroupDefinitionGeneral(command.groupDefinitionId, command.PromoCodeInstanceId.Value);
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
                            PromoCodeInstanceId = command.PromoCodeInstanceId,
                            IsDefault = false,
                            CreatedDate = DateTime.Now,
                            IsEligible = isEligible
                        });
                        var promocodeinstance = _promoCodeInstanceRepositoryAsync.GetById(command.PromoCodeInstanceId.Value);
                        promocodeinstance.IsUsed = true;
                        await _promoCodeInstanceRepositoryAsync.UpdateAsync(promocodeinstance);

                    }
                    else if (!canApplyInSpecificGroup && canApplyInGroupDefinition)
                    {
                        canApply = false;
                        await _interestedStudentRepositoryAsync.AddAsync(new InterestedStudent()
                        {
                            StudentId = command.StudentId,
                            PromoCodeInstanceId = command.PromoCodeInstanceId.Value,
                            GroupDefinitionId = command.groupDefinitionId,
                            CreatedDate = DateTime.Now,
                            IsPlacementTest = false,
                            RegisterDate = DateTime.Now,
                            IsEligible = isEligible
                        });
                        var promocodeinstance = _promoCodeInstanceRepositoryAsync.GetById(command.PromoCodeInstanceId.Value);
                        promocodeinstance.IsUsed = true;
                        await _promoCodeInstanceRepositoryAsync.UpdateAsync(promocodeinstance);
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
                                    IsDefault = false,
                                    CreatedDate = DateTime.Now,
                                    IsEligible = isEligible
                                });
                            }
                            else
                            {
                                await _overPaymentStudentRepositoryAsync.AddAsync(new OverPaymentStudent()
                                {
                                    StudentId = command.StudentId,
                                    GroupDefinitionId = command.groupDefinitionId,
                                    IsPlacementTest = true,
                                    CreatedDate = DateTime.Now,
                                    RegisterDate = DateTime.Now,
                                    IsEligible = isEligible
                                });
                            }


                        }
                        else
                        {

                            canApply = true;
                            await _groupInstanceStudentRepositoryAsync.AddAsync(new GroupInstanceStudents
                            {
                                GroupInstanceId = groupInstans.Id,
                                StudentId = command.StudentId,
                                IsDefault = false,
                                IsEligible = isEligible
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
                            PromoCodeInstanceId = command.PromoCodeInstanceId.Value,
                            GroupDefinitionId = command.groupDefinitionId,
                            CreatedDate = DateTime.Now,
                            IsPlacementTest = false,
                            RegisterDate = DateTime.Now,
                            IsEligible = isEligible

                        });
                        var promocodeinstance = _promoCodeInstanceRepositoryAsync.GetById(command.PromoCodeInstanceId.Value);
                        promocodeinstance.IsUsed = true;
                        await _promoCodeInstanceRepositoryAsync.UpdateAsync(promocodeinstance);
                    }
                    else
                    {
                        if (command.PlacmentTestId.HasValue)
                        {
                            await _overPaymentStudentRepositoryAsync.AddAsync(new OverPaymentStudent()
                            {
                                StudentId = command.StudentId,
                                GroupDefinitionId = command.groupDefinitionId,
                                IsPlacementTest = true,
                                CreatedDate = DateTime.Now,
                                RegisterDate = DateTime.Now,
                                IsEligible = isEligible
                            });
                        }
                        else
                        {
                            await _overPaymentStudentRepositoryAsync.AddAsync(new OverPaymentStudent()
                            {
                                StudentId = command.StudentId,
                                GroupDefinitionId = command.groupDefinitionId,
                                IsPlacementTest = false,
                                CreatedDate = DateTime.Now,
                                RegisterDate = DateTime.Now,
                                IsEligible = isEligible
                            });
                        }

                    }
                }
                if (command.PaymentTransaction != null)
                {
                    var obj = _mapper.Map<PaymentTransaction>(command.PaymentTransaction);
                    await _paymentTransactionsRepository.AddAsync(obj);
                }
                return new Response<int>(GroupDefinition.Id);
            }
        }
    }
}
