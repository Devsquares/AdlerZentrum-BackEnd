using Application.DTOs.GroupInstance.Queries;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetGroupByPromoCodeQuery : IRequest<Response<GetAllGroupInstancesViewModel>>
    {
        public string name { get; set; }
        public class GetLevelByIdQueryHandler : IRequestHandler<GetGroupByPromoCodeQuery, Response<GetAllGroupInstancesViewModel>>
        {
            private readonly IPromoCodeRepositoryAsync _promoCodeRepository;
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepository;
            private readonly IMapper _mapper;
            public GetLevelByIdQueryHandler(IPromoCodeRepositoryAsync promoCodeRepository,
                IGroupInstanceRepositoryAsync groupInstanceRepository, IMapper mapper)
            {
                _promoCodeRepository = promoCodeRepository;
                _groupInstanceRepository = groupInstanceRepository;
                _mapper = mapper;
            }
            public async Task<Response<GetAllGroupInstancesViewModel>> Handle(GetGroupByPromoCodeQuery query, CancellationToken cancellationToken)
            {
                var promoCode = _promoCodeRepository.GetByName(query.name);
                if(promoCode == null) throw new ApiException($"Promo Code Invaild.");
                if (promoCode.GroupId != null)
                {
                    var groupInstance = await _groupInstanceRepository.GetByIdAsync(promoCode.GroupId.Value);
                    if (groupInstance == null) throw new ApiException($"Promo Code Not Found.");
                    var userViewModel = _mapper.Map<GetAllGroupInstancesViewModel>(groupInstance);
                    return new Response<GetAllGroupInstancesViewModel>(userViewModel);
                }
                else
                {
                    throw new ApiException($"Promo Code Invaild.");
                }
            }
        }
    }
}
