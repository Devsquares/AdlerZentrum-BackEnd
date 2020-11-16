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

namespace Application.Features.Address.Queries.GetAllAddresses
{
    public class GetAllAddressesQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllAddressesViewModel>>>
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
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, FilteredPagedResponse<IEnumerable<GetAllAddressesViewModel>>>
    {
        private readonly IAddressRepositoryAsync _addressRepository;
        private readonly IMapper _mapper;
        public GetAllAddressesQueryHandler(IAddressRepositoryAsync addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllAddressesViewModel>>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllAddressesParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _addressRepository.GetCount(validFilter);
            var address = await _addressRepository.GetPagedReponseAsync(validFilter);
            var addressViewModel = _mapper.Map<IEnumerable<GetAllAddressesViewModel>>(address);
            return new FilteredPagedResponse<IEnumerable<GetAllAddressesViewModel>>(addressViewModel, validFilter, count);
        }
    }
}
