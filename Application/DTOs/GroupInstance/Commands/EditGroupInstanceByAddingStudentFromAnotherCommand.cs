using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.GroupInstance.Commands
{
    public class EditGroupInstanceByAddingStudentFromAnotherCommand : IRequest<Response<List<StudentsGroupInstanceModel>>>
    {
        public int GroupDefinitionId { get; set; }
        public int srcGroupInstanceId { get; set; }
        public int desGroupInstanceId { get; set; }
        public string studentId { get; set; }
        public int? promoCodeInstanceId { get; set; }
        public class EditGroupInstanceByAddingStudentFromAnotherCommandHandler : IRequestHandler<EditGroupInstanceByAddingStudentFromAnotherCommand, Response<List<StudentsGroupInstanceModel>>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepositoryAsync;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
            public EditGroupInstanceByAddingStudentFromAnotherCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
                  IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepositoryAsync,
             IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
             IGroupDefinitionRepositoryAsync GroupDefinitionRepository)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _groupConditionPromoCodeRepositoryAsync = groupConditionPromoCodeRepositoryAsync;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _GroupDefinitionRepositoryAsync = GroupDefinitionRepository;
            }
            public async Task<Response<List<StudentsGroupInstanceModel>>> Handle(EditGroupInstanceByAddingStudentFromAnotherCommand command, CancellationToken cancellationToken)
            {

                var groupDefinitionobject =  _GroupDefinitionRepositoryAsync.GetById(command.GroupDefinitionId);
                if (groupDefinitionobject == null)
                {
                    throw new ApiException($"Group definition Not Found");
                }
                if (groupDefinitionobject.Status == (int)GroupDefinationStatusEnum.Finished || groupDefinitionobject.Status == (int)GroupDefinationStatusEnum.Canceld)
                {
                    throw new ApiException($"Group definition finished or canceled");
                }
                var sourceGroupInstance = _groupInstanceRepositoryAsync.GetByIdPendingorCompleteAsync(command.srcGroupInstanceId).Result;
                if (sourceGroupInstance == null)
                {
                    throw new ApiException($"You cannot EDit as Source group instance {((GroupInstanceStatusEnum)sourceGroupInstance.Status).ToString()}");
                }
                var destinationGroupInstance = _groupInstanceRepositoryAsync.GetByIdPendingorCompleteAsync(command.desGroupInstanceId).Result;
                if (destinationGroupInstance == null)
                {
                    throw new ApiException($"You cannot EDit as Destination group instance {((GroupInstanceStatusEnum)destinationGroupInstance.Status).ToString()}");
                }
                bool canApplyInSpecificGroup = false;
                int totalStudents = groupDefinitionobject.GroupCondition.NumberOfSlots;
                int desTotalStudent = _groupInstanceStudentRepositoryAsync.GetCountOfStudents(command.desGroupInstanceId);
                if (desTotalStudent == totalStudents)
                {
                    throw new ApiException($"you cann't add student to the desyination group instance as it is full");
                }
                var student = _groupInstanceStudentRepositoryAsync.GetByStudentId(command.studentId, sourceGroupInstance.Id);
                if (command.promoCodeInstanceId != null)
                {
                    canApplyInSpecificGroup = _groupConditionPromoCodeRepositoryAsync.CheckPromoCodeCountInGroupInstance(destinationGroupInstance.Id, command.promoCodeInstanceId.Value);
                    if (canApplyInSpecificGroup)
                    {
                        student.GroupInstanceId = destinationGroupInstance.Id;
                        await _groupInstanceStudentRepositoryAsync.UpdateAsync(student);
                    }
                    else
                    {
                        throw new ApiException($"this promoCode is not suitable with this group");
                    }
                }
                else
                {
                    student.GroupInstanceId = destinationGroupInstance.Id;
                    await _groupInstanceStudentRepositoryAsync.UpdateAsync(student);
                }

                if (sourceGroupInstance.Status == (int)GroupInstanceStatusEnum.SlotCompleted)
                {
                    sourceGroupInstance.Status = (int)GroupInstanceStatusEnum.Pending;
                    await _groupInstanceRepositoryAsync.UpdateAsync(sourceGroupInstance);
                }
                desTotalStudent = _groupInstanceStudentRepositoryAsync.GetCountOfStudents(command.desGroupInstanceId);
                if (desTotalStudent == totalStudents)
                {
                    destinationGroupInstance.Status = (int)GroupInstanceStatusEnum.SlotCompleted;
                    await _groupInstanceRepositoryAsync.UpdateAsync(destinationGroupInstance);
                }
                List<int> groupInstanceId = new List<int>();
                groupInstanceId.Add(command.srcGroupInstanceId);
                groupInstanceId.Add(command.desGroupInstanceId);
                var studentsGroup = _groupInstanceRepositoryAsync.GetListByGroupDefinitionId(command.GroupDefinitionId, groupInstanceId);
                return new Response<List<StudentsGroupInstanceModel>>(studentsGroup);

            }
        }
    }
}
