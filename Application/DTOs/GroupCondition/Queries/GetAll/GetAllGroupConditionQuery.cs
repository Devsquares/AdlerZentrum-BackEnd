using Application.DTOs.GroupConditionPromoCodeModel;
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
    public class GetAllGroupConditionsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllGroupConditionViewModel>>>
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
    public class GetAllGroupConditionsQueryHandler : IRequestHandler<GetAllGroupConditionsQuery, FilteredPagedResponse<IEnumerable<GetAllGroupConditionViewModel>>>
    {
        private readonly IGroupConditionRepositoryAsync _GroupConditionRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IGroupConditionDetailsRepositoryAsync _groupConditionDetailsRepository;
        private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepository;
        public GetAllGroupConditionsQueryHandler(IGroupConditionRepositoryAsync GroupConditionService, IMapper mapper,
                IGroupConditionDetailsRepositoryAsync groupConditionDetailsRepository,
                IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepository)
        {
            _GroupConditionRepositoryAsync = GroupConditionService;
            _mapper = mapper;
            _groupConditionDetailsRepository = groupConditionDetailsRepository;
            _groupConditionPromoCodeRepository = groupConditionPromoCodeRepository;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllGroupConditionViewModel>>> Handle(GetAllGroupConditionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<FilteredRequestParameter>(request);
            IReadOnlyList<Domain.Entities.GroupCondition> GroupConditions;
            GroupConditions = await _GroupConditionRepositoryAsync.GetPagedReponseAsync(validFilter);
            var userViewModel = _mapper.Map<IEnumerable<GetAllGroupConditionViewModel>>(GroupConditions);
            var ids = GroupConditions.Select(x => x.Id).ToList();
            var GroupConditionDetail = _groupConditionDetailsRepository.GetByGroupConditionIds(ids);
            var Promocodes = _groupConditionPromoCodeRepository.GetByGroupConditionDetailId(GroupConditionDetail);
            List<GroupConditionPromoCodeInputModel> promocodeslst = new List<GroupConditionPromoCodeInputModel>();
            foreach (var GroupConditionD in GroupConditionDetail)
            {
                var getpromoCodes = Promocodes.Where(x => x.GroupConditionDetails.GroupConditionId == GroupConditionD.GroupConditionId && x.GroupConditionDetailsId == GroupConditionD.Id).ToList();
                promocodeslst = new List<GroupConditionPromoCodeInputModel>();
                foreach (var promocode in getpromoCodes)
                {
                    promocodeslst.Add(new GroupConditionPromoCodeInputModel()
                    {
                        PromoCodeId = promocode.PromoCodeId,
                        Name = promocode.PromoCode.Name,
                        Count = promocode.Count
                    });
                }
                var view = userViewModel.Where(x => x.Id == GroupConditionD.GroupConditionId).FirstOrDefault();
                if (view != null)
                {
                    //if (view.PromoCodes == null)
                    //{
                    //    view.PromoCodes = new List<List<GroupConditionPromoCodeInputModel>>();
                    //}
                    view.PromoCodes.AddRange(new List<List<GroupConditionPromoCodeInputModel>>() { promocodeslst });
                }
            }
            return new FilteredPagedResponse<IEnumerable<GetAllGroupConditionViewModel>>(userViewModel, validFilter, userViewModel.ToList().Count);
        }
    }
}
