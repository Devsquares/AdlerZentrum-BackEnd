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
    public class DeleteAdlerCardByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteAdlerCardByIdCommandHandler : IRequestHandler<DeleteAdlerCardByIdCommand, Response<int>>
        {
            private readonly IAdlerCardRepositoryAsync _adlercardRepository;
            public DeleteAdlerCardByIdCommandHandler(IAdlerCardRepositoryAsync adlercardRepository)
            {
                _adlercardRepository = adlercardRepository;
            }
            public async Task<Response<int>> Handle(DeleteAdlerCardByIdCommand command, CancellationToken cancellationToken)
            {
                var adlercard = await _adlercardRepository.GetByIdAsync(command.Id);
                if (adlercard == null) throw new ApiException($"AdlerCard Not Found.");
                if (adlercard.Status != (int)AdlerCardEnum.Draft) throw new ApiException("Cann't edit adler card.");
                await _adlercardRepository.DeleteAsync(adlercard);
                return new Response<int>(adlercard.Id);
            }
        }
    }
}
