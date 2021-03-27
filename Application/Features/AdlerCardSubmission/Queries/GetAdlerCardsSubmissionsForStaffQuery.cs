using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AdlerCardSubmission.Queries
{
    public class GetAdlerCardsSubmissionsForStaffQuery : IRequest<PagedResponse<IEnumerable<AdlerCardsSubmissionsForStaffModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string studentId { get; set; }
        public string studentName { get; set; }
        public int? levelId { get; set; }
        public string levelName { get; set; }
        public int? type { get; set; }
        public int? status { get; set; }
        public bool? assigned { get; set; }
        public string TeacherId { get; set; }
    }
    public class GetAdlerCardsSubmissionsForStaffQueryHandler : IRequestHandler<GetAdlerCardsSubmissionsForStaffQuery, PagedResponse<IEnumerable<AdlerCardsSubmissionsForStaffModel>>>
    {
        private readonly IAdlerCardSubmissionRepositoryAsync _adlercardsubmissionRepository;
        public GetAdlerCardsSubmissionsForStaffQueryHandler(IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository)
        {
            _adlercardsubmissionRepository = adlercardsubmissionRepository;
        }

        public async Task<PagedResponse<IEnumerable<AdlerCardsSubmissionsForStaffModel>>> Handle(GetAdlerCardsSubmissionsForStaffQuery request, CancellationToken cancellationToken)
        {
            int totalCount = 0;
            var adlercardsubmission =  _adlercardsubmissionRepository.GetAdlerCardsSubmissionsForStaff(request.PageNumber, request.PageSize, request.studentId, request.studentName, request.levelId, request.levelName, request.type, request.status, request.assigned,request.TeacherId, out totalCount);
            return new PagedResponse<IEnumerable<AdlerCardsSubmissionsForStaffModel>>(adlercardsubmission, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
