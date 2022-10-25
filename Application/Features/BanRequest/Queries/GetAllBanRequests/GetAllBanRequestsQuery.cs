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
    public class GetAllBanRequestsQuery : IRequest<PagedResponse<IEnumerable<GetAllBanRequestsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? status { get; set; }
    }
    public class GetAllBanRequestsQueryHandler : IRequestHandler<GetAllBanRequestsQuery, PagedResponse<IEnumerable<GetAllBanRequestsViewModel>>>
    {
        private readonly IBanRequestRepositoryAsync _banrequestRepository;
        private readonly IMapper _mapper;
        public GetAllBanRequestsQueryHandler(IBanRequestRepositoryAsync banrequestRepository, IMapper mapper)
        {
            _banrequestRepository = banrequestRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllBanRequestsViewModel>>> Handle(GetAllBanRequestsQuery request, CancellationToken cancellationToken)
        {

            request.PageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            request.PageSize = request.PageSize < 1 ? 10 : request.PageSize;

            int count = _banrequestRepository.GetAllCount(request.status);

            var banrequest = await _banrequestRepository.GetAll(request.PageNumber, request.PageSize, request.status);
            var banrequestViewModel = _mapper.Map<IEnumerable<GetAllBanRequestsViewModel>>(banrequest);
            return new Wrappers.PagedResponse<IEnumerable<GetAllBanRequestsViewModel>>(banrequestViewModel, request.PageNumber, request.PageSize, count);
        }
    }
}
