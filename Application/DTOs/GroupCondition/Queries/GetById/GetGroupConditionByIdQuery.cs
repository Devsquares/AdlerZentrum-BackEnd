using Application.DTOs.GroupConditionPromoCodeModel;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetGroupConditionByIdQuery : IRequest<Response<GetAllGroupConditionViewModel>>
    {
        public int Id { get; set; }
        public class GetGroupConditionByIdQueryHandler : IRequestHandler<GetGroupConditionByIdQuery, Response<GetAllGroupConditionViewModel>>
        {
            private readonly IGroupConditionRepositoryAsync _GroupConditionRepository;
            private readonly IGroupConditionDetailsRepositoryAsync _groupConditionDetailsRepository;
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepository;
            private readonly IMapper _mapper;
            public GetGroupConditionByIdQueryHandler(IGroupConditionRepositoryAsync GroupConditionRepository,
                IGroupConditionDetailsRepositoryAsync groupConditionDetailsRepository,
                IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepository,
                IMapper mapper)
            {
                _GroupConditionRepository = GroupConditionRepository;
                _groupConditionDetailsRepository = groupConditionDetailsRepository;
                _groupConditionPromoCodeRepository = groupConditionPromoCodeRepository;
                _mapper = mapper;
            }
            public async Task<Response<GetAllGroupConditionViewModel>> Handle(GetGroupConditionByIdQuery query, CancellationToken cancellationToken)
            {
                var GroupCondition = await _GroupConditionRepository.GetByIdAsync(query.Id);
                if (GroupCondition == null) throw new ApiException($"Group Not Found.");
                var GroupConditionDetail = _groupConditionDetailsRepository.GetByGroupConditionId(GroupCondition.Id);
                var Promocodes = _groupConditionPromoCodeRepository.GetByGroupConditionDetailId(GroupConditionDetail);
                var groupConditionViewModel = _mapper.Map<GetAllGroupConditionViewModel>(GroupCondition);
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
                    groupConditionViewModel.PromoCodes.AddRange(new List<List<GroupConditionPromoCodeInputModel>>() { promocodeslst });
                }
                return new Response<GetAllGroupConditionViewModel>(groupConditionViewModel);
            }
        }
    }
}
