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
    public class GetAllAdlerCardsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllAdlerCardsViewModel>>>
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
    public class GetAllAdlerCardsQueryHandler : IRequestHandler<GetAllAdlerCardsQuery, FilteredPagedResponse<IEnumerable<GetAllAdlerCardsViewModel>>>
    {
        private readonly IAdlerCardRepositoryAsync _adlercardRepository;
        private readonly IMapper _mapper;
        public GetAllAdlerCardsQueryHandler(IAdlerCardRepositoryAsync adlercardRepository, IMapper mapper)
        {
            _adlercardRepository = adlercardRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllAdlerCardsViewModel>>> Handle(GetAllAdlerCardsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllAdlerCardsParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _adlercardRepository.GetCount(validFilter);

            var adlercard = await _adlercardRepository.GetPagedReponseAsync(validFilter);
            var adlercardViewModel = _mapper.Map<IEnumerable<GetAllAdlerCardsViewModel>>(adlercard);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllAdlerCardsViewModel>>(adlercardViewModel, validFilter, count);
        }
    }
}
