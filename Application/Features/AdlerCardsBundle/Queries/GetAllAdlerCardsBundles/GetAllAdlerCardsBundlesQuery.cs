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
    public class GetAllAdlerCardsBundlesQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllAdlerCardsBundlesViewModel>>>
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
    public class GetAllAdlerCardsBundlesQueryHandler : IRequestHandler<GetAllAdlerCardsBundlesQuery, FilteredPagedResponse<IEnumerable<GetAllAdlerCardsBundlesViewModel>>>
    {
        private readonly IAdlerCardsBundleRepositoryAsync _adlercardsbundleRepository;
        private readonly IMapper _mapper;
        public GetAllAdlerCardsBundlesQueryHandler(IAdlerCardsBundleRepositoryAsync adlercardsbundleRepository, IMapper mapper)
        {
            _adlercardsbundleRepository = adlercardsbundleRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllAdlerCardsBundlesViewModel>>> Handle(GetAllAdlerCardsBundlesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllAdlerCardsBundlesParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _adlercardsbundleRepository.GetCount(validFilter);

            var adlercardsbundle = await _adlercardsbundleRepository.GetPagedReponseAsync(validFilter);
            var adlercardsbundleViewModel = _mapper.Map<IEnumerable<GetAllAdlerCardsBundlesViewModel>>(adlercardsbundle);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllAdlerCardsBundlesViewModel>>(adlercardsbundleViewModel, validFilter, count);
        }
    }
}
