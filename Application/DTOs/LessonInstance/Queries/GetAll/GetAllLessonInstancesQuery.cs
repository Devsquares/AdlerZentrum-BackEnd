using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllLessonInstancesQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllLessonInstancesViewModel>>>
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
    public class GetAllLessonInstancesQueryHandler : IRequestHandler<GetAllLessonInstancesQuery, FilteredPagedResponse<IEnumerable<GetAllLessonInstancesViewModel>>>
    {
        private readonly ILessonInstanceRepositoryAsync _LessonInstanceRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllLessonInstancesQueryHandler(ILessonInstanceRepositoryAsync LessonInstanceService, IMapper mapper)
        {
            _LessonInstanceRepositoryAsync = LessonInstanceService;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllLessonInstancesViewModel>>> Handle(GetAllLessonInstancesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<FilteredRequestParameter>(request);
            IReadOnlyList<Domain.Entities.LessonInstance> LessonInstances;
            LessonInstances = await _LessonInstanceRepositoryAsync.GetPagedReponseAsync(validFilter);
            var userViewModel = _mapper.Map<IEnumerable<GetAllLessonInstancesViewModel>>(LessonInstances);
            return new FilteredPagedResponse<IEnumerable<GetAllLessonInstancesViewModel>>(userViewModel, validFilter, userViewModel.ToList().Count);
        }
    }
}
