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

namespace Application.Features.EmailTemplate.Queries.GetAllEmailTemplates
{
    public class GetAllEmailTemplatesQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllEmailTemplatesViewModel>>>
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
    public class GetAllEmailTemplatesQueryHandler : IRequestHandler<GetAllEmailTemplatesQuery, FilteredPagedResponse<IEnumerable<GetAllEmailTemplatesViewModel>>>
    {
        private readonly IEmailTemplateRepositoryAsync _emailtemplateRepository;
        private readonly IMapper _mapper;
        public GetAllEmailTemplatesQueryHandler(IEmailTemplateRepositoryAsync emailtemplateRepository, IMapper mapper)
        {
            _emailtemplateRepository = emailtemplateRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllEmailTemplatesViewModel>>> Handle(GetAllEmailTemplatesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllEmailTemplatesParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _emailtemplateRepository.GetCount(validFilter);

            var emailtemplate = await _emailtemplateRepository.GetPagedReponseAsync(validFilter);
            var emailtemplateViewModel = _mapper.Map<IEnumerable<GetAllEmailTemplatesViewModel>>(emailtemplate);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllEmailTemplatesViewModel>>(emailtemplateViewModel, validFilter, count);
        }
    }
}
