using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllGroupDefinitionsQuery : IRequest<PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SubLevel { get; set; }
        public int? SubLevelId { get; set; }
        public List<int> Status { get; set; }
    }
    public class GetAllGroupDefinitionsQueryHandler : IRequestHandler<GetAllGroupDefinitionsQuery, PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>>
    {
        private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
        private readonly IGroupInstanceRepositoryAsync _GroupInstanceRepositoryAsync;
        private readonly IGroupInstanceStudentRepositoryAsync _GroupInstanceStudentRepositoryAsync;
        private readonly IInterestedStudentRepositoryAsync _InterestedStudentRepositoryAsync;
        private readonly IOverPaymentStudentRepositoryAsync _OverPaymentStudentRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IGroupConditionPromoCodeRepositoryAsync _groupconditionpromocodeRepository;
        public GetAllGroupDefinitionsQueryHandler(IGroupDefinitionRepositoryAsync GroupDefinitionService, IMapper mapper,
            IGroupInstanceRepositoryAsync GroupInstanceRepositoryAsync,
            IGroupInstanceStudentRepositoryAsync GroupInstanceStudentRepositoryAsync,
            IInterestedStudentRepositoryAsync InterestedStudentRepositoryAsync,
            IOverPaymentStudentRepositoryAsync OverPaymentStudentRepositoryAsync,
            IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository)
        {
            _GroupDefinitionRepositoryAsync = GroupDefinitionService;
            _mapper = mapper;
            _GroupInstanceRepositoryAsync = GroupInstanceRepositoryAsync;
            _GroupInstanceStudentRepositoryAsync = GroupInstanceStudentRepositoryAsync;
            _InterestedStudentRepositoryAsync = InterestedStudentRepositoryAsync;
            _OverPaymentStudentRepositoryAsync = OverPaymentStudentRepositoryAsync;
            _groupconditionpromocodeRepository = groupconditionpromocodeRepository;

        }

        public async Task<PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>> Handle(GetAllGroupDefinitionsQuery request, CancellationToken cancellationToken)
        {
            // TODO: need to remove this nested loop.
            int totalCount = 0;
            var validFilter = _mapper.Map<RequestParameter>(request);
            IReadOnlyList<Domain.Entities.GroupDefinition> GroupDefinitions;
            GroupDefinitions = _GroupDefinitionRepositoryAsync.GetAll(request.PageNumber, request.PageSize, request.SubLevel, request.Status, out totalCount, request.SubLevelId);
            var groupDefinitionsModel = _mapper.Map<IEnumerable<GetAllGroupDefinitionViewModel>>(GroupDefinitions);
            foreach (var groupDefinition in groupDefinitionsModel)
            {
                groupDefinition.ActualTotalGroupInstances = _GroupInstanceRepositoryAsync.GetCountByGroupDefinitionId(groupDefinition.Id);
                groupDefinition.ActualTotalStudents = await _GroupInstanceStudentRepositoryAsync.GetCountOfStudentsByGroupDefinitionId(groupDefinition.Id);
                groupDefinition.TotalInterestedStudents = _InterestedStudentRepositoryAsync.GetCountOfStudentsByGroupDefinitionId(groupDefinition.Id);
                groupDefinition.TotalOverPaymentStudents = _OverPaymentStudentRepositoryAsync.GetCountOfStudentsByGroupDefinitionId(groupDefinition.Id);
                var promos = _groupconditionpromocodeRepository.GetAllByGroupCondition(groupDefinition.GroupConditionId);
                foreach (var promo in promos)
                {
                    var promocodemodel = promo.Select(x => new PromoCodeCountModel()
                    {
                        count = x.Count,
                        promocodeId = x.PromoCodeId,
                        PromoCodeName = x.PromoCode.Name
                    }).ToList();
                    groupDefinition.PromoCodes.Add(promocodemodel);
                }
            }
            return new PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>(groupDefinitionsModel, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
