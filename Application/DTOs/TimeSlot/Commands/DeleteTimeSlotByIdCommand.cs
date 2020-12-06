using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.TimeSlot.Commands
{
    public class DeleteTimeSlotByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteTimeSlotByIdCommandHandler : IRequestHandler<DeleteTimeSlotByIdCommand, Response<int>>
        {
            private readonly ITimeSlotRepositoryAsync _TimeSlotRepositoryAsync;
            public DeleteTimeSlotByIdCommandHandler(ITimeSlotRepositoryAsync TimeSlotRepositoryAsync)
            {
                _TimeSlotRepositoryAsync = TimeSlotRepositoryAsync;
            }
            public async Task<Response<int>> Handle(DeleteTimeSlotByIdCommand command, CancellationToken cancellationToken)
            {
                var TimeSlot = await _TimeSlotRepositoryAsync.GetByIdAsync(command.Id);
                if (TimeSlot == null) throw new ApiException($"TimeSlot Not Found.");
                await _TimeSlotRepositoryAsync.DeleteAsync(TimeSlot);
                return new Response<int>(TimeSlot.Id);
            }
        }
    }
}
