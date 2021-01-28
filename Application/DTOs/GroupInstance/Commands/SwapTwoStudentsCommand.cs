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

namespace Application.DTOs.GroupInstance.Commands
{
    public class SwapTwoStudentsCommand : IRequest<Response<List<StudentsGroupInstanceModel>>>
    {
        public int GroupDefinitionId { get; set; }
        public int SrcGroupInstanceId { get; set; }
        public string SrcStudentId { get; set; }
        public int DesGroupInstanceId { get; set; }
        public string DesStudentId { get; set; }
        public class SwapTwoStudentsCommandHandler : IRequestHandler<SwapTwoStudentsCommand, Response<List<StudentsGroupInstanceModel>>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;

            private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepositoryAsync;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
            public SwapTwoStudentsCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
                  IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepositoryAsync,
             IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
             IGroupDefinitionRepositoryAsync GroupDefinitionRepository)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _groupConditionPromoCodeRepositoryAsync = groupConditionPromoCodeRepositoryAsync;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _GroupDefinitionRepositoryAsync = GroupDefinitionRepository;
            }
            public async Task<Response<List<StudentsGroupInstanceModel>>> Handle(SwapTwoStudentsCommand command, CancellationToken cancellationToken)
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
                var sourceGroupInstance = _groupInstanceRepositoryAsync.GetByIdPendingorCompleteAsync(command.SrcGroupInstanceId).Result;
                if (sourceGroupInstance == null)
                {
                    throw new ApiException($"You cannot EDit as Source group instance {((GroupInstanceStatusEnum)sourceGroupInstance.Status).ToString()}");
                }
                var destinationGroupInstance = _groupInstanceRepositoryAsync.GetByIdPendingorCompleteAsync(command.DesGroupInstanceId).Result;
                if (destinationGroupInstance == null)
                {
                    throw new ApiException($"You cannot EDit as Destination group instance {((GroupInstanceStatusEnum)destinationGroupInstance.Status).ToString()}");
                }
                var sourcestudent = _groupInstanceStudentRepositoryAsync.GetByStudentId(command.SrcStudentId, sourceGroupInstance.Id);
                var destinationstudent = _groupInstanceStudentRepositoryAsync.GetByStudentId(command.DesStudentId, destinationGroupInstance.Id);
                sourcestudent.GroupInstanceId = destinationGroupInstance.Id;
                destinationstudent.GroupInstanceId = sourceGroupInstance.Id;
                List<GroupInstanceStudents> swapstudents = new List<GroupInstanceStudents>();
                swapstudents.Add(sourcestudent);
                swapstudents.Add(destinationstudent);
                await _groupInstanceStudentRepositoryAsync.UpdateBulkAsync(swapstudents);
                List<int> groupInstanceId = new List<int>();
                groupInstanceId.Add(command.SrcGroupInstanceId);
                groupInstanceId.Add(command.DesGroupInstanceId);
                var swapStudents = _groupInstanceRepositoryAsync.GetListByGroupDefinitionId(command.GroupDefinitionId, groupInstanceId);
                return new Response<List<StudentsGroupInstanceModel>>(swapStudents);

            }
        }
    }
}
