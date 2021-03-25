using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class ValidateGroupInstancesStudentsCommand : IRequest<Response<int>>
    {
        public int GroupDefinitionId { get; set; }
        public List<StudentsGroupInstanceModel> GroupInstancesStudentList { get; set; }
        public class ValidateGroupInstancesStudentsCommandHandler : IRequestHandler<ValidateGroupInstancesStudentsCommand, Response<int>>
        {
            private readonly IGroupConditionDetailsRepositoryAsync _groupconditiondetailsRepository;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepositoryAsync;
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
            public ValidateGroupInstancesStudentsCommandHandler(IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository,
                IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync, IGroupInstanceRepositoryAsync groupInstanceRepository,
                  IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepositoryAsync,
             IGroupDefinitionRepositoryAsync GroupDefinitionRepository)
            {
                _groupconditiondetailsRepository = groupconditiondetailsRepository;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _groupConditionPromoCodeRepositoryAsync = groupConditionPromoCodeRepositoryAsync;
                _GroupDefinitionRepositoryAsync = GroupDefinitionRepository;
            }
            public async Task<Response<int>> Handle(ValidateGroupInstancesStudentsCommand command, CancellationToken cancellationToken)
            {
                var groupdefinition = _GroupDefinitionRepositoryAsync.GetById(command.GroupDefinitionId);
                if (groupdefinition == null)
                {
                    throw new Exception("Group Definition Not Found");
                }
                foreach (var groupInstanceStudent in command.GroupInstancesStudentList)
                {
                    int paymentStudents = groupInstanceStudent.Students.Where(x => x.isPlacementTest == false && x.PromoCodeId == null).Count();
                    int promocodesStudents = groupInstanceStudent.Students.Where(x => x.isPlacementTest == false && x.PromoCodeId != null).Count();
                    int placementStudents = groupInstanceStudent.Students.Where(x => x.isPlacementTest == true && x.PromoCodeId == null).Count();
                    if (groupInstanceStudent.Students.Count() > groupdefinition.GroupCondition.NumberOfSlots) // check total students
                    {
                        throw new Exception($"Group Instance Serial {groupInstanceStudent.GroupInstanceSerail} must contain {groupdefinition.GroupCondition.NumberOfSlots} student not {groupInstanceStudent.Students.Count()} ");
                    }

                    if (placementStudents > groupdefinition.GroupCondition.NumberOfSlotsWithPlacementTest) // check placement students
                    {
                        throw new Exception($"Group Instance Serial {groupInstanceStudent.GroupInstanceSerail} must contain {groupdefinition.GroupCondition.NumberOfSlotsWithPlacementTest} placement stuident not {placementStudents} ");
                    }
                    // check promocode students
                    //todo by mazen

                }
                return new Response<int>(groupdefinition.Id);

            }
        }
    }
}
