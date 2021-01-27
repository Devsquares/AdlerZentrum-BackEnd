using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.GroupInstance.Commands
{
    class CreateGroupInstanceWithInterestedOverPaymentStudentCommand : IRequest<Response<int>>
    {
        public int GroupDefinitionId { get; set; }
        public class CreateGroupInstanceWithInterestedOverPaymentStudentCommandHandler : IRequestHandler<CreateGroupInstanceWithInterestedOverPaymentStudentCommand, Response<int>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            public CreateGroupInstanceWithInterestedOverPaymentStudentCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
            }
            public async Task<Response<int>> Handle(CreateGroupInstanceWithInterestedOverPaymentStudentCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = new Domain.Entities.GroupInstance();
                Reflection.CopyProperties(command, groupInstance);
                await _groupInstanceRepositoryAsync.AddAsync(groupInstance);
                return new Response<int>(groupInstance.Id);

            }
        }
    }
}
