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

namespace Application.DTOs.GroupInstance.Commands
{
    public class UpdateGroupInstanceCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int GroupDefinitionId { get; set; }
        public string Serial { get; set; }
        public int? Status { get; set; }

        public class UpdateGroupInstanceCommandHandler : IRequestHandler<UpdateGroupInstanceCommand, Response<int>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            public UpdateGroupInstanceCommandHandler(IGroupInstanceRepositoryAsync GroupInstanceRepository)
            {
                _groupInstanceRepositoryAsync = GroupInstanceRepository;
            }
            public async Task<Response<int>> Handle(UpdateGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var GroupInstance = await _groupInstanceRepositoryAsync.GetByIdAsync(command.Id);

                if (GroupInstance == null)
                {
                    throw new ApiException($"Group Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, GroupInstance);
                    await _groupInstanceRepositoryAsync.UpdateAsync(GroupInstance);
                    return new Response<int>(GroupInstance.Id);
                }
            }
        }
    }
}
