using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.Level.Queries
{
    public class GetAllPromoCodesQuery : IRequest<PagedResponse<IEnumerable<GetAllPromoCodesViewModel>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class GetAllPromoCodesQueryHandler : IRequestHandler<GetAllPromoCodesQuery, PagedResponse<IEnumerable<GetAllPromoCodesViewModel>>>
    {
        private readonly IPromoCodeRepositoryAsync _promoCodeRepository;
        private readonly IMapper _mapper;
        public GetAllPromoCodesQueryHandler(IPromoCodeRepositoryAsync promoCodeRepository, IMapper mapper)
        {
            _promoCodeRepository = promoCodeRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllPromoCodesViewModel>>> Handle(GetAllPromoCodesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var user = await _promoCodeRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var userViewModel = _mapper.Map<IEnumerable<GetAllPromoCodesViewModel>>(user);
            return new PagedResponse<IEnumerable<GetAllPromoCodesViewModel>>(userViewModel, validFilter.PageNumber, validFilter.PageSize,_promoCodeRepository.GetCount());
        }
    }
}
