using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PromoCodeInstance.Queries.GetAllPromoCodeInstances
{
    public class GetAllPromoCodeInstancesQuery : IRequest<PagedResponse<List<PromoCodeInstancesViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? promocodeId { get; set; }
        public bool? isValid { get; set; }
        public string promoCodeName { get; set; }
        public string studentName { get; set; }
        //public Dictionary<string, string> FilterValue { get; set; }
        //public Dictionary<string, string> FilterRange { get; set; }
        //public Dictionary<string, List<string>> FilterArray { get; set; }
        //public Dictionary<string, string> FilterSearch { get; set; }
        //public string SortBy { get; set; }
        //public string SortType { get; set; }
        //public bool NoPaging { get; set; }
    }
    public class GetAllPromoCodeInstancesQueryHandler : IRequestHandler<GetAllPromoCodeInstancesQuery, PagedResponse<List<PromoCodeInstancesViewModel>>>
    {
        private readonly IPromoCodeInstanceRepositoryAsync _promocodeinstanceRepository;
        private readonly IMapper _mapper;
        public GetAllPromoCodeInstancesQueryHandler(IPromoCodeInstanceRepositoryAsync promocodeinstanceRepository, IMapper mapper)
        {
            _promocodeinstanceRepository = promocodeinstanceRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<PromoCodeInstancesViewModel>>> Handle(GetAllPromoCodeInstancesQuery request, CancellationToken cancellationToken)
        {
            int count = 0;
            var promocodesInstances = _promocodeinstanceRepository.GetAllReport(request.PageNumber, request.PageSize, out count, request.promocodeId, request.isValid, request.promoCodeName, request.studentName);
            foreach (var item in promocodesInstances)
            {
                if (item.IsUsed)
                {
                    item.IsValid = false;
                }
                else if (item.EndDate < DateTime.Now)
                {
                    item.IsValid = false;
                }
                else
                {
                    item.IsValid = true;
                }
            }
            return  new PagedResponse<List<PromoCodeInstancesViewModel>>(promocodesInstances,request.PageNumber,request.PageSize,count);
            //var validFilter = _mapper.Map<GetAllPromoCodeInstancesParameter>(request);
            //FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            //Reflection.CopyProperties(validFilter, filteredRequestParameter);
            //int count = _promocodeinstanceRepository.GetCount(validFilter);

            //var promocodeinstance = await _promocodeinstanceRepository.GetPagedReponseAsync(validFilter);
            //var promocodeinstanceViewModel = _mapper.Map<IEnumerable<GetAllPromoCodeInstancesViewModel>>(promocodeinstance);
            //return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllPromoCodeInstancesViewModel>>(promocodeinstanceViewModel, validFilter, count);
        }
    }
}
