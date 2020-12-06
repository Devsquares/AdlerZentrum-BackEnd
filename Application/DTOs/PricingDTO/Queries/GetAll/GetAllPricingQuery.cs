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

namespace Application.DTOs.Pricing.Queries
{
    public class GetAllPricingQuery : IRequest<PagedResponse<IEnumerable<GetAllPricingViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllPricingsQueryHandler : IRequestHandler<GetAllPricingQuery, PagedResponse<IEnumerable<GetAllPricingViewModel>>>
    {
        private readonly IPricingRepositoryAsync _PricingService;
        private readonly IMapper _mapper;
        public GetAllPricingsQueryHandler(IPricingRepositoryAsync PricingService, IMapper mapper)
        {
            _PricingService = PricingService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllPricingViewModel>>> Handle(GetAllPricingQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var user = await _PricingService.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var userViewModel = _mapper.Map<IEnumerable<GetAllPricingViewModel>>(user);
            return new PagedResponse<IEnumerable<GetAllPricingViewModel>>(userViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
