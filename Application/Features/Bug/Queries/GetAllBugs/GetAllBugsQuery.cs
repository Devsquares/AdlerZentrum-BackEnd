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

namespace Application.Features.Bug.Queries.GetAllBugs
{
    public class GetAllBugsQuery : IRequest<BugPagedResponse<IEnumerable<GetAllBugsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
    public class GetAllBugsQueryHandler : IRequestHandler<GetAllBugsQuery, BugPagedResponse<IEnumerable<GetAllBugsViewModel>>>
    {
        private readonly IBugRepositoryAsync _bugRepository;
        private readonly IMapper _mapper;
        public GetAllBugsQueryHandler(IBugRepositoryAsync bugRepository, IMapper mapper)
        {
            _bugRepository = bugRepository;
            _mapper = mapper;
        }

        public async Task<BugPagedResponse<IEnumerable<GetAllBugsViewModel>>> Handle(GetAllBugsQuery request, CancellationToken cancellationToken)
        {
            int count = _bugRepository.GetCount(request.Type, request.Status, request.Priority);
            var bug = await _bugRepository.GetPagedReponseAsync(request.PageNumber, request.PageSize, request.Type, request.Status, request.Priority);
            var bugViewModel = _mapper.Map<IEnumerable<GetAllBugsViewModel>>(bug);
            return new Wrappers.BugPagedResponse<IEnumerable<GetAllBugsViewModel>>(bugViewModel, request.PageNumber, request.PageSize, count, request.Type, request.Status, request.Priority);
        }
    }
}
