using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CreatePromoCodeInstanceCommand : IRequest<Response<int>>
    {
		public int PromoCodeId { get; set; }
		//public PromoCode PromoCode { get; set; }
		//public string PromoCodeKey { get; set; }
		//public bool IsUsed { get; set; }
		public string StudentId { get; set; }
        public string StudentEmail { get; set; }
        //public ApplicationUser Student { get; set; }
        public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

        public int? count { get; set; }
        public int? GroupDefinitionId { get; set; }
    }

    public class CreatePromoCodeInstanceCommandHandler : IRequestHandler<CreatePromoCodeInstanceCommand, Response<int>>
    {
        private readonly IPromoCodeInstanceRepositoryAsync _promocodeinstanceRepository;
        private readonly IGroupDefinitionRepositoryAsync _groupDefinitionRepositoryAsync;
        private readonly IPromoCodeRepositoryAsync _promocodeRepository;
        private readonly IMapper _mapper;
        public CreatePromoCodeInstanceCommandHandler(IPromoCodeInstanceRepositoryAsync promocodeinstanceRepository, IMapper mapper,
            IPromoCodeRepositoryAsync promocodeRepository, IGroupDefinitionRepositoryAsync groupDefinitionRepositoryAsync)
        {
            _promocodeinstanceRepository = promocodeinstanceRepository;
            _mapper = mapper;
            _promocodeRepository = promocodeRepository;
            _groupDefinitionRepositoryAsync = groupDefinitionRepositoryAsync;
        }

        public async Task<Response<int>> Handle(CreatePromoCodeInstanceCommand request, CancellationToken cancellationToken)
        {
            var promoCode = _promocodeRepository.GetByIdAsync(request.PromoCodeId).Result;
            if(promoCode == null)
            {
                throw new ApiException("Promocode Not Found");
            }
            if (request.GroupDefinitionId.HasValue)
            {
                var groupDefinition = _groupDefinitionRepositoryAsync.GetByIdAsync(request.GroupDefinitionId.Value).Result;
                if (groupDefinition == null)
                {
                    throw new ApiException("GroupDefinition Not Found");
                }
            }

            var promocodeinstance = new Domain.Entities.PromoCodeInstance();
            if (request.count != null && request.count > 0)
            {
                var promocodeinstances = new List<Domain.Entities.PromoCodeInstance>();
                for (int i = 0; i < request.count; i++)
                {
                    promocodeinstance = new Domain.Entities.PromoCodeInstance();
                    Reflection.CopyProperties(request, promocodeinstance);
                    promocodeinstance.IsUsed = false;
                    promocodeinstance.PromoCodeKey = HelperUtilities.RandomString(10);
                    promocodeinstances.Add(promocodeinstance);
                }
                await _promocodeinstanceRepository.AddBulkAsync(promocodeinstances);
            }
            else
            {
               
                Reflection.CopyProperties(request, promocodeinstance);
                promocodeinstance.IsUsed = false;
                promocodeinstance.PromoCodeKey = HelperUtilities.RandomString(10);
                await _promocodeinstanceRepository.AddAsync(promocodeinstance);
                
            }
            return new Response<int>(promocodeinstance.Id);
        }
    }
}
