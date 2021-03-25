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
    public class DeletePromoCodeInstanceByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeletePromoCodeInstanceByIdCommandHandler : IRequestHandler<DeletePromoCodeInstanceByIdCommand, Response<int>>
        {
            private readonly IPromoCodeInstanceRepositoryAsync _promocodeinstanceRepository;
            public DeletePromoCodeInstanceByIdCommandHandler(IPromoCodeInstanceRepositoryAsync promocodeinstanceRepository)
            {
                _promocodeinstanceRepository = promocodeinstanceRepository;
            }
            public async Task<Response<int>> Handle(DeletePromoCodeInstanceByIdCommand command, CancellationToken cancellationToken)
            {
                var promocodeinstance = await _promocodeinstanceRepository.GetByIdAsync(command.Id);
                if (promocodeinstance == null) throw new ApiException($"PromoCodeInstance Not Found.");
                await _promocodeinstanceRepository.DeleteAsync(promocodeinstance);
                return new Response<int>(promocodeinstance.Id);
            }
        }
    }
}
