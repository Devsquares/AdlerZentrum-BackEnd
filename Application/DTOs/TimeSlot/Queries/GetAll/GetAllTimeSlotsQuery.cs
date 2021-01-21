using Application.DTOs.Level.Queries;
using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs 
{
    public class GetAllTimeSlotsQuery : IRequest<PagedResponse<IEnumerable<GetAllTimeSlotsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllTimeSlotsQueryHandler : IRequestHandler<GetAllTimeSlotsQuery, PagedResponse<IEnumerable<GetAllTimeSlotsViewModel>>>
    {
        private readonly ITimeSlotRepositoryAsync _TimeSlotService;
        private readonly IMapper _mapper;
        public GetAllTimeSlotsQueryHandler(ITimeSlotRepositoryAsync TimeSlotService, IMapper mapper)
        {
            _TimeSlotService = TimeSlotService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllTimeSlotsViewModel>>> Handle(GetAllTimeSlotsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var user = await _TimeSlotService.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize, "TimeSlotDetails");
            var userViewModel = _mapper.Map<IEnumerable<GetAllTimeSlotsViewModel>>(user);
            return new PagedResponse<IEnumerable<GetAllTimeSlotsViewModel>>(userViewModel, validFilter.PageNumber, validFilter.PageSize,_TimeSlotService.GetCount());
        }
    }
}
