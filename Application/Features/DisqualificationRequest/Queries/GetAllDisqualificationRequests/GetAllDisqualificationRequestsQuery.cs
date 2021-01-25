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
    public class GetAllDisqualificationRequestsQuery : IRequest<PagedResponse<IEnumerable<GetAllDisqualificationRequestsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllDisqualificationRequestsQueryHandler : IRequestHandler<GetAllDisqualificationRequestsQuery, PagedResponse<IEnumerable<GetAllDisqualificationRequestsViewModel>>>
    {
        private readonly IDisqualificationRequestRepositoryAsync _disqualificationrequestRepository;
        private readonly IMapper _mapper;
        public GetAllDisqualificationRequestsQueryHandler(IDisqualificationRequestRepositoryAsync disqualificationrequestRepository, IMapper mapper)
        {
            _disqualificationrequestRepository = disqualificationrequestRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllDisqualificationRequestsViewModel>>> Handle(GetAllDisqualificationRequestsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllDisqualificationRequestsParameter>(request);
            RequestParameter RequestParameter = new RequestParameter();
            Reflection.CopyProperties(validFilter, RequestParameter);
            int count = _disqualificationrequestRepository.GetCount(validFilter);

            var disqualificationrequest = await _disqualificationrequestRepository.GetPagedReponseAsync(validFilter);
            var disqualificationrequestViewModel = _mapper.Map<IEnumerable<GetAllDisqualificationRequestsViewModel>>(disqualificationrequest);
            return new PagedResponse<IEnumerable<GetAllDisqualificationRequestsViewModel>>(disqualificationrequestViewModel, validFilter.PageNumber, validFilter.PageSize, count);
        }
    }
}
