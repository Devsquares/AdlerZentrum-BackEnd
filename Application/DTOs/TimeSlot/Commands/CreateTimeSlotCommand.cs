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

namespace Application.DTOs
{
    public class CreateTimeSlotCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public virtual ICollection<TimeSlotDetails> TimeSlotDetails { get; set; }

        public class CreateTimeSlotCommandHandler : IRequestHandler<CreateTimeSlotCommand, Response<int>>
        {
            private readonly ITimeSlotRepositoryAsync _TimeSlotRepository;
            public CreateTimeSlotCommandHandler(ITimeSlotRepositoryAsync TimeSlotRepository)
            {
                _TimeSlotRepository = TimeSlotRepository;
            }
            public async Task<Response<int>> Handle(CreateTimeSlotCommand command, CancellationToken cancellationToken)
            {
                var TimeSlot = new Domain.Entities.TimeSlot();

                Reflection.CopyProperties(command, TimeSlot);
                await _TimeSlotRepository.AddAsync(TimeSlot);
                return new Response<int>(TimeSlot.Id);

            }
        }
    }
}
