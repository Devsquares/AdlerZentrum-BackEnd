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
    public class CancelSingleGroupInstanceCommand : IRequest<Response<int>>
    {
        public int GroupInstanceId { get; set; }
        public class CancelSingleGroupInstanceCommandHandler : IRequestHandler<CancelSingleGroupInstanceCommand, Response<int>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly ITestInstanceRepositoryAsync _testInstanceRepositoryAsync;
            public CancelSingleGroupInstanceCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
                IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
                IGroupDefinitionRepositoryAsync GroupDefinitionRepository,
                ITestInstanceRepositoryAsync testInstanceRepositoryAsync,
                IInterestedStudentRepositoryAsync InterestedStudentRepositoryAsync)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _GroupDefinitionRepository = GroupDefinitionRepository;
                _testInstanceRepositoryAsync = testInstanceRepositoryAsync;
            }
            /// <summary>
            /// add all students from all group instances to interested and overpayment tables then delete them and  the delete all groupInstances
            /// </summary>
            /// <param name="command"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<Response<int>> Handle(CancelSingleGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = _groupInstanceRepositoryAsync.GetByIdAsync(command.GroupInstanceId).Result;
                if (groupInstance == null) throw new ApiException($"Group definition Not Found.");
                List<InterestedStudent> interestedStudents = new List<InterestedStudent>();
                List<OverPaymentStudent> overPaymentStudent = new List<OverPaymentStudent>();
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var allStudents = _groupInstanceStudentRepositoryAsync.GetByGroupDefinitionAndGroupInstance(groupInstance.GroupDefinitionId, groupInstance.Id);
                    if (allStudents != null && allStudents.Count > 0)
                    {
                        foreach (var student in allStudents)
                        {
                            student.IsDefault = false;
                        }
                        await _groupInstanceStudentRepositoryAsync.UpdateBulkAsync(allStudents);
                    }
                    var tests = _testInstanceRepositoryAsync.GetAllTestInstancesByGroup(command.GroupInstanceId).Result;
                    if (tests != null && tests.Count > 0)
                    {
                        foreach (var test in tests)
                        {
                            test.Status = (int)TestStatusEnum.Cancel;
                        }
                        await _testInstanceRepositoryAsync.UpdateBulkAsync(tests);
                    }
                    groupInstance.Status = (int)GroupInstanceStatusEnum.Canceld;
                    await _groupInstanceRepositoryAsync.UpdateAsync(groupInstance);
                    scope.Complete();
                }
                return new Response<int>(groupInstance.Id);

            }
        }
    }
}
