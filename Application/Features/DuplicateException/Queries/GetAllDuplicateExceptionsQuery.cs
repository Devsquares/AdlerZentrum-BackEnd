using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetAllDuplicateExceptionsQuery : IRequest<PagedResponse<IEnumerable<DuplicateException>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllDuplicateExceptionsQueryHandler : IRequestHandler<GetAllDuplicateExceptionsQuery, PagedResponse<IEnumerable<DuplicateException>>>
    {
        private readonly IDuplicateExceptionRepositoryAsync _disqualificationrequestRepository;
        private readonly IMapper _mapper;
        public GetAllDuplicateExceptionsQueryHandler(IDuplicateExceptionRepositoryAsync disqualificationrequestRepository, IMapper mapper)
        {
            _disqualificationrequestRepository = disqualificationrequestRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<DuplicateException>>> Handle(GetAllDuplicateExceptionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            int count = _disqualificationrequestRepository.GetCount();

            var disqualificationrequest = await _disqualificationrequestRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var disqualificationrequestViewModel = _mapper.Map<IEnumerable<DuplicateException>>(disqualificationrequest);
            return new PagedResponse<IEnumerable<DuplicateException>>(disqualificationrequestViewModel, validFilter.PageNumber, validFilter.PageSize, count);
        }
    } 
}
