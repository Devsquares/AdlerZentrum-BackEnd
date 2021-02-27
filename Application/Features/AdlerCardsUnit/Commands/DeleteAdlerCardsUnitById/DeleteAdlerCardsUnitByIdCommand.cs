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
    public class DeleteAdlerCardsUnitByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteAdlerCardsUnitByIdCommandHandler : IRequestHandler<DeleteAdlerCardsUnitByIdCommand, Response<int>>
        {
            private readonly IAdlerCardsUnitRepositoryAsync _adlercardsunitRepository;
            private readonly IAdlerCardRepositoryAsync _adlercardRepository;
            public DeleteAdlerCardsUnitByIdCommandHandler(IAdlerCardsUnitRepositoryAsync adlercardsunitRepository,
                IAdlerCardRepositoryAsync adlercardRepository)
            {
                _adlercardsunitRepository = adlercardsunitRepository;
                _adlercardRepository = adlercardRepository;
            }
            public async Task<Response<int>> Handle(DeleteAdlerCardsUnitByIdCommand command, CancellationToken cancellationToken)
            {
                var adlercardsunit = await _adlercardsunitRepository.GetByIdAsync(command.Id);
                if (adlercardsunit == null) throw new ApiException($"AdlerCardsUnit Not Found.");
                var adlerCards = _adlercardRepository.GetAllByUnitId(command.Id);
                if(adlerCards != null && adlerCards.Count >0)
                {
                    throw new ApiException($"Can't delete this unit because it contains alder cards ");
                }
                await _adlercardsunitRepository.DeleteAsync(adlercardsunit);
                return new Response<int>(adlercardsunit.Id);
            }
        }
    }
}
