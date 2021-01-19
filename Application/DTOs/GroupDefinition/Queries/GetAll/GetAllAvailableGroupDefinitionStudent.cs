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
    public class GetAllAvailableGroupDefinitionStudent : IRequest<List<GetAllGroupDefinitionViewModel>>
    {
        public int SubLevelId { get; set; }
        public int? PromoCodeId { get; set; } 
    }
    public class GetAllAvailableGroupDefinitionStudentHandler : IRequestHandler<GetAllAvailableGroupDefinitionStudent, List<GetAllGroupDefinitionViewModel>>
    {
        private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IGroupConditionPromoCodeRepositoryAsync _groupconditionpromocodeRepository;
        private readonly IGroupInstanceRepositoryAsync _groupinstanceRepository;
        public GetAllAvailableGroupDefinitionStudentHandler(IGroupDefinitionRepositoryAsync GroupDefinitionService, IMapper mapper,
            IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository,
            IGroupInstanceRepositoryAsync groupinstanceRepository)
        {
            _GroupDefinitionRepositoryAsync = GroupDefinitionService;
            _mapper = mapper;
            _groupconditionpromocodeRepository = groupconditionpromocodeRepository;
            _groupinstanceRepository = groupinstanceRepository;
        }

        public async Task<List<GetAllGroupDefinitionViewModel>> Handle(GetAllAvailableGroupDefinitionStudent request, CancellationToken cancellationToken)
        {
            List<GetAllGroupDefinitionViewModel> GroupDefinitionViewModelList = new List<GetAllGroupDefinitionViewModel>();
            var groupDefinitions = _GroupDefinitionRepositoryAsync.GetAvailableGroupDefinitionsForStudent(request.SubLevelId);
            bool canApplyInGroupInstance = false;
            foreach (var groupDefinition in groupDefinitions)
            {
                var groupinstance = _groupinstanceRepository.GetByGroupDefinitionId(groupDefinition.Id);
                if(groupinstance == null)
                {
                    continue;
                }
                if (request.PromoCodeId.HasValue)
                {
                    canApplyInGroupInstance = _groupconditionpromocodeRepository.CheckPromoCodeCountInGroupInstance(groupinstance.Id, request.PromoCodeId.Value);
                }
                var GroupDefinitionViewModel = _mapper.Map<GetAllGroupDefinitionViewModel>(groupDefinition);
                GroupDefinitionViewModel.IsInterested = canApplyInGroupInstance;
                GroupDefinitionViewModelList.Add(GroupDefinitionViewModel);
            }
            return GroupDefinitionViewModelList;
            //var validFilter = _mapper.Map<RequestParameter>(request);
            //IReadOnlyList<Domain.Entities.GroupDefinition> GroupDefinitions;
            //GroupDefinitions = await _GroupDefinitionRepositoryAsync.GetPagedReponseAsync(request.PageNumber, request.PageSize);
            //var userViewModel = _mapper.Map<IEnumerable<GetAllGroupDefinitionViewModel>>(GroupDefinitions);
            //return new PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>(userViewModel, request.PageNumber, request.PageSize, _GroupDefinitionRepositoryAsync.GetCount());
        }
    }
}
