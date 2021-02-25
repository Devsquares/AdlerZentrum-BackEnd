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
	public class UpdateAdlerCardsBundleCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public int Count { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public double DiscountPrice { get; set; }
		public DateTime? AvailableFrom { get; set; }
		public  DateTime? AvailableTill { get; set; }
		public int Status { get; set; }

        public class UpdateAdlerCardsBundleCommandHandler : IRequestHandler<UpdateAdlerCardsBundleCommand, Response<int>>
        {
            private readonly IAdlerCardsBundleRepositoryAsync _adlercardsbundleRepository;
            public UpdateAdlerCardsBundleCommandHandler(IAdlerCardsBundleRepositoryAsync adlercardsbundleRepository)
            {
                _adlercardsbundleRepository = adlercardsbundleRepository;
            }
            public async Task<Response<int>> Handle(UpdateAdlerCardsBundleCommand command, CancellationToken cancellationToken)
            {
                var adlercardsbundle = await _adlercardsbundleRepository.GetByIdAsync(command.Id);

                if (adlercardsbundle == null)
                {
                    throw new ApiException($"AdlerCardsBundle Not Found.");
                }
                else
                {
				adlercardsbundle.Count = command.Count;
				adlercardsbundle.Name = command.Name;
				adlercardsbundle.Price = command.Price;
				adlercardsbundle.DiscountPrice = command.DiscountPrice;
				adlercardsbundle.AvailableFrom = command.AvailableFrom;
				adlercardsbundle.AvailableTill = command.AvailableTill;
				adlercardsbundle.Status = command.Status; 

                    await _adlercardsbundleRepository.UpdateAsync(adlercardsbundle);
                    return new Response<int>(adlercardsbundle.Id);
                }
            }
        }

    }
}
