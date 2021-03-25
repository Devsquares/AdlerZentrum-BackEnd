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
    public class GetAllInterestedStudentsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllInterestedStudentsViewModel>>>
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
    public class GetAllInterestedStudentsQueryHandler : IRequestHandler<GetAllInterestedStudentsQuery, FilteredPagedResponse<IEnumerable<GetAllInterestedStudentsViewModel>>>
    {
        private readonly IInterestedStudentRepositoryAsync _interestedstudentRepository;
        private readonly IMapper _mapper;
        public GetAllInterestedStudentsQueryHandler(IInterestedStudentRepositoryAsync interestedstudentRepository, IMapper mapper)
        {
            _interestedstudentRepository = interestedstudentRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllInterestedStudentsViewModel>>> Handle(GetAllInterestedStudentsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllInterestedStudentsParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _interestedstudentRepository.GetCount(validFilter);

            var interestedstudent = await _interestedstudentRepository.GetPagedReponseAsync(validFilter);
            var interestedstudentViewModel = _mapper.Map<IEnumerable<GetAllInterestedStudentsViewModel>>(interestedstudent);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllInterestedStudentsViewModel>>(interestedstudentViewModel, validFilter, count);
        }
    }
}
