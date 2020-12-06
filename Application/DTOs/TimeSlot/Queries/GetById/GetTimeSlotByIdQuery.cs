using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.TimeSlot.Queries
{
    public class GetTimeSlotByIdQuery : IRequest<Response<Domain.Entities.TimeSlot>>
    {
        public int Id { get; set; }
        public class GetTimeSlotByIdQueryHandler : IRequestHandler<GetTimeSlotByIdQuery, Response<Domain.Entities.TimeSlot>>
        {
            private readonly ITimeSlotRepositoryAsync _TimeSlotService;
            public GetTimeSlotByIdQueryHandler(ITimeSlotRepositoryAsync TimeSlotService)
            {
                _TimeSlotService = TimeSlotService;
            }
            public async Task<Response<Domain.Entities.TimeSlot>> Handle(GetTimeSlotByIdQuery query, CancellationToken cancellationToken)
            {
                var TimeSlot = await _TimeSlotService.GetByIdAsync(query.Id);
                if (TimeSlot == null) throw new ApiException($"TimeSlot Not Found.");
                return new Response<Domain.Entities.TimeSlot>(TimeSlot);
            }
        }
    }
}
