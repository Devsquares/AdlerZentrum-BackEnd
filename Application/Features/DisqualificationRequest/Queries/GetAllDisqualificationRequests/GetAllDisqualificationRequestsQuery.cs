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
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int? status { get; set; }
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
            if (request.pageNumber == 0) request.pageNumber = 1;
            if (request.pageSize == 0) request.pageSize = 10;
            int count = _disqualificationrequestRepository.GetAllCount(request.status);

            var disqualificationrequest = await _disqualificationrequestRepository.GetAll(request.pageNumber, request.pageSize, request.status);
            var disqualificationrequestViewModel = _mapper.Map<IEnumerable<GetAllDisqualificationRequestsViewModel>>(disqualificationrequest);
            return new PagedResponse<IEnumerable<GetAllDisqualificationRequestsViewModel>>(disqualificationrequestViewModel, request.pageNumber, request.pageSize, count);
        }
    }
}
