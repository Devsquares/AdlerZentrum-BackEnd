using Application.Enums;
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
    public class CancelGroupInstanceCommand : IRequest<Response<int>>
    {
        public int GroupDefinitionId { get; set; }
        public class CancelGroupInstanceCommandHandler : IRequestHandler<CancelGroupInstanceCommand, Response<int>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IOverPaymentStudentRepositoryAsync _overPaymentStudentRepositoryAsync;
            private readonly IInterestedStudentRepositoryAsync _InterestedStudentRepositoryAsync;
            public CancelGroupInstanceCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
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
            public async Task<Response<int>> Handle(CancelGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupDefinitionInstance = _GroupDefinitionRepository.GetById(command.GroupDefinitionId);
                if (groupDefinitionInstance == null) throw new ApiException($"Group definition Not Found.");

                var allStudents = _groupInstanceStudentRepositoryAsync.GetAllByGroupDefinition(command.GroupDefinitionId, null);
                List<InterestedStudent> interestedStudents = new List<InterestedStudent>();
                List<OverPaymentStudent> overPaymentStudent = new List<OverPaymentStudent>();
                using (TransactionScope scope = new TransactionScope())
                {
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
                                    GroupDefinitionId = groupDefinitionInstance.Id,
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
                                    GroupDefinitionId = groupDefinitionInstance.Id,
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
                    var groupStudents = _groupInstanceStudentRepositoryAsync.GetByGroupDefinitionAndGroupInstance(groupDefinitionInstance.Id, null);
                    await _groupInstanceStudentRepositoryAsync.DeleteBulkAsync(groupStudents);
                    var groups = _groupInstanceRepositoryAsync.GetByGroupDefinitionAndGroupInstance(groupDefinitionInstance.Id, null);
                    await _groupInstanceRepositoryAsync.DeleteBulkAsync(groups);
                    groupDefinitionInstance.Status = (int)GroupDefinationStatusEnum.Canceld;
                    await _GroupDefinitionRepository.UpdateAsync(groupDefinitionInstance);
                    scope.Complete();
                }
                return new Response<int>(groupDefinitionInstance.Id);

            }
        }
    }
}
