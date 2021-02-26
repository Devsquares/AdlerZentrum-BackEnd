using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetAllAdlerCardsUnitsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllAdlerCardsUnitsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Dictionary<string, string> FilterValue { get; set; }
        public Dictionary<string, string> FilterRange { get; set; }
        public Dictionary<string, List<string>> FilterArray { get; set; }
        public Dictionary<string, string> FilterSearch { get; set; }
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public bool NoPaging { get; set; }
    }
    public class GetAllAdlerCardsUnitsQueryHandler : IRequestHandler<GetAllAdlerCardsUnitsQuery, FilteredPagedResponse<IEnumerable<GetAllAdlerCardsUnitsViewModel>>>
    {
        private readonly IAdlerCardsUnitRepositoryAsync _adlercardsunitRepository;
        private readonly IMapper _mapper;
        public GetAllAdlerCardsUnitsQueryHandler(IAdlerCardsUnitRepositoryAsync adlercardsunitRepository, IMapper mapper)
        {
            _adlercardsunitRepository = adlercardsunitRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllAdlerCardsUnitsViewModel>>> Handle(GetAllAdlerCardsUnitsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllAdlerCardsUnitsParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _adlercardsunitRepository.GetCount(validFilter);

            var adlercardsunit = await _adlercardsunitRepository.GetPagedReponseAsync(validFilter);
            var adlercardsunitViewModel = _mapper.Map<IEnumerable<GetAllAdlerCardsUnitsViewModel>>(adlercardsunit);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllAdlerCardsUnitsViewModel>>(adlercardsunitViewModel, validFilter, count);
        }
    }
}
