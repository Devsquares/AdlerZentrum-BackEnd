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

namespace Application.Features.GroupInstancesStudents.Commands
{
    public class RemoveStudentGroupInstanceCommand : IRequest<Response<int>>
    {
        public string StudentId { get; set; }
        public int GroupInstanceId { get; set; }
        public class RemoveStudentGroupInstanceCommandHandler : IRequestHandler<RemoveStudentGroupInstanceCommand, Response<int>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IOverPaymentStudentRepositoryAsync _overPaymentStudentRepositoryAsync;
            private readonly IInterestedStudentRepositoryAsync _InterestedStudentRepositoryAsync;
            public RemoveStudentGroupInstanceCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
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
            /// add only one student from  group instance to interested or overpayment tables then delete him
            /// </summary>
            /// <param name="command"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<Response<int>> Handle(RemoveStudentGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = _groupInstanceRepositoryAsync.GetByIdAsync(command.GroupInstanceId).Result;
                if (groupInstance == null) throw new ApiException($"Group Instance Not Found.");
                var student = _groupInstanceStudentRepositoryAsync.GetByStudentId(command.StudentId, command.GroupInstanceId);
                using (TransactionScope scope = new TransactionScope())
                {
                    if (student.PromoCodeId != null)
                    {
                        await _InterestedStudentRepositoryAsync.AddAsync(new Domain.Entities.InterestedStudent()
                        {
                            StudentId = student.StudentId,
                            GroupDefinitionId = groupInstance.Id,
                            CreatedDate = DateTime.Now,
                            IsPlacementTest = false,
                            PromoCodeId = student.PromoCodeId.Value,
                            RegisterDate = student.CreatedDate.Value,
                        });
                    }
                    else
                    {
                        await _overPaymentStudentRepositoryAsync.AddAsync(new Domain.Entities.OverPaymentStudent()
                        {
                            StudentId = student.StudentId,
                            GroupDefinitionId = groupInstance.Id,
                            CreatedDate = DateTime.Now,
                            IsPlacementTest = student.IsPlacementTest,
                            RegisterDate = student.CreatedDate.Value,
                        });
                    }
                    await _groupInstanceStudentRepositoryAsync.DeleteAsync(student);
                    var studentCount = _groupInstanceStudentRepositoryAsync.GetCountOfStudents(groupInstance.Id);
                    if (studentCount == 0)
                    {
                        await _groupInstanceRepositoryAsync.DeleteAsync(groupInstance);
                    }
                    scope.Complete();
                }
                return new Response<int>(groupInstance.Id);

            }
        }
    }
}
