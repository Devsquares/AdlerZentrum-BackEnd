using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CreateAdlerCardsBundleCommand : IRequest<Response<int>>
    {
		public int Count { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public double DiscountPrice { get; set; }
		public DateTime? AvailableFrom { get; set; }
		public  DateTime? AvailableTill { get; set; }
		public int Status { get; set; }
    }

    public class CreateAdlerCardsBundleCommandHandler : IRequestHandler<CreateAdlerCardsBundleCommand, Response<int>>
    {
        private readonly IAdlerCardsBundleRepositoryAsync _adlercardsbundleRepository;
        private readonly IMapper _mapper;
        public CreateAdlerCardsBundleCommandHandler(IAdlerCardsBundleRepositoryAsync adlercardsbundleRepository, IMapper mapper)
        {
            _adlercardsbundleRepository = adlercardsbundleRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateAdlerCardsBundleCommand request, CancellationToken cancellationToken)
        {
            var adlercardsbundle = _mapper.Map<Domain.Entities.AdlerCardsBundle>(request);
            await _adlercardsbundleRepository.AddAsync(adlercardsbundle);
            return new Response<int>(adlercardsbundle.Id);
        }
    }
}
