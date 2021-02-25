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
    public partial class CreateAdlerCardCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public int AdlerCardsUnitId { get; set; }
        public int QuestionId { get; set; }
        public int AllowedDuration { get; set; }
        public double TotalScore { get; set; }
        public int Status { get; set; }
        public int AdlerCardsTypeId { get; set; }
        public int LevelId { get; set; }
    }

    public class CreateAdlerCardCommandHandler : IRequestHandler<CreateAdlerCardCommand, Response<int>>
    {
        private readonly IAdlerCardRepositoryAsync _adlercardRepository;
        private readonly IMapper _mapper;
        public CreateAdlerCardCommandHandler(IAdlerCardRepositoryAsync adlercardRepository, IMapper mapper)
        {
            _adlercardRepository = adlercardRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateAdlerCardCommand request, CancellationToken cancellationToken)
        {
            var adlercard = _mapper.Map<Domain.Entities.AdlerCard>(request);
            await _adlercardRepository.AddAsync(adlercard);
            return new Response<int>(adlercard.Id);
        }
    }
}
