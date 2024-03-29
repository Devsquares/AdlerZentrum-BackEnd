﻿using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class CheckPromoCodeInstanceCommand : IRequest<Response<PromoCodeInstancesViewModel>>
    {
        public string PromoCodeKey { get; set; }
        public string StudentId { get; set; }
        public string StudentEmail { get; set; }
        public class CheckPromoCodeInstanceCommandHandler : IRequestHandler<CheckPromoCodeInstanceCommand, Response<PromoCodeInstancesViewModel>>
        {
            private readonly IPromoCodeInstanceRepositoryAsync _promoCodeInstanceRepository;
            public CheckPromoCodeInstanceCommandHandler(IPromoCodeInstanceRepositoryAsync promoCodeInstanceRepository)
            {
                _promoCodeInstanceRepository = promoCodeInstanceRepository;
            }
            public async Task<Response<PromoCodeInstancesViewModel>> Handle(CheckPromoCodeInstanceCommand query, CancellationToken cancellationToken)
            {
                var promoCodeInstance = _promoCodeInstanceRepository.GetByPromoCodeKey(query.PromoCodeKey,query.StudentId,query.StudentEmail);
                if (promoCodeInstance == null) return new Response<PromoCodeInstancesViewModel>("Promo Code Instance Not Found.)");
                if (promoCodeInstance.Id == 0) return new Response<PromoCodeInstancesViewModel>("Promo Code Instance Not Found.");
                if (promoCodeInstance.IsUsed)
                {
                    promoCodeInstance.IsValid = false;
                }
                else if (promoCodeInstance.EndDate < DateTime.Now)
                {
                    promoCodeInstance.IsValid = false;
                }
                else
                {
                    promoCodeInstance.IsValid = true;
                }
                return new Response<PromoCodeInstancesViewModel>(promoCodeInstance);
            }
        }
    }
}
