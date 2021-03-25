using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class ActivateAdlerCardCommand : IRequest<Response<int>>
    {
        public int AdlerCardId { get; set; }
        public class ActivateAdlerCardCommandHandler : IRequestHandler<ActivateAdlerCardCommand, Response<int>>
        {
            private readonly IAdlerCardRepositoryAsync _adlercardRepository;
            public ActivateAdlerCardCommandHandler(IAdlerCardRepositoryAsync adlercardRepository)
            {
                _adlercardRepository = adlercardRepository;
            }

            public async Task<Response<int>> Handle(ActivateAdlerCardCommand command, CancellationToken cancellationToken)
            {
                var adlerCard = _adlercardRepository.GetByIdAsync(command.AdlerCardId).Result;
                if(adlerCard == null)
                {
                    throw new ApiException("No AdlerCard Found");
                }
                adlerCard.Status = (int)AdlerCardEnum.Open;
                await _adlercardRepository.UpdateAsync(adlerCard);
                return new Response<int>(adlerCard.Id);
            }
        }
    }
}
