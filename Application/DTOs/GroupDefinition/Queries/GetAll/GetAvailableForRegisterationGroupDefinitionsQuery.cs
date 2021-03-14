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
    public class GetAvailableForRegisterationGroupDefinitionsQuery : IRequest<PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SubLevel { get; set; }
        public int? SubLevelId { get; set; }
        public int? PromoCodeInstanceId { get; set; }
    }
    public class GetAvailableForRegisterationGroupDefinitionsQueryHandler : IRequestHandler<GetAvailableForRegisterationGroupDefinitionsQuery, PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>>
    {
        private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
        private readonly IGroupInstanceRepositoryAsync _GroupInstanceRepositoryAsync;
        private readonly IGroupInstanceStudentRepositoryAsync _GroupInstanceStudentRepositoryAsync;
        private readonly IInterestedStudentRepositoryAsync _InterestedStudentRepositoryAsync;
        private readonly IOverPaymentStudentRepositoryAsync _OverPaymentStudentRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAvailableForRegisterationGroupDefinitionsQueryHandler(IGroupDefinitionRepositoryAsync GroupDefinitionService, IMapper mapper,
            IGroupInstanceRepositoryAsync GroupInstanceRepositoryAsync,
            IGroupInstanceStudentRepositoryAsync GroupInstanceStudentRepositoryAsync,
            IInterestedStudentRepositoryAsync InterestedStudentRepositoryAsync,
            IOverPaymentStudentRepositoryAsync OverPaymentStudentRepositoryAsync)
        {
            _GroupDefinitionRepositoryAsync = GroupDefinitionService;
            _mapper = mapper;
            _GroupInstanceRepositoryAsync = GroupInstanceRepositoryAsync;
            _GroupInstanceStudentRepositoryAsync = GroupInstanceStudentRepositoryAsync;
            _InterestedStudentRepositoryAsync = InterestedStudentRepositoryAsync;
            _OverPaymentStudentRepositoryAsync = OverPaymentStudentRepositoryAsync;

        }

        public async Task<PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>> Handle(GetAvailableForRegisterationGroupDefinitionsQuery request, CancellationToken cancellationToken)
        {
            int totalCount = 0;
            var validFilter = _mapper.Map<RequestParameter>(request);
            IReadOnlyList<Domain.Entities.GroupDefinition> GroupDefinitions;
            GroupDefinitions = _GroupDefinitionRepositoryAsync.GetAvailableForRegisteration(request.PageNumber, request.PageSize, request.SubLevel, out totalCount,request.SubLevelId,request.PromoCodeInstanceId);
            var groupDefinitionsModel = _mapper.Map<IEnumerable<GetAllGroupDefinitionViewModel>>(GroupDefinitions);
            foreach (var groupDefinition in groupDefinitionsModel)
            {
                groupDefinition.ActualTotalGroupInstances = _GroupInstanceRepositoryAsync.GetCountByGroupDefinitionId(groupDefinition.Id);
                groupDefinition.ActualTotalStudents = await _GroupInstanceStudentRepositoryAsync.GetCountOfStudentsByGroupDefinitionId(groupDefinition.Id);
                groupDefinition.TotalInterestedStudents = _InterestedStudentRepositoryAsync.GetCountOfStudentsByGroupDefinitionId(groupDefinition.Id);
                groupDefinition.TotalOverPaymentStudents = _OverPaymentStudentRepositoryAsync.GetCountOfStudentsByGroupDefinitionId(groupDefinition.Id);
            }
            return new PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>(groupDefinitionsModel, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
