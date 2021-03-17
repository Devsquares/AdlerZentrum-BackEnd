using Application.Exceptions;
using Application.Interfaces;
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

namespace Application.DTOs
{
    public class UpdateTimeSlotCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public ICollection<TimeSlotDetails> TimeSlotDetails { get; set; }

        public class UpdateTimeSlotCommandHandler : IRequestHandler<UpdateTimeSlotCommand, Response<int>>
        {
            private readonly ITimeSlotRepositoryAsync _TimeSlotRepository;
            public UpdateTimeSlotCommandHandler(ITimeSlotRepositoryAsync TimeSlotRepository)
            {
                _TimeSlotRepository = TimeSlotRepository;
            }
            public async Task<Response<int>> Handle(UpdateTimeSlotCommand command, CancellationToken cancellationToken)
            {
                var TimeSlot = await _TimeSlotRepository.GetByIdAsync(command.Id);

                if (TimeSlot == null)
                {
                    throw new ApiException($"TimeSlot Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, TimeSlot);
                    _TimeSlotRepository.DeleteDetails(command.Id);
                    await _TimeSlotRepository.UpdateAsync(TimeSlot);
                    foreach (var item in TimeSlot.TimeSlotDetails)
                    {
                        item.TimeSlotId = command.Id;
                    }
                    _TimeSlotRepository.AddDetails(TimeSlot.TimeSlotDetails.ToList());
                    return new Response<int>(TimeSlot.Id);
                }
            }
        }
    }
}
