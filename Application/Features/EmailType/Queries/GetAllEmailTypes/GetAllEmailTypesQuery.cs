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

namespace Application.Features.EmailType.Queries.GetAllEmailTypes
{
    public class GetAllEmailTypesQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllEmailTypesViewModel>>>
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
    public class GetAllEmailTypesQueryHandler : IRequestHandler<GetAllEmailTypesQuery, FilteredPagedResponse<IEnumerable<GetAllEmailTypesViewModel>>>
    {
        private readonly IEmailTypeRepositoryAsync _emailtypeRepository;
        private readonly IMapper _mapper;
        public GetAllEmailTypesQueryHandler(IEmailTypeRepositoryAsync emailtypeRepository, IMapper mapper)
        {
            _emailtypeRepository = emailtypeRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllEmailTypesViewModel>>> Handle(GetAllEmailTypesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllEmailTypesParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _emailtypeRepository.GetCount(validFilter);

            var emailtype = await _emailtypeRepository.GetPagedReponseAsync(validFilter);
            var emailtypeViewModel = _mapper.Map<IEnumerable<GetAllEmailTypesViewModel>>(emailtype);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllEmailTypesViewModel>>(emailtypeViewModel, validFilter, count);
        }
    }
}
