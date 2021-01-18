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
    public class GetAllSublevelsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllSublevelsViewModel>>>
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
    public class GetAllSublevelsQueryHandler : IRequestHandler<GetAllSublevelsQuery, FilteredPagedResponse<IEnumerable<GetAllSublevelsViewModel>>>
    {
        private readonly ISublevelRepositoryAsync _sublevelRepository;
        private readonly IMapper _mapper;
        public GetAllSublevelsQueryHandler(ISublevelRepositoryAsync sublevelRepository, IMapper mapper)
        {
            _sublevelRepository = sublevelRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllSublevelsViewModel>>> Handle(GetAllSublevelsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllSublevelsParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _sublevelRepository.GetCount(validFilter);

            var sublevel = await _sublevelRepository.GetPagedReponseAsync(validFilter);
            var sublevelViewModel = _mapper.Map<IEnumerable<GetAllSublevelsViewModel>>(sublevel);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllSublevelsViewModel>>(sublevelViewModel, validFilter, count);
        }
    }
}
