using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetLessonInstanceToAssignQuery : IRequest<PagedResponse<IReadOnlyList<LessonInstance>>>
    {
        public int? GroupInsatanceId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
    }
    public class GetLessonInstanceToAssignQueryHandler : IRequestHandler<GetLessonInstanceToAssignQuery, PagedResponse<IReadOnlyList<LessonInstance>>>
    {
        private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
        private readonly IMapper _mapper;
        public GetLessonInstanceToAssignQueryHandler(ITestInstanceRepositoryAsync testInstanceRepositoryAsync, IMapper mapper)
        {
            _testinstanceRepository = testInstanceRepositoryAsync;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IReadOnlyList<LessonInstance>>> Handle(GetLessonInstanceToAssignQuery query, CancellationToken cancellationToken)
        {
            int count = 0;
            if (query.PageNumber == 0) query.PageNumber = 1;
            if (query.PageSize == 0) query.PageSize = 10;
            var testinstance = _testinstanceRepository.GetLessonInstanceToAssign(query.GroupInsatanceId, query.PageNumber, query.PageSize, out count);
 
            return new PagedResponse<IReadOnlyList<LessonInstance>>(testinstance, query.PageNumber, query.PageSize, count);
        }
    }

}
