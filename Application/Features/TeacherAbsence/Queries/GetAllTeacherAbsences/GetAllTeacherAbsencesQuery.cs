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

namespace Application.Features.TeacherAbsence.Queries.GetAllTeacherAbsences
{
    public class GetAllTeacherAbsencesQuery : IRequest<PagedResponse<IEnumerable<GetAllTeacherAbsencesViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //public Dictionary<string, string> FilterValue { get; set; }
        //public Dictionary<string, string> FilterRange { get; set; }
        //public Dictionary<string, List<string>> FilterArray { get; set; }
        //public Dictionary<string, string> FilterSearch { get; set; }
        //public string SortBy { get; set; }
        //public string SortType { get; set; }
        //public bool NoPaging { get; set; }
    }
    public class GetAllTeacherAbsencesQueryHandler : IRequestHandler<GetAllTeacherAbsencesQuery, PagedResponse<IEnumerable<GetAllTeacherAbsencesViewModel>>>
    {
        private readonly ITeacherAbsenceRepositoryAsync _teacherabsenceRepository;
        private readonly IMapper _mapper;
        public GetAllTeacherAbsencesQueryHandler(ITeacherAbsenceRepositoryAsync teacherabsenceRepository, IMapper mapper)
        {
            _teacherabsenceRepository = teacherabsenceRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllTeacherAbsencesViewModel>>> Handle(GetAllTeacherAbsencesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllTeacherAbsencesParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _teacherabsenceRepository.GetCount(validFilter);

            var teacherabsence = await _teacherabsenceRepository.GetPagedReponseAsync(validFilter);
            var teacherabsenceViewModel = _mapper.Map<IEnumerable<GetAllTeacherAbsencesViewModel>>(teacherabsence);
            return new Wrappers.PagedResponse<IEnumerable<GetAllTeacherAbsencesViewModel>>(teacherabsenceViewModel, request.PageNumber, request.PageSize, count);
        }
    }
}
