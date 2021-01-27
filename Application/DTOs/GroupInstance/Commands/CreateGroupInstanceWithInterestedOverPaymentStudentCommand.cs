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
    public class CreateGroupInstanceWithInterestedOverPaymentStudentCommand : IRequest<Response<StudentsGroupInstanceModel>>
    {
        public int GroupDefinitionId { get; set; }
        public class CreateGroupInstanceWithInterestedOverPaymentStudentCommandHandler : IRequestHandler<CreateGroupInstanceWithInterestedOverPaymentStudentCommand, Response<StudentsGroupInstanceModel>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            public CreateGroupInstanceWithInterestedOverPaymentStudentCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
            }
            public async Task<Response<StudentsGroupInstanceModel>> Handle(CreateGroupInstanceWithInterestedOverPaymentStudentCommand command, CancellationToken cancellationToken)
            {
                var newGroupInstance = _groupInstanceRepositoryAsync.CreateGroupFromInterestedOverPayment(command.GroupDefinitionId);
                return new Response<StudentsGroupInstanceModel>(newGroupInstance.Result);

            }
        }
    }
}
