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
    public class DeleteAdlerCardsBundleByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteAdlerCardsBundleByIdCommandHandler : IRequestHandler<DeleteAdlerCardsBundleByIdCommand, Response<int>>
        {
            private readonly IAdlerCardsBundleRepositoryAsync _adlercardsbundleRepository;
            public DeleteAdlerCardsBundleByIdCommandHandler(IAdlerCardsBundleRepositoryAsync adlercardsbundleRepository)
            {
                _adlercardsbundleRepository = adlercardsbundleRepository;
            }
            public async Task<Response<int>> Handle(DeleteAdlerCardsBundleByIdCommand command, CancellationToken cancellationToken)
            {
                var adlercardsbundle = await _adlercardsbundleRepository.GetByIdAsync(command.Id);
                if (adlercardsbundle == null) throw new ApiException($"AdlerCardsBundle Not Found.");
                await _adlercardsbundleRepository.DeleteAsync(adlercardsbundle);
                return new Response<int>(adlercardsbundle.Id);
            }
        }
    }
}
