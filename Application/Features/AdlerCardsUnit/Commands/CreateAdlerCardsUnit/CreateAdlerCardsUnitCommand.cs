using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CreateAdlerCardsUnitCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public int LevelId { get; set; }
        public int AdlerCardsTypeId { get; set; }
        public int Order { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }

    public class CreateAdlerCardsUnitCommandHandler : IRequestHandler<CreateAdlerCardsUnitCommand, Response<int>>
    {
        private readonly IAdlerCardsUnitRepositoryAsync _adlercardsunitRepository;
        private readonly IMapper _mapper;
        public CreateAdlerCardsUnitCommandHandler(IAdlerCardsUnitRepositoryAsync adlercardsunitRepository, IMapper mapper)
        {
            _adlercardsunitRepository = adlercardsunitRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateAdlerCardsUnitCommand request, CancellationToken cancellationToken)
        {
            var adlercardsunit = _mapper.Map<Domain.Entities.AdlerCardsUnit>(request);
            await _adlercardsunitRepository.AddAsync(adlercardsunit);
            return new Response<int>(adlercardsunit.Id);
        }
    }
}
